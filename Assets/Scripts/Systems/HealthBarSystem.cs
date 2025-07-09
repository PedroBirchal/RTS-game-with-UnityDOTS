using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

partial struct HealthBarSystem : ISystem {

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        /*
        Vector3 cameraForward = Vector3.zero;
        if(Camera.main != null ){
            cameraForward = Camera.main.transform.forward;
        }
        */

        foreach((
                RefRO<HealthBar> healthBar,
                RefRW<LocalTransform> localTransform
            ) in SystemAPI.Query<
                RefRO<HealthBar>,
                RefRW<LocalTransform>
            >()){
            
            Health health = SystemAPI.GetComponent<Health>(healthBar.ValueRO.healthEntity);
            float healthNormalized = (float)health.health / health.maxHealth;

            RefRW<PostTransformMatrix> barVisualPostTransformMatrix =  SystemAPI.GetComponentRW<PostTransformMatrix>(healthBar.ValueRO.barVisualEntity);
            barVisualPostTransformMatrix.ValueRW.Value = float4x4.Scale(healthNormalized, 1, 1);
        }
        
    }

}
