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

            float distanceBeforeSq = math.distancesq(localTransform.ValueRO.Position, targetLocalTransform.Position);

            float3 moveDirection = targetLocalTransform.Position - localTransform.ValueRO.Position;
            moveDirection = math.normalize(moveDirection);
            localTransform.ValueRW.Position += moveDirection * projectile.ValueRO.speed * SystemAPI.Time.DeltaTime;

            float distanceAfterSq = math.distancesq(localTransform.ValueRO.Position, targetLocalTransform.Position);

            if(distanceAfterSq > distanceBeforeSq){
                // Projectile has overshot the target.
                localTransform.ValueRW.Position = targetLocalTransform.Position;
            }

            float destroyDistanceSq = 0.2f;
            if(math.distancesq(localTransform.ValueRO.Position, targetLocalTransform.Position) < destroyDistanceSq) {
                RefRW<Health> targetHealth = SystemAPI.GetComponentRW<Health>(target.ValueRO.targetEntity);
                targetHealth.ValueRW.health -= projectile.ValueRO.damage;

                entityCommandBuffer.DestroyEntity(entity);
            }
        }
    }
}
