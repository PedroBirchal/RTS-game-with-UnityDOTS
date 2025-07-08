using UnityEngine;
using Unity.Entities;

public class ProjectileAuthoring : MonoBehaviour
{

    public float speed = 5.0f;
    public int damage;

    public class Baker : Baker<ProjectileAuthoring> {

        public override void Bake(ProjectileAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Projectile{
                speed = authoring.speed,
                damage = authoring.damage,
            });
        }

    }

}

public struct Projectile : IComponentData {
    public float speed;
    public int damage;
}
