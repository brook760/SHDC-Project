using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantDamage : MonoBehaviour
{
    [SerializeField] float StepTimer;
    public int Damage;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StepTimer -= Time.deltaTime;
            if(StepTimer <= 0)
            {
                other.GetComponent<Character>().TakeDamage(Damage);
                StepTimer = 2.5f;
            }
        }
    }
}
