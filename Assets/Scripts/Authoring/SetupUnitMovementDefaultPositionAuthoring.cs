using UnityEngine;
using Unity.Entities;

public class SetupUnitMovementDefaultPositionAuthoring : MonoBehaviour
{


    public class Baker : Baker<SetupUnitMovementDefaultPositionAuthoring> {

        public override void Bake(SetupUnitMovementDefaultPositionAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new SetupUnitMovementDefaultPosition());
        }

    }
}

public struct SetupUnitMovementDefaultPosition : IComponentData{
}