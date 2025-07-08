using UnityEngine;
using Unity.Entities;

public class ProjectileAttackAuthoring : MonoBehaviour
{

    public float timerMax;
    public int damage = 1;
    public float attackDistance;

    public class Baker : Baker<ProjectileAttackAuthoring> {

        public override void Bake(ProjectileAttackAuthoring authoring){
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new ProjectileAttack{
                timerMax = authoring.timerMax,
                damage = authoring.damage,
                attackDistance = authoring.attackDistance,
            });
        }

    }

}

public struct ProjectileAttack : IComponentData {
    public float timer;
    public float timerMax;
    public int damage;
    public float attackDistance;
}
