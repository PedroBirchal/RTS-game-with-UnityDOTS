using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

partial struct SelectedVisualSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach(RefRO<Selected> selected in SystemAPI.Query<RefRO<Selected>>()){
            RefRW<LocalTransform> indicatorLocalTransform = SystemAPI.GetComponentRW<LocalTransform>(selected.ValueRO.visualIndicator);
            indicatorLocalTransform.ValueRW.Scale = selected.ValueRO.scale;
        }

        foreach(RefRO<Selected> selected in SystemAPI.Query<RefRO<Selected>>().WithDisabled<Selected>()){
            RefRW<LocalTransform> indicatorLocalTransform = SystemAPI.GetComponentRW<LocalTransform>(selected.ValueRO.visualIndicator);
            indicatorLocalTransform.ValueRW.Scale = 0.0f;
        }
    }
}
