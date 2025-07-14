using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public string sceneToLoad;
    public string respawnName;
    public Transform desiredLoacation;

    private void OnTriggerEnter(Collider other)
    {
        other.transform.position = desiredLoacation.position;
        SceneManager.LoadScene(sceneToLoad);
        PlayerPrefs.SetString("RespawnPoint", respawnName);
    }
    private void Start()
    {
        if (PlayerPrefs.HasKey("RespawnPoint"))
        {
            string storedRespawn = PlayerPrefs.GetString("RespawnPoint");
            GameObject respawnPoint = GameObject.Find(storedRespawn);
            if (respawnPoint != null)
            {
                transform.position = respawnPoint.transform.position;
            }
            PlayerPrefs.DeleteKey("RespawnPoint");
        }
    }
}
