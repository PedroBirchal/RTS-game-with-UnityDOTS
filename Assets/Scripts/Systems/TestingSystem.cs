using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

partial struct TestingSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        /*
        int numberOfEntities = 0;

        foreach((
            RefRW<LocalTransform> localTransform ,
            RefRO<UnitMovement> unitMovement, 
            RefRW<PhysicsVelocity> physicsVelocity,
            RefRO<Selected> selected) 
            in 
            SystemAPI.Query<
            RefRW<LocalTransform>,
            RefRO<UnitMovement>,
            RefRW<PhysicsVelocity>,
            RefRO<Selected>
            >()){
            
            numberOfEntities++;
        }
        Debug.Log(numberOfEntities);
        */
    }

}
