using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public long lastUpdated;
    public int DeathCount;
    public Vector3 playerPostion;
    public SerializableDictionary<string, bool> crystalsCollected;
    public SerializableDictionary<string, bool> keys;
    public GameData()
    {
        DeathCount = 0;
        playerPostion = Vector3.zero;
        crystalsCollected = new SerializableDictionary<string, bool>();
        keys = new SerializableDictionary<string, bool>();
    }
    public int GetPrecentageComplete()
    {
        int totalCollected = 0;
        foreach(bool collected in crystalsCollected.Values)
        {
            if (collected)
                totalCollected++;
        }
        int percentageComplete = -1;
        if(crystalsCollected.Count!= 0)
        {
            percentageComplete = (totalCollected * 100 / crystalsCollected.Count);
        }
        return percentageComplete;
    }
}
