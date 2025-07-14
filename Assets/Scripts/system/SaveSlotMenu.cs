using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SaveSlotMenu : MonoBehaviour
{
    public UIControl MainMenu;
    private SaveSlots[] SaveSlot;

    private bool isLoadingGame = false;
    private void Awake()
    {
        SaveSlot = this.GetComponentsInChildren<SaveSlots>();
    }
    public void OnSaveSlotCliked(SaveSlots saveSlot)
    {
        DiasbleMenuButtons();
        //update the selected profile ID
        DataPresistenceManager.Instance.ChangeSelectedProfileID(saveSlot.GetProfileID());

        //create new game
        if (!isLoadingGame)
        {
            DataPresistenceManager.Instance.NewGame();
        }
        DataPresistenceManager.Instance.SaveGame();

        //load the scene
        StartCoroutine(MainMenu.Load(SceneManager.GetActiveScene().buildIndex + 1));
    }
    public void OnBackCliked()
    {
        MainMenu.ActivateMenu();
        this.DiactivateMenu();
    }
    public void ActivateMenu(bool isloadingGame)
    {
        this.gameObject.SetActive(true);
        this.isLoadingGame = isloadingGame;
        
        Dictionary<string,GameData> profilesGameData = 
            DataPresistenceManager.Instance.GetAllProfileGameData();

        foreach (SaveSlots saveSlot in SaveSlot)
        {
            profilesGameData.TryGetValue(saveSlot.GetProfileID(), out GameData profileData);
            saveSlot.SetData(profileData);
            if (profileData == null && isloadingGame)
            {
                saveSlot.SetIntractable(false);
            }
            else
            {
                saveSlot.SetIntractable(true);

            }
        }
    }
    public void DiactivateMenu()
    {
        this.gameObject.SetActive(false);
    }
    public void DiasbleMenuButtons()
    {
        foreach (SaveSlots saveSlot in SaveSlot)
        {
            saveSlot.SetIntractable(false);
        }
    }
}
