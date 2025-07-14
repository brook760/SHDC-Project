using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatMenu : MonoBehaviour
{
    public GameObject player;
    public int healthOfPlayer;
    void Update()
    {
        if(player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
        healthOfPlayer =player.GetComponent<Character>().currentHealth;
        if(healthOfPlayer <= 0)
        {
            gameObject.SetActive(true);
        }
    }
    public void Replay()
    {
        DataPresistenceManager.Instance.LoadGame();
        SceneManager.LoadSceneAsync("MainGame");
    }
    public void Menu()
    {
            DataPresistenceManager.Instance.SaveGame();
            SceneManager.LoadSceneAsync("Menu");
    }
}
