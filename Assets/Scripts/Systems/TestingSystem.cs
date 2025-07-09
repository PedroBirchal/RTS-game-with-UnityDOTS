using Unity.Burst;
using Unity.Entities;
using UnityEngine;

partial struct TestingSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        int numberOfAllies = 0;

        foreach( var ally in SystemAPI.Query<RefRO<Unit>>().WithPresent<Selected>()){
            numberOfAllies ++;
        }
    }

}
