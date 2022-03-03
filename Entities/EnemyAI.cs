using DefaultNamespace.Util;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.AI;

//QUELLE: https://www.youtube.com/watch?v=UjkSFoLxesw

public enum State { PATROLLING, CHASING, ATTACK, DEAD }

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
        this.boyController = GameObject.Find("Boy").GetComponent<BoyController>();
        
        //Reichweite beim Patrolling
        this.walkPointRange = walkPointRange;

        this.playerLayerMask = LayerMask.GetMask("Player");
        this.groundLayerMask = LayerMask.GetMask("Ground");
    }

    public State Behaviour()
    {
        this.playerInSightRange = Physics.CheckSphere(this.enemyTransform.position, this.sightRange, this.playerLayerMask);
        this.playerInAttackRange = Physics.CheckSphere(this.enemyTransform.position, this.attackRange, this.playerLayerMask);
        
        if (!playerInSightRange && !playerInAttackRange)
        {
            Patroling();
            return State.PATROLLING;
        }
        
        if(playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
            return State.CHASING;
        }

        Debug.Log(!IsOnCooldown());
        if (playerInAttackRange && playerInSightRange && !IsOnCooldown())
        {
            AttackPlayer();
            StartCooldown();
            return State.ATTACK;
        }

        if (this.cooldown != null)
        {
            this.cooldown.Run();
            Debug.Log(this.cooldown.CurrentTime);
        }

        return State.CHASING;
    }

    private void Patroling()
    {
        if(!this.walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            this.navMeshAgent.SetDestination(this.walkPoint);

        Vector3 distanceToWalkPoint = enemyTransform.position - walkPoint;

        //Reached Walkpoint
        if (distanceToWalkPoint.magnitude < 1f)
            this.walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomX = Random.Range(-this.walkPointRange, this.walkPointRange);
        float randomZ = Random.Range(-this.walkPointRange, this.walkPointRange);
        this.walkPoint = new Vector3(enemyTransform.position.x + randomX, enemyTransform.position.y, enemyTransform.position.z + randomZ);

        if (Physics.Raycast(this.walkPoint, -enemyTransform.up, 2f, this.groundLayerMask)) this.walkPointSet = true;
    }

    private void ChasePlayer()
    {
        this.enemyTransform.LookAt(this.boyController.transform);
        this.navMeshAgent.SetDestination(this.boyController.gameObject.transform.position);
    }

    private void AttackPlayer()
    {
        this.enemyTransform.LookAt(this.boyController.transform);
        this.navMeshAgent.SetDestination(this.boyController.gameObject.transform.position);
    }

    private void StartCooldown()
    {
        this.cooldown = new Countdown(5f);
    }
    
    private bool IsOnCooldown()
    {
        if (this.cooldown == null) return false;
        return this.cooldown.CurrentTime > 0;
    }
}
