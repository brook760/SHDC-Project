using UnityEngine;
using TMPro;

public class SpawnPoint : MonoBehaviour
{
    public PlayerSpawner spawner;
    public AudioSource Achivement;
    public GameObject text; 
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            text.GetComponent<TextMeshProUGUI>().text = this.name;
            text.SetActive(true);

            Achivement.PlayOneShot(Achivement.clip);
            spawner.transform.position = transform.position;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        text.SetActive(false);
    }
}
