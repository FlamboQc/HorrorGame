using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using Vector3 = UnityEngine.Vector3;

public class Enemy : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    //Others var

    public static bool PlayerLightOn;
    public bool x2;//times 2
    public bool d2;//divide2




    //Animations

   
    bool isChasing;

    //patroling
    public Vector3 walkPoint;
   public bool walkpointset;
    public float walkPointRange;

    // Attacking

    public float timebetweenAttacks;
    bool alreadyAttacked;

    //states
    public float sightRange;
    public float attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Start ()
    {
        player = GameObject.Find("Fps Controller").transform;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        x2= true;
    }
    
    private void Update ()
    {
        //check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        //Set AI state
        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();

        if(PlayerLightOn && d2)
        {
            sightRange = sightRange * 2;
            x2 = true;
            d2= false;
            
        }

        if (!PlayerLightOn && x2)
        {
            sightRange = sightRange / 2;
            d2= true;
            x2= false;
            
        }


    }//Update

    private void Patroling()
    {
       

        if (walkpointset == false) SearchWalkPoint();

        if (walkpointset)
            agent.SetDestination(walkPoint);


        Vector3 distanceToWalkPoint = transform.position - walkPoint;
       
        if (distanceToWalkPoint.magnitude < 1f)
            walkpointset = false;
       
    }

    private void SearchWalkPoint()
    {
        float RandomZ = Random.Range(-walkPointRange, walkPointRange);
        float RandomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + RandomX, transform.position.y,  transform.position.z + RandomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkpointset = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        isChasing = true;
       
    } 

    private void AttackPlayer()
    {

        
;


        agent.SetDestination(transform.position);
        
        transform.LookAt(player);

        if (!alreadyAttacked)
        { 
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timebetweenAttacks);
        } 
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

 
  


}//class
