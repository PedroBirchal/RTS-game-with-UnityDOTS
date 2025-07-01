using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Physics;

partial struct UnitMovementSystem : ISystem {

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        UnitMovementJob unitMovementJob = new UnitMovementJob();
        unitMovementJob.ScheduleParallel(); 

        /*
        foreach( (
            RefRW<LocalTransform> localTransform,
            RefRO<UnitMovement> unitMovement,
            RefRW<PhysicsVelocity> physicsVelocity)

            in SystemAPI.Query<
                RefRW<LocalTransform>,
                RefRO<UnitMovement>,
                RefRW<PhysicsVelocity>>()) {

            float3 moveDirection = unitMovement.ValueRO.targetPosition - localTransform.ValueRO.Position;
            moveDirection = math.normalize(moveDirection);

            physicsVelocity.ValueRW.Linear = moveDirection * unitMovement.ValueRO.speed;
            physicsVelocity.ValueRW.Angular = float3.zero;
        }
        */
    }

}

[BurstCompile]
public partial struct UnitMovementJob : IJobEntity {
    
    public void Execute(ref LocalTransform localTransform, in UnitMovement unitMovement, ref PhysicsVelocity physicsVelocity){
        float3 moveDirection = unitMovement.targetPosition - localTransform.Position;

        float targetReachToleranceSq = 0.05f;
        if(math.lengthsq(moveDirection) < targetReachToleranceSq){
            physicsVelocity.Linear = float3.zero;
            physicsVelocity.Angular = float3.zero;
            return;
        }

        moveDirection = math.normalize(moveDirection);

        physicsVelocity.Linear = moveDirection * unitMovement.speed;
        physicsVelocity.Angular = float3.zero;
    }

};