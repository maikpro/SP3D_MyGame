
using DefaultNamespace.Controller.Enemies;
using UnityEngine;

namespace Camera.Player.CommandPattern
{
    public class AttackCommand : ICommand
    {
        public const string attackParameterName = "isAttacking";
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
            if (this.forEnemy)
            {
                EnemyAttacking(); 
            }
            else
            {
                PlayerAttacking();
            }
        }

        //Quelle der Attacke: https://www.youtube.com/watch?v=sPiVz1k-fEs
        private void PlayerAttacking()
        {

            this.animator.SetBool(attackParameterName, true);
            // Attack
            Collider[] hitColliders = Physics.OverlapSphere(this.attackPoint.position, this.attackRange, this.attackLayers);

            foreach (Collider collider in hitColliders)
            {
                Debug.Log("YOU ATTACKED " + collider.name);

                if (collider.CompareTag("Enemy"))
                {
                    Enemy enemy = collider.GetComponent<EnemyController>().Enemy;
                    enemy.TakesDamage(1);
                
                    Debug.Log(collider.name + " left " + enemy.Life.Counter);
                }
                
                /*else if (collider.CompareTag("Player"))
                {
                    var player = collider.GetComponent<BoyController>().Player;
                    player.TakesDamage(1);
                }*/
                
                else if (collider.CompareTag("DestructableBox"))
                {
                    DestructableObject destructableObject = collider.GetComponent<DestructableObject>();
                    destructableObject.OnAttackDestroy();
                }
            }
        }

        private void EnemyAttacking()
        {
            // Attack
            Collider[] hitColliders = Physics.OverlapSphere(this.attackPoint.position, this.attackRange, this.attackLayers);

            foreach (Collider collider in hitColliders)
            {
                Debug.Log("enemy ATTACKED " + collider.name);
                
                // Damage Player
                /*GameLogic gameLogic = GameObject.Find("GameLogic").GetComponent<GameLogic>();
                gameLogic.MinusLife();*/
                
                // Hitting Animation Player
                BoyController boyController = collider.GetComponent<BoyController>();
                boyController.IsHit = true;
            }
        }
    }
}