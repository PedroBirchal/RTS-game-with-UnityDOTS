using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

partial struct MovementOverrideSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        foreach((
            RefRO<LocalTransform> localTransform,
            RefRO<MovementOverride> movementOverride,
            EnabledRefRW<MovementOverride> movementOverrideEnabled,
            RefRW<UnitMovement> unitMovement
            ) in SystemAPI.Query<
                RefRO<LocalTransform>,
                RefRO<MovementOverride>,
                EnabledRefRW<MovementOverride>,
                RefRW<UnitMovement>
            >()){

                if(math.distancesq(localTransform.ValueRO.Position, movementOverride.ValueRO.targetPosition) > UnitMovementSystem.REACHED_TARGET_POSITION_DISTANCE_SQ){
                    // Move Closer
                    unitMovement.ValueRW.targetPosition = movementOverride.ValueRO.targetPosition;
                }
                else{
                    // Reached the override position.
                    movementOverrideEnabled.ValueRW = false;
                }
        }
    }
}
