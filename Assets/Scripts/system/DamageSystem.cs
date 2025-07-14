using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSystem : MonoBehaviour
{
    bool canDealDamage;
    [SerializeField] List<GameObject> hasDealtDamage;

    //[SerializeField] float weaponLength;
    [SerializeField] int weaponDamage;
    void Start()
    {
        canDealDamage = false;
        hasDealtDamage = new List<GameObject>();
    }
   /* void Update()
    {
        if (canDealDamage)
        {
            RaycastHit hit;
            int layerMask = 1 << LayerMask.NameToLayer("Player");
            if(Physics.Raycast(transform.position,-transform.up,out hit, weaponLength, layerMask))
            {
               if(hit.transform.TryGetComponent(out AiEnemy enemy))
                {
                    enemy.TakeDamage(weaponDamage);
                    hasDealtDamage.Add(hit.transform.gameObject);
                }

            }
        }
    }*/
    public void OnTriggerEnter(Collider other)
    {
        if (canDealDamage)
        {
            if(other.transform.TryGetComponent(out AiEnemy enemy))
            {
                enemy.TakeDamage(weaponDamage);
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
