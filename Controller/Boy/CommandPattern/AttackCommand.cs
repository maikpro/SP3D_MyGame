
using DefaultNamespace.Controller.Enemies;
using UnityEngine;

namespace Camera.Player.CommandPattern
{
    /**
     * AttackCommand wird aufgerufen im BoyController aufgerufen, sobald der Spieler mit die rechte Maustaste tätigt.
     * ODER wenn der Gegner angreift!
     * Hierbei wird ForEnemy als Flag verwendet, um zwischen Spielerangriff und Gegnerangriff zu unterscheiden!
     */
    public class AttackCommand : ICommand
    {
        private Animator animator;
        private Transform attackPoint;
        private float attackRange;
        private LayerMask attackLayers;
        private bool forEnemy; //Zur Wiederverwendung des Codes wird dieser Schalter auf true gesetzt wenn Enemy den AttackCommand nutzt!

        // ATTACK FOR PLAYER
        public AttackCommand(Animator animator, Transform attackPoint, float attackRange, LayerMask attackLayers)
        {
            this.animator = animator;
            this.attackPoint = attackPoint;
            this.attackRange = attackRange;
            this.attackLayers = attackLayers;
            this.forEnemy = false;
        }
        
        // Attack FOR ENEMY
        public AttackCommand(Transform attackPoint, float attackRange, LayerMask attackLayers, bool forEnemy)
        {
            this.attackPoint = attackPoint;
            this.attackRange = attackRange;
            this.attackLayers = attackLayers;
            this.forEnemy = forEnemy;
        }

        public void Execute()
        {
            if (this.forEnemy) // wenn das Flag gestetzt ist, dann greift der Gegner an!
            {
                EnemyAttacking(); 
            }
            else // sonst der Spieler
            {
                PlayerAttacking();
            }
        }

        /**
         * Quelle der Attacke für die Attacke:
         * Brackeys - MELEE COMBAT in Unity:
         * https://www.youtube.com/watch?v=sPiVz1k-fEs
         */
        private void PlayerAttacking()
        {

            this.animator.SetBool(GlobalNamingHandler.ATTACK_PARAMETER_NAME, true); // Attack-Animation
            
            // Attack, wenn die Sphere mit einem AttackLayer überschneidet, werden diese Objekte in das Array hitColliders gelegt.
            Collider[] hitColliders = Physics.OverlapSphere(this.attackPoint.position, this.attackRange, this.attackLayers);

            /**
             * Dann werden alle Objekte durchlaufen und es werden die Tags geprüft,
             * entweder der Spieler schlägt den Gegener oder der Spieler schlägt gegen eine Box
             * dementsprechend wird dann die passende Methode aufgerufen.
             */
            foreach (Collider collider in hitColliders)
            {
                Debug.Log("YOU ATTACKED " + collider.name);

                if (collider.CompareTag("Enemy"))
                {
                    Enemy enemy = collider.GetComponent<EnemyController>().Enemy;
                    enemy.TakesDamage(1);
                
                    Debug.Log(collider.name + " left " + enemy.Life.Counter);
                }

                else if (collider.CompareTag("DestructableBox"))
                {
                    DestructableObject destructableObject = collider.GetComponent<DestructableObject>();
                    destructableObject.OnAttackDestroy();
                }
            }
        }

        /**
         * Gleiches Prinzip wie bei PlayerAttacking nur dass hier der Gegner keine Boxen zerstören kann,
         * sondern nur den Spieler angreifen kann.
         */
        private void EnemyAttacking()
        {
            // Attack
            Collider[] hitColliders = Physics.OverlapSphere(this.attackPoint.position, this.attackRange, this.attackLayers);

            foreach (Collider collider in hitColliders)
            {
                Debug.Log("enemy ATTACKED " + collider.name);

                // Hitting Animation Player
                BoyController boyController = collider.GetComponent<BoyController>();
                boyController.IsHit = true;
            }
        }
    }
}