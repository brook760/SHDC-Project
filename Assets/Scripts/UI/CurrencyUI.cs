using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyUI : MonoBehaviour,IDataPresistence
{
    [Header("Death")]
    public int DeathCount = 0;
    public TextMeshProUGUI DeathText;

    [Header("Crystals")]
    public int crystalCollected = 0;
    public TextMeshProUGUI Crystaltext;

    public static CurrencyUI instance;
    public bool Winner= false;
    private void Start()
    {
        if (instance == null)
            instance = this;
    }
    public void PlayerDeath()
    {
        DeathCount +=1;
    }
    public void CrystalCollected()
    {
        crystalCollected ++;
    }

    // Update is called once per frame
    void Update()
    {
        DeathText.text = DeathCount.ToString();
        Crystaltext.text = crystalCollected.ToString() +" / 20 ";
        if(crystalCollected == 20)
        {
            Winner = true;
        }
    }
    public int DeathPercentage()
    {
        int Marks;
        if (DeathCount == 0)
        {
            Marks = 3;
        }
        else if(DeathCount <= 10)
        {
            Marks = 2;
        }
        else
        {
            Marks = 1;
        }
        return Marks;
    }

    public void LoadData(GameData gameData)
    {
        DeathCount = gameData.DeathCount;
        foreach(KeyValuePair<string, bool> pair in gameData.crystalsCollected)
        {
            if (pair.Value)
            {
                crystalCollected++;
            }
        }
    }

    public void Savedata(GameData gameData)
    {
        gameData.DeathCount = DeathCount;
    }
}
