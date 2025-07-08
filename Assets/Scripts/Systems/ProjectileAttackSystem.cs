using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

partial struct ProjectileAttackSystem : ISystem {

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        EntitiesReferences entitiesReferences = SystemAPI.GetSingleton<EntitiesReferences>();
        foreach ((
            RefRW<ProjectileAttack> projectileAttack,
            RefRO<Target> target,
            RefRO<LocalTransform> localTransform,
            RefRW<UnitMovement> unitMovement
        )
        in
        SystemAPI.Query<
            RefRW<ProjectileAttack>,
            RefRO<Target>,
            RefRO<LocalTransform>,
            RefRW<UnitMovement>
        >()){
            if(target.ValueRO.targetEntity == Entity.Null){
                continue;
            }

            LocalTransform targetLocalTransform = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.targetEntity);

            if(math.distance(localTransform.ValueRO.Position, targetLocalTransform.Position) > projectileAttack.ValueRO.attackDistance){
                // Target is too far, move closer
                unitMovement.ValueRW.targetPosition = targetLocalTransform.Position;
                continue;
            }else{
                // Close enough to target, so stop moving
                unitMovement.ValueRW.targetPosition = localTransform.ValueRO.Position;
            }

            projectileAttack.ValueRW.timer -= SystemAPI.Time.DeltaTime;
            if(projectileAttack.ValueRO.timer > 0){
                continue;
            }
            projectileAttack.ValueRW.timer = projectileAttack.ValueRO.timerMax;

            

            Entity projectileEntity = state.EntityManager.Instantiate(entitiesReferences.projectilePrefabEntity);
            float3 projectileSpawnWorldPosition = localTransform.ValueRO.TransformPoint(projectileAttack.ValueRO.projectileSpawnLocalPosition);
            SystemAPI.SetComponent(projectileEntity, LocalTransform.FromPosition(projectileSpawnWorldPosition));
            RefRW<Projectile> projectileProjectile = SystemAPI.GetComponentRW<Projectile>(projectileEntity);
            projectileProjectile.ValueRW.damage = projectileAttack.ValueRO.damage;
            RefRW<Target> projectileTarget = SystemAPI.GetComponentRW<Target>(projectileEntity);
            projectileTarget.ValueRW.targetEntity = target.ValueRO.targetEntity;

        }
    }

}
