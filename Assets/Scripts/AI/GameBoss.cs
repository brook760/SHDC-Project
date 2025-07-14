using UnityEngine;

public class GameBoss : MonoBehaviour
{
    AiEnemy enemy;
    public Healthbar healthbar;
    public GameObject Chair;
    [SerializeField] GameObject weaponHolder;
    [SerializeField] GameObject weapon;
    [SerializeField] GameObject weaponSheath;
    public GameObject BossUI;
    public AudioSource source;
    public GameObject AIEnemy;

    GameObject currentWeaponInHand;
    GameObject currentWeaponInSheath;
    public float LookRange = 10f;
    public float stepTimer;
    public float SpawnTimer;
    public LayerMask Player;

    void Start()
    {
        enemy = GetComponent<AiEnemy>();
        healthbar.SetMaxHealth(0, enemy.config.health);
        currentWeaponInSheath = Instantiate(weapon, weaponSheath.transform);
        BossUI.SetActive(false);
    }

    private void Update()
    {
        bool Range = Physics.CheckSphere(transform.position, LookRange, Player);
        healthbar.SetHealth(enemy.health);
        if (Range && enemy.health >0)
        {
            enemy.AgentOn = true;
            BossUI.SetActive(true);
            //enemy.transform.LookAt(transform.position);
            enemy.animator.SetBool("Sitting", false);
            enemy.animator.SetBool("SightRange", true);
            source.Play();
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {

                source.PlayOneShot(source.clip, 1);
                stepTimer = 170;
            }
            Vector3 offset = new(-1, 0, 1);
            SpawnTimer -= Time.deltaTime;
            if (SpawnTimer <= 0f)
            {
                Instantiate(AIEnemy, transform.localPosition + offset, transform.rotation);
                SpawnTimer = 20;
            }
        }
        else if(!Range && enemy.health >0)
        {
            BossUI.SetActive(false);
            enemy.animator.SetBool("SightRange", false);
            source.Pause();
            ReturnToSit();
        }
        else
        {
            source.Pause();
        }


    }
    private void ReturnToSit()
    {
        enemy.AgentOn = false;
        enemy.agent.SetDestination(Chair.transform.position);
        if (enemy.agent.remainingDistance <= enemy.config.AttackRange)
        {
            transform.rotation = Chair.transform.rotation;
            transform.SetPositionAndRotation(Chair.transform.position, Chair.transform.rotation);
            enemy.animator.SetBool("Sitting", true);
        }
    }
    private void OnDrawGizmos()
    {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, LookRange);
    }
    public void DrawWeapon()
    {
        currentWeaponInHand = Instantiate(weapon, weaponHolder.transform);
        Destroy(currentWeaponInSheath);
        int count = weaponSheath.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            Destroy(weaponSheath.transform.GetChild(i).gameObject);
        }
    }

    public void SheathWeapon()
    {
        currentWeaponInSheath = Instantiate(weapon, weaponSheath.transform);
        Destroy(currentWeaponInHand);
        int count = weaponHolder.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            Destroy(weaponHolder.transform.GetChild(i).gameObject);
        }
    }    
}
