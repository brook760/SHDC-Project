using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorObject : MonoBehaviour
{
    public AudioSource DoorClose;
    public float closeDelay = 0.8f;
    public AudioSource DoorOpen;
    public float openDelay = 0;
    public LockedObject m_LockedObject;
    public KeyUIControl UIControl;

    private void Start()
    {
        m_LockedObject = GetComponent<LockedObject>();
        UIControl = GameObject.Find("UI").GetComponentInChildren<KeyUIControl>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            UIControl.UIOn = true;
        }
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
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            UIControl.UIOn = false;
        }
    }
}
