using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

partial struct SetupUnitMovementDefaultPositionSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        /*
        EntityCommandBuffer entityCommandBuffer = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        foreach((
            RefRO<LocalTransform> localTransform, 
            RefRW<UnitMovement> unitMovement,
            RefRO<SetupUnitMovementDefaultPosition> setupUnitMovementDefaultPosition,
            Entity entity
        ) in SystemAPI.Query<
            RefRO<LocalTransform>,
            RefRW<UnitMovement>,
            RefRO<SetupUnitMovementDefaultPosition>
        >().WithEntityAccess()){
            unitMovement.ValueRW.targetPosition = localTransform.ValueRO.Position;
            entityCommandBuffer.RemoveComponent<SetupUnitMovementDefaultPosition>(entity);
        }
        */

        SetupUnitMovementDefaultPositionJob setupUnitMovementDefaultPositionJob = new SetupUnitMovementDefaultPositionJob();
        setupUnitMovementDefaultPositionJob.ScheduleParallel();
        
        EntityCommandBuffer entityCommandBuffer = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        foreach((
            RefRO<SetupUnitMovementDefaultPosition> setupUnitMovementDefaultPosition,
            Entity entity
        ) in SystemAPI.Query<
            RefRO<SetupUnitMovementDefaultPosition>
        >().WithEntityAccess()){
            entityCommandBuffer.RemoveComponent<SetupUnitMovementDefaultPosition>(entity);
        }
    }

}

[BurstCompile]

public partial struct SetupUnitMovementDefaultPositionJob : IJobEntity {
    public void Execute(in LocalTransform localTransform, ref UnitMovement unitMovement){
        unitMovement.targetPosition = localTransform.Position;
    }
}
