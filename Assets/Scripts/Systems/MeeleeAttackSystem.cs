using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateAfter(typeof(RandomWalkingSystem))]
partial struct MeeleeAttackSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach((
            RefRO<LocalTransform> localTransform,
            RefRW<MeeleeAttack> meeleeAttack,
            RefRO<Target> target,
            RefRW<UnitMovement> unitMovement
        )
        in SystemAPI.Query<
            RefRO<LocalTransform>,
            RefRW<MeeleeAttack>,
            RefRO<Target>,
            RefRW<UnitMovement>>().WithDisabled<MovementOverride>()){

            if(target.ValueRO.targetEntity == Entity.Null){
                continue;
            }
            
            LocalTransform targetLocalTransform = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.targetEntity);
            float meeleeAttackDistanceSq = 2f;
            if(math.distancesq(localTransform.ValueRO.Position, targetLocalTransform.Position) > meeleeAttackDistanceSq){
                //Too far
                unitMovement.ValueRW.targetPosition = targetLocalTransform.Position;
            }
            else{
                //Close enough
                unitMovement.ValueRW.targetPosition = localTransform.ValueRO.Position;

                meeleeAttack.ValueRW.timer -= SystemAPI.Time.DeltaTime;
                if(meeleeAttack.ValueRO.timer > 0){
                    continue;
                }
                meeleeAttack.ValueRW.timer = meeleeAttack.ValueRO.timerMax;

                RefRW<Health> targetHealth = SystemAPI.GetComponentRW<Health>(target.ValueRO.targetEntity);
                targetHealth.ValueRW.health -= 1;

            }
        }       
    }


}
