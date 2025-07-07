using Unity.Entities;
using UnityEngine;

class SelectedAuthoring : MonoBehaviour {

    public GameObject visualIndicator;
    public float scale = 1;

    class Baker : Baker<SelectedAuthoring> {

        public override void Bake(SelectedAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Selected{
                visualIndicator = GetEntity(authoring.visualIndicator, TransformUsageFlags.Dynamic),
                scale = authoring.scale,
            });
            SetComponentEnabled<Selected>(entity, false);
        }

    }

}

public struct Selected : IComponentData, IEnableableComponent {
    
    public Entity visualIndicator;
    public float scale;

    public bool onSelected;
    public bool onDeselected;

};