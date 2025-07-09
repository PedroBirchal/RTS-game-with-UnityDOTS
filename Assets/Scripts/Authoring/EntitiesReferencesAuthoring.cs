using UnityEngine;
using Unity.Entities;


public class EntitiesReferencesAuthoring : MonoBehaviour {
    
    public GameObject projectilePrefabGameObject;
    public GameObject zombiePrefabGameObject;

    public class Baker : Baker<EntitiesReferencesAuthoring> {

        public override void Bake(EntitiesReferencesAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new EntitiesReferences{
                projectilePrefabEntity = GetEntity(authoring.projectilePrefabGameObject, TransformUsageFlags.Dynamic),
                zombiePrefabEntity = GetEntity(authoring.zombiePrefabGameObject, TransformUsageFlags.Dynamic),
            });
        }

    }

}

public struct EntitiesReferences : IComponentData {

    public Entity projectilePrefabEntity;
    public Entity zombiePrefabEntity;

}