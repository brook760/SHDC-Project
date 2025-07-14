using UnityEngine;
using UnityEngine.AI;

public class AiEnemy : MonoBehaviour
{
    public AIConfig config;
    public bool debug;
    public int health;
    [HideInInspector] public GameObject player;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator animator;
    CapsuleCollider capsuleCollider;
    public AudioSource source;

    [HideInInspector] public bool AgentOn = true;
    [HideInInspector] public float timePassed;
    float newDestinatinCD = 0.5f;
    void Start()
    {
        health = config.health;
        player = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        enabled = true;
    }
    private void Update()
    {
        if(health <= 0)
        {
            Die();
        }

        animator.SetFloat("speed", agent.velocity.magnitude / agent.speed);
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if (player.GetComponent<Character>().currentHealth == 0)
            return;

        if (AgentOn)
        {
            if (timePassed >= config.AttackCD)
            {
                if (Vector3.Distance(player.transform.position, transform.position) <= config.AttackRange)
                {
                    animator.SetTrigger("attack");
                    timePassed = 0;
                }
            }
            timePassed += Time.deltaTime;

            if (newDestinatinCD <= 0 && Vector3.Distance(player.transform.position, transform.position) <= config.LookRange)
            {
                newDestinatinCD = 0.5f;
                agent.SetDestination(player.transform.position);
                transform.LookAt(player.transform);
            }
            newDestinatinCD -= Time.deltaTime;
        }   
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        animator.SetTrigger("damage");
    }
    void Die()
    {
        animator.SetTrigger("death");
        enabled = false;
        capsuleCollider.enabled = false;
        GetComponentInChildren<BoxCollider>().enabled = false;
    }
    public void StartDealDamage()
    {
        source.PlayOneShot(source.clip);
        GetComponentInChildren<EnemyHitSystem>().StartDealDamage();
    }
    public void EndDealDamage()
    {
        GetComponentInChildren<EnemyHitSystem>().EndDealDamage();
    }
    private void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, config.AttackRange);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, config.LookRange);
        }
    }
}
