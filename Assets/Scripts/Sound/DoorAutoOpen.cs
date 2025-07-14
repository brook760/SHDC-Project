using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAutoOpen : MonoBehaviour
{
    public AudioSource DoorClose;
    public float closeDelay = 0.8f;
    public AudioSource DoorOpen;
    public float openDelay = 0;
    public AutoOpen m_LockedObject;

    private void Start()
    {
        m_LockedObject = GetComponent<AutoOpen>();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if (m_LockedObject.isopen)
            {
                DoorOpen.PlayDelayed(openDelay);
            }
            else if (!m_LockedObject.isopen)
            {
                DoorClose.PlayDelayed(closeDelay);
            }
        }
    }
}
