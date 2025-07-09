using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class ProjectileAttackAuthoring : MonoBehaviour
{

    public float timerMax;
    public int damage = 1;
    public float attackDistance;
    public Transform projectileSpawnPositionTransform;
    public GameObject EntitiesReferencesGameObject;


    public class Baker : Baker<ProjectileAttackAuthoring> {

        public override void Bake(ProjectileAttackAuthoring authoring){
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new ProjectileAttack{
                timerMax = authoring.timerMax,
                damage = authoring.damage,
                attackDistance = authoring.attackDistance,
                projectileSpawnLocalPosition = authoring.projectileSpawnPositionTransform.localPosition,
                entitiesReferencesEntity = GetEntity(authoring.EntitiesReferencesGameObject,TransformUsageFlags.Dynamic),
            });
        }

    }

}

public struct ProjectileAttack : IComponentData {
    public float timer;
    public float timerMax;
    public int damage;
    public float attackDistance;
    public float3 projectileSpawnLocalPosition;
    public Entity entitiesReferencesEntity;
}
