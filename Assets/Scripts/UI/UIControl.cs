using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIControl : MonoBehaviour
{
    [Header("Transition")]
    public Animator transition;
    public float trasnsitionTime = 1f;
    public GameObject Loadingscreen;
    public Slider loader;
    public TextMeshProUGUI prText;

    [Header("Menu Buttons")]
    public GameObject MainMenu;
    public GameObject PlayMenu;
    public SaveSlotMenu saveSlotMenu;

    [Header("Game Settings")]
    public AudioMixer myMixer;
    public Slider VolumeSlider;
    public Slider SFXSlider;
    public bool normal = true;

    [Header("Character")]
    public GameObject[] contents;
    public Transform parent;
    public int ContentIndex;
    public TMP_Dropdown dropdownValue;
    public Slider Hp;
    public Slider Mana;
    public Slider Armour;

    [Header("Shop")]
    public GameObject[] shopContent;
    public Transform ShopParent;
    public int ShopIndex;
    public TMP_Dropdown shopdownValue;
    public GameObject[] stars;
    public int  CoinCount;
    public TextMeshProUGUI coins;

    [Header("SFX")]
    [Tooltip("The GameObject holding the Audio Source component for the HOVER SOUND")]
    public AudioSource hoverSound;
    [Tooltip("The GameObject holding the Audio Source component for the AUDIO SLIDER")]
    public AudioSource sliderSound;
    [Tooltip("The GameObject holding the Audio Source component for the SWOOSH SOUND when switching to the Settings Screen")]
    public AudioSource swooshSound;

    private void Start()
    {
        if (DataPresistenceManager.Instance.NoDataFound())
        {
            Debug.Log("NoSaveData");
        }
        int count = parent.childCount;
        contents = new GameObject[count];
        List<string> ObjList = new();

        for (int i = 0; i < count; i++)
        {
            contents[i] = parent.GetChild(i).gameObject;
            string n = parent.GetChild(i).name;
            ObjList.Add(n);

            if (i == 0) contents[i].SetActive(true);
            else contents[i].SetActive(false);
        }

        dropdownValue.AddOptions(ObjList);

        int shopCount = ShopParent.childCount;
        shopContent = new GameObject[shopCount];
        List<string> shopList = new();
        for (int i = 0; i < shopCount; i++)
        {
            shopContent[i] = ShopParent.GetChild(i).gameObject;
            string n = ShopParent.GetChild(i).name;
            shopList.Add(n);

            if (i == 0) shopContent[i].SetActive(true);
            else shopContent[i].SetActive(false);
        }

        shopdownValue.AddOptions(shopList);

        if (PlayerPrefs.HasKey("Music"))
        {
            LoadVolume();
        }
        else
        {
            SetVolume();
            SetSFX();
        }
    }

    #region mainmenu
    public void Continue()
    {
        DisableMenuButtons();
        DataPresistenceManager.Instance.SaveGame();
        StartCoroutine(Load(SceneManager.GetActiveScene().buildIndex + 1));
    }
    public void NewGame()
    {
        saveSlotMenu.ActivateMenu(false);
        this.DeactivateMenu();
    }
    public void LoadGame()
    {
        saveSlotMenu.ActivateMenu(true);
        this.DeactivateMenu();
    }
    public void ActivateMenu()
    {
        PlayMenu.SetActive(false);
        MainMenu.SetActive(true);

    }
    public void DeactivateMenu()
    {
        PlayMenu.SetActive(false);
        MainMenu.SetActive(false);

    }
    void DisableMenuButtons()
    {
        MainMenu.SetActive(false);
        PlayMenu.SetActive(false);
    }
    public IEnumerator Load(int level)
    {

        transition.SetTrigger("Start");
        yield return new WaitForSeconds(trasnsitionTime);
        Loadingscreen.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(level);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            loader.value = progress;
            prText.text = progress * 100f + "%";

            yield return null;
        }
    }
    #endregion

    #region SoundPlay
    public void PlayHover()
    {
        hoverSound.Play();
    }

    public void PlaySFXHover()
    {
        sliderSound.Play();
    }

    public void PlaySwoosh()
    {
        swooshSound.Play();
    }
    #endregion

    #region GameSettings
    public void SetDifficulty()
    {
        normal = !normal;
    }
    public void SetSFX()
    {
        float volume = SFXSlider.value;
        myMixer.SetFloat("SFX", MathF.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFX", volume);
    }
    public void SetVolume()
    {
        float volume = VolumeSlider.value;
        myMixer.SetFloat("Music",MathF.Log10(volume)*20);
        PlayerPrefs.SetFloat("Music", volume);
    }
    public void LoadVolume()
    {
        VolumeSlider.value = PlayerPrefs.GetFloat("Music");
        SFXSlider.value = PlayerPrefs.GetFloat("SFX");
        SetVolume();
        SetSFX();
    }
    #endregion

    #region character
    public void NextCharacter()
    {
        if (dropdownValue.value >= dropdownValue.options.Count - 1)
            dropdownValue.value = 0;
        else
            dropdownValue.value++;
        ChangeCharacter();
    }
    public void PreviousCharacter()
    {
        if (dropdownValue.value <= 0)
            dropdownValue.value = dropdownValue.options.Count - 1;
        else
            dropdownValue.value--;
        ChangeCharacter();
    }
    private void ChangeCharacter()
    {
        contents[ContentIndex].SetActive(false);
        contents[dropdownValue.value].SetActive(true);
        ContentIndex = dropdownValue.value;

        switch (ContentIndex)
        {
            case 0: Hp.value = 0.9f; Mana.value = 0.5f; Armour.value = 0.2f; break;
            case 1: Hp.value = 0.9f; Mana.value = 0.7f; Armour.value = 0.2f; break;
            case 2: Hp.value = 0.8f; Mana.value = 0.2f; Armour.value = 0.8f; break;
            case 3: Hp.value = 0.7f; Mana.value = 0.3f; Armour.value = 0.7f; break;
            case 4: Hp.value = 1f; Mana.value = 0.8f; Armour.value = 0.4f; break;
        }
    }
    public void Selectcharacter()
    {
       PlayerPrefs.SetInt("SelectedID", ContentIndex);
    }
    #endregion

    #region Shop
    public void OnClickShopIcon()
    {
        if (PlayerPrefs.HasKey("Coins"))
        {
            CoinCount = PlayerPrefs.GetInt("Coins");
        }
        coins.text = CoinCount.ToString();
    }
    public void NextShopContent()
    {
        if (shopdownValue.value >= shopdownValue.options.Count - 1)
            shopdownValue.value = 0;
        else
            shopdownValue.value++;
        ChangeShopContent();
    }
    public void PreviousShopContent()
    {
        if (shopdownValue.value <= 0)
            shopdownValue.value = shopdownValue.options.Count - 1;
        else
            shopdownValue.value--;
        ChangeShopContent();
    }
    private void ChangeShopContent()
    {
        shopContent[ShopIndex].SetActive(false);
        shopContent[shopdownValue.value].SetActive(true);
        ShopIndex = shopdownValue.value;

        switch (ShopIndex)
        {
            case 0: stars[0].SetActive(true);
                    stars[1].SetActive(true);
                    stars[2].SetActive(false);
                    stars[3].SetActive(false);
                     break;
            case 1: stars[0].SetActive(true);
                    stars[1].SetActive(true);
                    stars[2].SetActive(true);
                    stars[3].SetActive(false);
                    break;
            case 2: stars[0].SetActive(true);
                    stars[1].SetActive(false);
                    stars[2].SetActive(false);
                    stars[3].SetActive(false); 
                    break;
            case 3: stars[0].SetActive(true);
                    stars[1].SetActive(true);
                    stars[2].SetActive(true);
                    stars[3].SetActive(true);
                    break;
        }
    }
    public void SelectShopContent()
    {
        Debug.Log("Item: " + shopContent[ShopIndex].name + " buyed");
        CoinCount -= 3;
        PlayerPrefs.SetInt("Coins", CoinCount);
        coins.SetText(CoinCount.ToString());
    }
    #endregion
    //Quit menu
    public void QuitGame()
    {
	    Application.Quit();
    }
}
