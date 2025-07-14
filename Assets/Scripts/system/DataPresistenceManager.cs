using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class DataPresistenceManager : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private bool initializeDataIfNull = false;

    [Header("file Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;

    private GameData gamedata;
    private List<IDataPresistence> DataPresistenceObjects;
    public FileDataHandler dataHandler;

    public string SelectedProfileID = "test";
    public static DataPresistenceManager Instance
    {
        get; private set;
    }

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.Log("Found more than one Data Presistence manager in a scene");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        this.dataHandler = new(Application.persistentDataPath, fileName, useEncryption);
        this.SelectedProfileID = dataHandler.GetMostRecentlyUpdatedProfileID();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        DataPresistenceObjects = FindAllDataPresistencesObjects();
        LoadGame();
    }
    public void ChangeSelectedProfileID(string newProfileID)
    {
        SelectedProfileID = newProfileID;
        LoadGame();
    }
    public void NewGame()
    {
        this.gamedata = new GameData();
    }
    public void LoadGame()
    {
        this.gamedata = dataHandler.Load(SelectedProfileID);

        //if no data can be loaded, initialize to a new game
        if(this.gamedata == null && initializeDataIfNull)
        {
            NewGame();
        }
        if (this.gamedata == null) 
        {
            Debug.Log("No Data was found. Initializing data to defaults.");
            return;
        }

        //push the loaded data to all other scripts that need it
        foreach(IDataPresistence dataPresistenceObj in DataPresistenceObjects) 
        {
            dataPresistenceObj.LoadData(this.gamedata);
        }
        Debug.Log("Loading game" + SceneManager.GetActiveScene().name);
    }
    public void SaveGame()
    {
        if(this.gamedata == null) 
        {
            Debug.LogWarning("No data was found. a new Game needs to be started");
            return;
        }
        //pass the data to other scripts so they can update it
        foreach (IDataPresistence dataPresistenceObj in DataPresistenceObjects)
        {
            dataPresistenceObj.Savedata(gamedata);
        }
        //Timestamp the data
        gamedata.lastUpdated = System.DateTime.Now.ToBinary();

        // save that data to a file using the data handler
        dataHandler.Save(gamedata,SelectedProfileID);

        Debug.Log("Saving Game:"+ SceneManager.GetActiveScene().name);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<IDataPresistence> FindAllDataPresistencesObjects()
    {
        IEnumerable<IDataPresistence> dataPresistencesObjects = 
            FindObjectsOfType<MonoBehaviour>(true).OfType<IDataPresistence>();
        return new List<IDataPresistence>(dataPresistencesObjects);
    }
    public bool NoDataFound()
    {
        return gamedata == null;
    }

    public Dictionary<string, GameData> GetAllProfileGameData()
    {
        return dataHandler.LoadAllProfiles();
    }
}
