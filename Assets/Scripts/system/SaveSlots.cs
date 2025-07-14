using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlots : MonoBehaviour
{
    [Header("Profile")]
    [SerializeField] private string profileID = "";

    [Header("Content")]
    [SerializeField] private GameObject noDataContent;
    [SerializeField] private GameObject hasDataContent;
    [SerializeField] private TextMeshProUGUI precentageCompleteText;

    private Button SaveSlotButton;

    private void Awake()
    {
        SaveSlotButton = GetComponent<Button>();
    }
    public void SetData(GameData data)
    {
        if(data == null)
        {
            noDataContent.SetActive(true);
            hasDataContent.SetActive(false);
        }
        else
        {
            noDataContent.SetActive(false);
            hasDataContent.SetActive(true);
            precentageCompleteText.text = data.GetPrecentageComplete() + "% COMPLETE"; 
        }
    }
    public string GetProfileID()
    {
        return this.profileID;
    }
    public void SetIntractable(bool intractable)
    {
        SaveSlotButton.interactable = intractable;
    }
}
