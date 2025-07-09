using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class MovementOverrideAuthoring : MonoBehaviour{

    public class Baker : Baker<MovementOverrideAuthoring> {

        public override void Bake(MovementOverrideAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new MovementOverride());
            SetComponentEnabled<MovementOverride>(entity, false);
        }

    }

}

public struct MovementOverride : IComponentData, IEnableableComponent {
    public float3 targetPosition;
}
