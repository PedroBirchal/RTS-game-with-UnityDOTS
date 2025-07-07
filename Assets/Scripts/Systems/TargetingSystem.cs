using Unity.Collections;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Physics;

partial struct TargetingSystem : ISystem {

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        PhysicsWorldSingleton physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        CollisionWorld collisionWorld = physicsWorldSingleton.CollisionWorld;
        NativeList<DistanceHit> distanceHitList = new NativeList<DistanceHit>(Allocator.Temp);
        foreach (( 
                RefRO<LocalTransform> localTransform,
                RefRW<Targeting> targeting,
                RefRW<Target> target
            )
            in
            SystemAPI.Query<
                RefRO<LocalTransform>, 
                RefRW<Targeting>,
                RefRW<Target>
            >()){

            targeting.ValueRW.timer -= SystemAPI.Time.DeltaTime;
            if(targeting.ValueRO.timer > 0){
                continue;
            }
            targeting.ValueRW.timer = targeting.ValueRO.timerMax;

            distanceHitList.Clear();
            CollisionFilter collisionFilter = new CollisionFilter{
                BelongsTo = ~0u,
                CollidesWith = 1u << GameAssets.UNITS_LAYER,
                GroupIndex = 0,
            };
            if(collisionWorld.OverlapSphere(localTransform.ValueRO.Position, targeting.ValueRO.range, ref distanceHitList, collisionFilter)){
                foreach( DistanceHit distanceHit in distanceHitList){
                    Unit targetUnit = SystemAPI.GetComponent<Unit>(distanceHit.Entity);
                    if(targetUnit.faction == targeting.ValueRO.targetFaction){
                        target.ValueRW.targetEntity = distanceHit.Entity;
                        break;
                    }
                }
            }
        }
    }

}
