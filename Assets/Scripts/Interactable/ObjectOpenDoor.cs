using UnityEngine;

public class ObjectOpenDoor : MonoBehaviour
{
    public bool isopen;
    public Animator door;
    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            isopen = true;
            door.SetBool("Open", true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            isopen = false;
            door.SetBool("Open", false);
        }
    }
}
