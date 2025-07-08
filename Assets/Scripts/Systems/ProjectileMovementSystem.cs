using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

partial struct ProjectileMovementSystem : ISystem {
    [BurstCompile]
    public void OnUpdate(ref SystemState state) {

        EntityCommandBuffer entityCommandBuffer = SystemAPI.GetSingleton<EndFixedStepSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        foreach( (
                RefRW<LocalTransform> localTransform,
                RefRO<Projectile> projectile,
                RefRO<Target> target,
                Entity entity
            )
            in SystemAPI.Query<
                RefRW<LocalTransform>,
                RefRO<Projectile>,
                RefRO<Target>
            >().WithEntityAccess()) {
            if(target.ValueRO.targetEntity == Entity.Null){
                entityCommandBuffer.DestroyEntity(entity);
                continue;
            }
            LocalTransform targetLocalTransform = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.targetEntity);
            ShootVictim targetShootVictim = SystemAPI.GetComponent<ShootVictim>(target.ValueRO.targetEntity);
            float3 targetPosition = targetLocalTransform.TransformPoint(targetShootVictim.hitLocalPosition);

            float distanceBeforeSq = math.distancesq(localTransform.ValueRO.Position, targetPosition);

            float3 moveDirection = targetPosition - localTransform.ValueRO.Position;
            moveDirection = math.normalize(moveDirection);
            localTransform.ValueRW.Position += moveDirection * projectile.ValueRO.speed * SystemAPI.Time.DeltaTime;

            float distanceAfterSq = math.distancesq(localTransform.ValueRO.Position, targetPosition);

            if(distanceAfterSq > distanceBeforeSq){
                // Projectile has overshot the target.
                localTransform.ValueRW.Position = targetPosition;
            }

            float destroyDistanceSq = 0.2f;
            if(math.distancesq(localTransform.ValueRO.Position, targetPosition) < destroyDistanceSq) {
                RefRW<Health> targetHealth = SystemAPI.GetComponentRW<Health>(target.ValueRO.targetEntity);
                targetHealth.ValueRW.health -= projectile.ValueRO.damage;

                entityCommandBuffer.DestroyEntity(entity);
            }
        }
    }
}
