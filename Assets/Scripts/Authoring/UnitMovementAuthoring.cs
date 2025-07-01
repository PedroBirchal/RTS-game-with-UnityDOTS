using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class UnitMovementAuthoring : MonoBehaviour{

    public float speed;


    public class Baker : Baker<UnitMovementAuthoring> {

        public override void Bake(UnitMovementAuthoring authoring){
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new UnitMovement{
                speed = authoring.speed,
            });
        }

    }

}

public struct UnitMovement : IComponentData{

    public float speed;
    public float3 targetPosition;

}
