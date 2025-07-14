using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.TryGetComponent<Character>(out var player))
        {
            if (player != null) 
            {
                player.regenerateHealth = true;
            }
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.TryGetComponent<Character>(out var player))
        {
            if (player != null)
            {
                player.regenerateHealth = false;
            }
        }
    }
}
