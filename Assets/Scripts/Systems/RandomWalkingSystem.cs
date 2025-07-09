using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct RandomWalkingSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state){

        RandomWalkingJob randomWalkingJob = new RandomWalkingJob();
        randomWalkingJob.ScheduleParallel();
        /*
        foreach((
            RefRW<RandomWalking> randomWalking,
            RefRW<UnitMovement> unitMovement,
            RefRO<LocalTransform> localTransform ) 
            in SystemAPI.Query<
            RefRW<RandomWalking>,
            RefRW<UnitMovement>,
            RefRO<LocalTransform>>()){
            
            float reachDistanceSq = UnitMovementSystem.REACHED_TARGET_POSITION_DISTANCE_SQ;
            if(math.distancesq(localTransform.ValueRO.Position, randomWalking.ValueRO.targetPosition) < reachDistanceSq){
                Random random = randomWalking.ValueRO.random;
                float3 randomDirection = new float3(random.NextFloat(-1f, 1f), 0f, random.NextFloat(-1f, 1f));
                randomDirection = math.normalize(randomDirection);
                randomWalking.ValueRW.targetPosition = 
                    randomWalking.ValueRO.originPosition + 
                    randomDirection * random.NextFloat(randomWalking.ValueRO.distanceMin, randomWalking.ValueRO.distanceMax);
                randomWalking.ValueRW.random = random;
            }
            else{
                unitMovement.ValueRW.targetPosition = randomWalking.ValueRO.targetPosition;
            }
        }
        */
    }
}

[BurstCompile]
public partial struct RandomWalkingJob : IJobEntity {
    public void Execute(ref RandomWalking randomWalking, ref UnitMovement unitMovement, in LocalTransform localTransform){
        float reachDistanceSq = UnitMovementSystem.REACHED_TARGET_POSITION_DISTANCE_SQ;
        if(math.distancesq(localTransform.Position, randomWalking.targetPosition) < reachDistanceSq){
            Random random = randomWalking.random;
            float3 randomDirection = new float3(random.NextFloat(-1f, 1f), 0f, random.NextFloat(-1f, 1f));
                randomDirection = math.normalize(randomDirection);
                randomWalking.targetPosition = 
                    randomWalking.originPosition + 
                    randomDirection * random.NextFloat(randomWalking.distanceMin, randomWalking.distanceMax);
                randomWalking.random = random;
        }
        else{
            unitMovement.targetPosition = randomWalking.targetPosition;
        }
    }
}
