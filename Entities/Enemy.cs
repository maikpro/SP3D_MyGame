using DefaultNamespace.Util;
using UnityEngine;
using UnityEngine.AI;

namespace DefaultNamespace.Controller.Enemies
{
    public class Enemy : EnemyAI
    {
        private Life life;
        private bool isAttacking;
        private Countdown cooldownTimer;

        public Enemy(Life life, NavMeshAgent navMeshAgent, Transform enemyTransform, float sightRange, float attackRange, float walkPointRange) : base(navMeshAgent, enemyTransform,  sightRange, attackRange,  walkPointRange)
        {
            this.life = life;
            this.isAttacking = false;
            this.cooldownTimer = new Countdown(0f);
        }

        public Life Life
        {
            get => life;
        }

        public bool IsAttacking
        {
            get => isAttacking;
            set => isAttacking = value;
        }

        public void ClearNavMeshDestination()
        {
            this.navMeshAgent.updatePosition = false;
        }
        
        public void TakesDamage(int damage)
        {
            this.life.Minus(damage);
        }
    }
}