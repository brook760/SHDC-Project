using UnityEngine;

public class AutoOpen : MonoBehaviour
{
    public bool isopen;
    public GameObject Text;

    public Animator anim;
    private void Start()
    {
        isopen = false;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                if (!isopen)
                {
                    anim.SetBool("Open", true);
                    isopen = true;
                    Text.SetActive(false);
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Text.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            anim.SetBool("Open", false);
            isopen = false;
        }
    }
}
