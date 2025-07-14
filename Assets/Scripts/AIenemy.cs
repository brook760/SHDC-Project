using UnityEngine;
using UnityEngine.AI;

public class AIenemy : MonoBehaviour
{
    public Animator anim;
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    //patroling
    public Vector3 walkPoints;
    bool walkPointSet;
    public float walkPointRange;

    //attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    //public GameObject gun;
    public float Damage = 10f;
    public float range = 100f;
    public int damage;

    //states
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Start()
    {

        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        if(player == null)
            player = FindObjectOfType<Character>().transform;
        //check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            Patroling();
            anim.SetBool("chase", false);
        }
        if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
            anim.SetBool("chase", true);
            anim.SetBool("attack", false);
        }
        if (playerInSightRange && playerInAttackRange)
        {
            AttackPlayer();
            anim.SetBool("attack", true);
        }

    }
    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoints);

        Vector3 distanceToWalkPoint = transform.position - walkPoints;
        // walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;

    }
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoints = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoints, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
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

        if (!alreadyAttacked)
        {
           // RaycastHit hit;
           // if (Physics.Raycast(gun.transform.position, gun.transform.forward, out hit, range))
           // {
           //     Debug.Log(hit.transform.name);
               // Health enemy = hit.transform.GetComponent<Health>();
               // if (enemy != null)
               // {
                    //enemy.TakeDamage(damage);
              //  }
           // }
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
