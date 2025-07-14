using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LockedObject : MonoBehaviour
{
    public GameObject keyInventory;

    public bool isReach,hasKey,isopen;
    public GameObject LockText,Text;

    private Animator anim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        isReach = false;
        hasKey = false;
        isopen = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isReach = true;
            Text.SetActive(true);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                if (hasKey)
                {
                    anim.SetBool("Open", true);
                    isopen = true;
                    Text.SetActive(false);
                }
                else
                {
                    LockText.GetComponent<TextMeshProUGUI>().text = "Find key: " +keyInventory.name;
                    LockText.SetActive(true);
                    Text.SetActive(false);
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isReach = false;
            anim.SetBool("Open", false);
            isopen = false;
            LockText.SetActive(false);
            Text.SetActive(false);
        }
    }
    private void Update()
    {
        if (keyInventory.activeInHierarchy)
            hasKey = true;
        else
            hasKey = false;
    }
}
