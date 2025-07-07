using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;


[UpdateInGroup(typeof(LateSimulationSystemGroup))]
[UpdateBefore(typeof(ResetEventsSystem))]
partial struct SelectedVisualSystem : ISystem {

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        foreach(RefRO<Selected> selected in SystemAPI.Query<RefRO<Selected>>().WithPresent<Selected>()) {

            if (selected.ValueRO.onDeselected) {
                RefRW<LocalTransform> indicatorLocalTransform = SystemAPI.GetComponentRW<LocalTransform>(selected.ValueRO.visualIndicator);
                indicatorLocalTransform.ValueRW.Scale = 0.0f;
            }
            if(selected.ValueRO.onSelected) {
                RefRW<LocalTransform> indicatorLocalTransform = SystemAPI.GetComponentRW<LocalTransform>(selected.ValueRO.visualIndicator);
                indicatorLocalTransform.ValueRW.Scale = selected.ValueRO.scale;
            }
            
        }
    }


}
