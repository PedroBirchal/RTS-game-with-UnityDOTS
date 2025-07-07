using Unity.Burst;
using Unity.Entities;
using UnityEngine;

partial struct ProjectileAttackSystem : ISystem {

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        foreach ((
            RefRW<ProjectileAttack> projectileAttack,
            RefRO<Target> target
        )
        in
        SystemAPI.Query<
            RefRW<ProjectileAttack>,
            RefRO<Target>
        >()){
            if(target.ValueRO.targetEntity == Entity.Null){
                continue;
            }

            projectileAttack.ValueRW.timer -= SystemAPI.Time.DeltaTime;
            if(projectileAttack.ValueRO.timer > 0){
                continue;
            }
            projectileAttack.ValueRW.timer = projectileAttack.ValueRO.timerMax;

            RefRW<Health> targetHealth = SystemAPI.GetComponentRW<Health>(target.ValueRO.targetEntity);
            targetHealth.ValueRW.health -= projectileAttack.ValueRO.damage;
        }
    }

}
