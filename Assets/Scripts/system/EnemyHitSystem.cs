using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitSystem : MonoBehaviour
{
    [HideInInspector]
    public bool canDealDamage;
    [SerializeField] List<GameObject> hasDealtDamage;

    [SerializeField] int weaponDamage;
    void Start()
    {
        canDealDamage = false;
        hasDealtDamage = new List<GameObject>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (canDealDamage)
        {
            if (other.transform.TryGetComponent(out Character health))
            {
                health.TakeDamage(weaponDamage);
                hasDealtDamage.Add(other.transform.gameObject);
            }
        }
    }
    public void StartDealDamage()
    {
        canDealDamage = true;
        hasDealtDamage.Clear();
    }
    public void EndDealDamage()
    {
        canDealDamage = false;
    }
}
