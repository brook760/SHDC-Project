using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameUI : MonoBehaviour
{
    public GameObject player;
    public int PlayerHealth;
    public GameObject DefeatMenu;
    public GameObject WinnerMenu;
    public PlayerSpawner Spawner;
    public CurrencyUI Currency;
    public GameObject[] Stars;
    public AudioSource source;

    public AiEnemy BossEnemy;
    bool death = false;
    bool Winner;
    private void Start()
    {
       DefeatMenu.SetActive(false);
        WinnerMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Winner = true;
    }
    private void Update()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
        PlayerHealth = player.GetComponent<Character>().currentHealth;
        if (PlayerHealth == 0)
        {
            DefeatMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
            if (!death)
            {
                Currency.PlayerDeath();
                death = true;
            }
        }
        for(int i =0; i< Currency.DeathPercentage(); i++)
        {
            Stars[i].SetActive(true);
        }
        if (Currency.Winner && BossEnemy.health <=0 && Winner)
        {
            source.PlayOneShot(source.clip, 1);
            Cursor.lockState = CursorLockMode.Confined;
            WinnerMenu.SetActive(true);
            Winner = false;
        }
    } 
    public void MainMenu()
    {
        death = false;
        DataPresistenceManager.Instance.SaveGame();
        SceneManager.LoadSceneAsync("Menu");
    }
    public void Replay()
    {
        Cursor.lockState = CursorLockMode.Locked;
        player.transform.position = Spawner.transform.position;
        player.GetComponent<Character>().enabled = true;
        player.GetComponent<Character>().playerInput.enabled = true;
        player.GetComponent<Character>().controller.enabled = true;
        player.GetComponent<Character>().currentHealth = 100;
        player.GetComponent<Character>().animator.SetTrigger("Respawn");
        death = false;
    }
    public void Quit()
    {
        death = false;
        DataPresistenceManager.Instance.SaveGame();
        Application.Quit();

    }
    public void Continue()
    {
        WinnerMenu.SetActive(false);
    }

}
