using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractUI : MonoBehaviour
{
    [SerializeField]
    private Transform cam;
    [SerializeField]
    private Transform UI;
    public GameObject Switch;
    public GameObject DoorOpen;
    public float time;
    public int radius = 30;
    public bool isPressed;

    private void LateUpdate()
    {
        UI.transform.LookAt(UI.transform.position + cam.forward);
    }
    private void OnTriggerStay(Collider other)
    {
        Vector3 currentRot = Switch.transform.localEulerAngles;
        if (Input.GetKey(KeyCode.E))
        {
            isPressed = !isPressed;
        }
        if (isPressed)
        {
            Switch.transform.localEulerAngles = Vector3.Lerp(currentRot, new Vector3
        (currentRot.x, currentRot.y, radius ), time * Time.deltaTime);

            DoorOpen.SetActive(false);
        }
    }
}
