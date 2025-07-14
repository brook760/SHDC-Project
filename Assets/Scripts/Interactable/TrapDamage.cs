using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDamage : MonoBehaviour
{
    public int damage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out Character player))
        {
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }

    }
}
