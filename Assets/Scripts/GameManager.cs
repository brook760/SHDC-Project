using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject[] player;
    public int playerIndex;
    public bool inGamePlayScene = false;
    public GameObject Spawn;
    public GameObject PlayerLocation;
    void Start()
    {
        if (DataPresistenceManager.Instance.NoDataFound())
        {
            Debug.Log("NoSaveData");
        }
        int selectedPlayer = PlayerPrefs.GetInt("SelectedID");
        if (inGamePlayScene)
        {
            playerIndex = selectedPlayer;
            player[selectedPlayer].SetActive(true);
            PlayerLocation = 
                Instantiate(player[selectedPlayer],Spawn.transform.position, Quaternion.identity);
        }
    }
    public void SpawnOnPrevious()
    {
        int selectedPlayer = PlayerPrefs.GetInt("SelectedID");
            playerIndex = selectedPlayer;
            player[selectedPlayer].SetActive(true);
            PlayerLocation =
                Instantiate(player[selectedPlayer], Spawn.transform.position, Quaternion.identity);
    }
}
