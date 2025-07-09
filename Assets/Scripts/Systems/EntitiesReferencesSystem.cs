using Unity.Burst;
using Unity.Entities;
using UnityEngine;

[CreateBefore(typeof(ZombieSpawnerSystem))]
partial struct EntitiesReferencesSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state){
        foreach( var entitiesReferences in SystemAPI.Query<EntitiesReferences>()){
        }
    }
}
