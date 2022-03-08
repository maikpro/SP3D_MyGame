using DefaultNamespace.Util;
using UnityEngine;
using UnityEngine.AI;

/*
 * QUELLE:
 * Dave / GameDevelopment - FULL 3D ENEMY AI in 6 MINUTES! || Unity Tutorial:
 * https://www.youtube.com/watch?v=UjkSFoLxesw
 */ 

public enum State { PATROLLING, CHASING, ATTACK, DEAD } // State-Machine 

/**
 * EnemyAI wird für die Steuerung des Gegners verwendet.
 * Je nach Verhalten werden unterschiedliche Methoden ausgeführt.
 */
public class EnemyAI
{
    protected NavMeshAgent navMeshAgent;
    private BoyController boyController;
    private Transform enemyTransform;

    // LayerMasks
    private LayerMask playerLayerMask;
    private LayerMask groundLayerMask;
    
    // Patroling
    private Vector3 walkPoint;
    private bool walkPointSet;
    private float walkPointRange;
    
    // States
    private float sightRange, attackRange;
    private bool playerInSightRange, playerInAttackRange;
    
    // CooldownTimer
    private Countdown cooldown;

    public EnemyAI(NavMeshAgent navMeshAgent, Transform enemyTransform, float sightRange, float attackRange, float walkPointRange)
    {
        this.navMeshAgent = navMeshAgent;
        this.enemyTransform = enemyTransform;
        this.sightRange = sightRange;
        this.attackRange = attackRange;
        this.boyController = GameObject.Find(GlobalNamingHandler.boyName).GetComponent<BoyController>();
        
        //Reichweite beim Patrolling
        this.walkPointRange = walkPointRange;

        this.playerLayerMask = LayerMask.GetMask(GlobalNamingHandler.LAYERMASK_PLAYER);
        this.groundLayerMask = LayerMask.GetMask(GlobalNamingHandler.LAYERMASK_GROUND);
    }

    public State Behaviour()
    {
        this.playerInSightRange = Physics.CheckSphere(this.enemyTransform.position, this.sightRange, this.playerLayerMask);
        this.playerInAttackRange = Physics.CheckSphere(this.enemyTransform.position, this.attackRange, this.playerLayerMask);
        
        // Wenn der Spieler nicht in Sichtweite oder in Attacknähe ist dann Patrolliere
        if (!playerInSightRange && !playerInAttackRange)
        {
            Patroling();
            return State.PATROLLING;
        }
        
        // Wenn der Spieler in Sichtweite ist, dann verfolge den Spieler
        if(playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
            return State.CHASING;
        }

        Debug.Log(!IsOnCooldown());
        // Wenn der Spieler in Attacknähe ist dann greife ihn an.
        if (playerInAttackRange && playerInSightRange && !IsOnCooldown())
        {
            AttackPlayer();
            StartCooldown(); // Damit der Gegner nicht ständig angreift wird ein Cooldown gestetzt.
            return State.ATTACK;
        }

        if (this.cooldown != null)
        {
            this.cooldown.Run();
            Debug.Log(this.cooldown.CurrentTime);
        }

        return State.CHASING;
    }

    /**
     * Der Gegner patrolliert wenn der Spieler nicht in der Sichtweite oder Angriffsnähe ist.
     */
    private void Patroling()
    {
        if(!this.walkPointSet) SearchWalkPoint(); // Wenn kein Zielpunkt gesetzt dann suche dir zufällig einen neuen.

        if (walkPointSet)
            this.navMeshAgent.SetDestination(this.walkPoint); // wenn der Zielpunkt gesetzt ist begibt sich der Gegner durch den NavMeshAgents dorthin.

        Vector3 distanceToWalkPoint = enemyTransform.position - walkPoint; // Die Distance zwischen Start und Zielpunkt wird berechnet

        //Reached Walkpoint
        if (distanceToWalkPoint.magnitude < 1f) // wenn der Zielpunkt erreicht ist, muss ein neuer Zielpunkt gesucht werden
            this.walkPointSet = false;
    }

    /**
     * Mit dieser Methode wird für den Gegner der nächste Zielpunkt gesucht.
     * Damit der Gegner mit dem NavMeshAgnets sich dorthin begeben kann.
     */
    private void SearchWalkPoint()
    {
        float randomX = Random.Range(-this.walkPointRange, this.walkPointRange);
        float randomZ = Random.Range(-this.walkPointRange, this.walkPointRange);
        this.walkPoint = new Vector3(enemyTransform.position.x + randomX, enemyTransform.position.y, enemyTransform.position.z + randomZ);

        if (Physics.Raycast(this.walkPoint, -enemyTransform.up, 2f, this.groundLayerMask)) this.walkPointSet = true;
    }

    /**
     * Wird aufgerufen wenn der Spieler sich in der Sichtnähe des Gegners befindet
     */
    private void ChasePlayer()
    {
        this.enemyTransform.LookAt(this.boyController.transform);
        this.navMeshAgent.SetDestination(this.boyController.gameObject.transform.position);
    }

    /**
     * Wird aufgerufen, wenn sich der Spieler in Angriffsnähe des Gegners befindet.
     */
    private void AttackPlayer()
    {
        this.enemyTransform.LookAt(this.boyController.transform);
        this.navMeshAgent.SetDestination(this.boyController.gameObject.transform.position);
    }

    /**
     * Starte den Cooldown, um keine dauerhaften Angriffe zu erlauben
     */
    private void StartCooldown()
    {
        this.cooldown = new Countdown(5f);
    }
    
    /**
     * Prüfung, ob der Gegner noch im Cooldownmodus ist.
     */
    private bool IsOnCooldown()
    {
        if (this.cooldown == null) return false;
        return this.cooldown.CurrentTime > 0;
    }
}
