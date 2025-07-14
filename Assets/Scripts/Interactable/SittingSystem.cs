using UnityEngine;
using UnityEngine.InputSystem;

public class SittingSystem : MonoBehaviour
{
    public GameObject player,exit, inText, standText;
    public bool isStanding = false,interatacble;
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            inText.SetActive(true);
            interatacble = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inText.SetActive(false);
            interatacble = false;
        }
    }
    void Update()
    {

        if (interatacble == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                inText.SetActive(false);
                standText.SetActive(true);
                player.transform.parent = transform;
                player.GetComponent<CharacterController>().enabled = false;
                player.GetComponent<PlayerInput>().enabled = false;
                player.GetComponent<Animator>().SetBool("Driving", true);
                player.GetComponent<Animator>().SetTrigger("Car");
                player.GetComponent<Character>().enabled = false;
                player.transform.SetPositionAndRotation(transform.position, transform.rotation);
                isStanding = true;
                interatacble =false;
            }
        }
        if(isStanding == true)
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                standText.SetActive(false);
                isStanding = false;
                player.transform.position = exit.transform.position;
                player.transform.parent = null;
                player.GetComponent<CharacterController>().enabled = true;
                player.GetComponent<PlayerInput>().enabled = true;
                player.GetComponent<Animator>().SetBool("Driving",false);
                player.GetComponent<Character>().enabled = true;
            }
        }
    }
}
