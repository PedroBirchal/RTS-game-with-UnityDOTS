using UnityEngine;
using Unity.Entities;

public class MeeleeAttackAuthoring : MonoBehaviour{

    public float timerMax;

    public class Baker : Baker<MeeleeAttackAuthoring> {

        public override void Bake(MeeleeAttackAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new MeeleeAttack{
                timerMax = authoring.timerMax,
            });
        }

    }

}

public struct MeeleeAttack : IComponentData{
    public float timer;
    public float timerMax;
}
