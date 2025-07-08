using UnityEngine;
using Unity.Entities;


public class EntitiesReferencesAuthoring : MonoBehaviour {
    
    public GameObject projectilePrefabGameObject;

    public class Baker : Baker<EntitiesReferencesAuthoring> {

        public override void Bake(EntitiesReferencesAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EntitiesReferences{
                projectilePrefabEntity = GetEntity(authoring.projectilePrefabGameObject, TransformUsageFlags.Dynamic),
            });
        }

    }

}

public struct EntitiesReferences : IComponentData {

    public Entity projectilePrefabEntity;

}