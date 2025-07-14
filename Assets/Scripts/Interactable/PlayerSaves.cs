using UnityEngine;
using TMPro;

public class PlayerSaves : MonoBehaviour
{
    private TextMeshProUGUI CoinText;
    public int currentCoins = 0;
    public AudioSource coinAudioSource;
    public AudioClip CoinCollect;
    private void Awake()
    {
        Transform UI = GameObject.Find("UI").transform;
        CoinText = UI.Find("Collectables").Find("AddMoney").GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Start()
    {
        if (PlayerPrefs.HasKey("Coins"))
        {
            currentCoins = PlayerPrefs.GetInt("Coins");
        }
        CoinText.text = currentCoins.ToString();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coins"))
        {
            coinAudioSource.pitch = Random.Range(1, 2);
            coinAudioSource.PlayOneShot(CoinCollect,1);
            Destroy(other.gameObject);
            currentCoins += 1;
            PlayerPrefs.SetInt("Coins", currentCoins);
            CoinText.SetText(currentCoins.ToString());
        }
    }
    public void Update()
    {
        CoinText.SetText(currentCoins.ToString());
    }
}
