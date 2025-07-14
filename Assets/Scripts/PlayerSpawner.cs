using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour,IDataPresistence
{
    public GameObject[] player;
    public int playerIndex;
    public GameObject PlayerLocation;
    void Start()
    {
        int selectedPlayer = PlayerPrefs.GetInt("SelectedID");
        playerIndex = selectedPlayer;
        player[selectedPlayer].SetActive(true);
        PlayerLocation = Instantiate(player[selectedPlayer], 
        transform.position, Quaternion.identity);
    }
    private void Update()
    {

        if (Input.GetKey(KeyCode.Backspace))
        {
            Cursor.lockState = CursorLockMode.Confined;
            DataPresistenceManager.Instance.SaveGame();
            SceneManager.LoadSceneAsync("Menu");
        }
    }
    public void LoadData(GameData gameData)
    {
        transform.position = gameData.playerPostion;
    }

    public void Savedata(GameData gameData)
    {
        gameData.playerPostion = transform.position;
    }
}
