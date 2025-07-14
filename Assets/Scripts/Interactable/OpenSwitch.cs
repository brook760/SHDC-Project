using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSwitch : MonoBehaviour
{
    [SerializeField]
    private Transform cam;
    [SerializeField]
    private Transform UI;
    public Animator door;
    private void LateUpdate()
    {
        UI.transform.LookAt(UI.transform.position + cam.forward);
    }
    private void OnTriggerStay(Collider other)
    {
        
        if (Input.GetKey(KeyCode.E))
        {
            door.SetBool("Open", true);
        }
    }
}
