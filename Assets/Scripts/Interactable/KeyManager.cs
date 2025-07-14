using TMPro;
using UnityEngine;

public class KeyManager : MonoBehaviour,IDataPresistence
{
    [SerializeField] private string id;


    public GameObject keyObject;
    public GameObject keyInventory;
    public GameObject keyText;

    bool collected = false;
    [HideInInspector] public bool isReach;
    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }
    private void Start()
    {
        isReach = false;
        keyInventory.SetActive(false);    
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isReach = true;
            keyText.SetActive(true);
            keyText.GetComponent<TextMeshProUGUI>().text = keyInventory.name;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (!collected)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (Input.GetKey(KeyCode.E))
                {
                    collected = true;
                    keyObject.SetActive(false);
                    keyInventory.SetActive(true);
                    keyText.SetActive(false);
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isReach = false;
            keyText.SetActive(false);
        }
    }

    public void LoadData(GameData gameData)
    {
        gameData.keys.TryGetValue(id, out collected);
        if (collected)
        {
            gameObject.SetActive(false);
            keyInventory.SetActive(true);
        }
    }

    public void Savedata(GameData gameData)
    {
        if (gameData.keys.ContainsKey(id))
        {
            gameData.keys.Remove(id);
        }
        gameData.keys.Add(id, collected);
    }
}
