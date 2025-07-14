using UnityEngine;

public class Crystals : MonoBehaviour,IDataPresistence
{

    [SerializeField] private string id;
    public AudioSource Source;
    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }
    private MeshRenderer visual;

    bool collected = false;

    private void Awake()
    {
        visual = GetComponentInChildren<MeshRenderer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!collected)
        {
            if (other.CompareTag("Player"))
            {
                Source.PlayOneShot(Source.clip, 1);
                visual.enabled = false;
                collected = true;
                CurrencyUI.instance.CrystalCollected();
            }
        }
    }

    public void LoadData(GameData gameData)
    {
        gameData.crystalsCollected.TryGetValue(id, out collected);
        if (collected)
        {
            visual.gameObject.SetActive(false);
        }
    }

    public void Savedata(GameData gameData)
    {
        if (gameData.crystalsCollected.ContainsKey(id))
        {
            gameData.crystalsCollected.Remove(id);
        }
        gameData.crystalsCollected.Add(id, collected);
    }
}
