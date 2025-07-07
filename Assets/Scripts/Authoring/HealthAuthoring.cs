using UnityEngine;
using Unity.Entities;

public class HealthAuthoring : MonoBehaviour {

    public int maxHealth;

    public class Baker : Baker<HealthAuthoring> {

        public override void Bake(HealthAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Health{
                maxHealth = authoring.maxHealth,
                health = authoring.maxHealth,
            });
        }

    }

}

public struct Health : IComponentData {
    public int health;
    public int maxHealth;
}
