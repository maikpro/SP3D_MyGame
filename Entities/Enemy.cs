using DefaultNamespace.Util;
using UnityEngine;
using UnityEngine.AI;

namespace DefaultNamespace.Controller.Enemies
{
    /**
     * Diese Klasse ist für die Verwaltung des Gegner-Zustandes.
     * Sie erbt von der EnemyAI, die für die Steuerung des Gegners mit NavAgent verantwortlich ist
     */
    
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

        /**
         * Um die nächste Position nicht mehr zu aktualisieren.
         */
        public void ClearNavMeshDestination()
        {
            this.navMeshAgent.updatePosition = false;
        }
        
        /**
         * Wenn der Gegner schaden nimmt, wird diese Methode aufgerufen.
         */
        public void TakesDamage(int damage)
        {
            this.life.Minus(damage);
        }
    }
}