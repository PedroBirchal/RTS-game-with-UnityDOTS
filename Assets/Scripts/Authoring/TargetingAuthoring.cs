using UnityEngine;
using Unity.Entities;

public class TargetingAuthoring : MonoBehaviour{

    public float range = 5.0f;
    public Faction targetFaction;
    public float timerMax = 0.5f;

    public class Baker : Baker<TargetingAuthoring> {

        public override void Bake(TargetingAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Targeting{
                range = authoring.range,
                targetFaction = authoring.targetFaction,
                timerMax = authoring.timerMax,
            });
        }

    }

}

public struct Targeting : IComponentData {

    public float range;
    public Faction targetFaction;
    public float timer;
    public float timerMax;
}
