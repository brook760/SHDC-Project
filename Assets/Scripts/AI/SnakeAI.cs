using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SnakeAI : MonoBehaviour
{
    [Range(0, 100)]
    public int health = 50;
    [HideInInspector]
    public NavMeshAgent agent;
    public Transform player;
    [HideInInspector]
    public Animator anim;

    public LayerMask whatIsGround, whatIsPlayer;

    //states
    [Header("Interation")]
    public float sightRange;
    public float attackRange;
    public bool playerInSightRange,playerInAttackRange;

    //patroling
    [Header("patrol point")]
    public Transform[] waypoints;
    private int waypointIndex;
    private Vector3 walkPoints;

    //attacking
    [Header("attack")]
    public float timeBetweenAttacks;
    public int damage;
    bool alreadyAttacked;
    SphereCollider _collider;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        _collider = GetComponent<SphereCollider>();
    }
    void Update()
    {
        if (health <= 0)
        {
            Die();
        }
        if (player == null)
            player = GameObject.FindWithTag("Player").transform;
        if (player.GetComponent<Character>().currentHealth == 0)
            return;
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (!playerInSightRange && !playerInAttackRange)
        {
            Patroling();
            anim.SetBool("Patrol", true);
            anim.SetBool("Chase", false);
        }
        if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
            anim.SetBool("Patrol", false);
            anim.SetBool("Chase", true);
        }
        if (playerInSightRange && playerInAttackRange)
        {
            AttackPlayer();
            anim.SetBool("Patrol", false);
            anim.SetBool("Chase", false);
        }
    }
    private void Patroling()
    {
        SearchWalkPoint();
        if(Vector3.Distance(transform.position,walkPoints) < 1)
        {

            waypointIndex++;
            if (waypointIndex == waypoints.Length)
            {
                waypointIndex = 0;
            }
        } 
    }
    private void SearchWalkPoint()
    {
        walkPoints = waypoints[waypointIndex].position;
        transform.LookAt(walkPoints);
        agent.SetDestination(walkPoints);
    }
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }
    private void AttackPlayer()
    {
        //make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);
        anim.SetBool("Attack", !alreadyAttacked);
        if (!alreadyAttacked)
        { 
            if(Physics.Raycast(transform.position,transform.forward,out RaycastHit hit, sightRange))
            {
                player.GetComponent<Character>().TakeDamage(damage);
            }
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        anim.SetTrigger("damage");
    }
    void Die()
    {
        anim.SetTrigger("death");
        enabled = false;
        _collider.enabled = false;
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
