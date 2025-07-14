using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler
{
    private string dataDirPath = "";
    private string dataFileName = "";
    private bool useEncryption = false;
    private readonly string encryptionCodeWord = "word";

    public FileDataHandler(string dataDirPath, string dataFileName, bool useEncryption)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
        this.useEncryption = useEncryption;
    }

    public GameData Load(string profileID)
    {
        //base case
        if (profileID == null)
            return null;

        string fullPath = Path.Combine(dataDirPath,profileID, dataFileName);
        GameData LoadgameData = null;
        if (File.Exists(fullPath))
        {
            try
            {
                // load the serialized data from the file
                string dataToLoad = "";
                using (FileStream stream = new(fullPath, FileMode.Open))
                {
                    using StreamReader reader = new(stream);
                    dataToLoad = reader.ReadToEnd();
                }

                //optionally decrypt the data
                if (useEncryption)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }

                // deserialize the data from json back inot the C# object
                LoadgameData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                
                Debug.LogError("Error second time trying to load file at: "+ fullPath + "\n" + e);
            }
        }
        return LoadgameData;
    }
    public void Save(GameData data,string profileID)
    {
        // base case
        if (profileID == null)
            return;
        // use Path.Combine to account for different OS's having different path seperators
        string fullPath = Path.Combine(dataDirPath,profileID, dataFileName);
        try
        {
            //create the directory the file will be written to it doesnt't already exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            //serialize the C# game data object into jason
            string dataToStore = JsonUtility.ToJson(data,true);

            // optionally encrypt the data
            if (useEncryption)
            {
                dataToStore = EncryptDecrypt(dataToStore);
            }

            //write the serialized data to the file
            using FileStream stream = new(fullPath, FileMode.Create);
            using StreamWriter writer = new(stream);
            writer.Write(dataToStore);
        }
        catch (Exception e)
        {
            Debug.LogError("while saving file: "+fullPath+"\n"+e);
        }
    }
    private string EncryptDecrypt(string data)
    {
        string modifiedData = "";
        for(int i=0; i < data.Length; i++)
        {
            modifiedData += (char)(data[i] ^ encryptionCodeWord[i % encryptionCodeWord.Length]);
        }
        return modifiedData;
    }
    public string GetMostRecentlyUpdatedProfileID()
    {
        string mostRecentlyProfileID = null;
        Dictionary<string, GameData> ProfileGameData = LoadAllProfiles();
        foreach(KeyValuePair<string, GameData> pair in ProfileGameData)
        {
            string profileID = pair.Key;
            GameData data = pair.Value;

            if(data == null)
            {
                continue;
            }
            // data exist
            if(mostRecentlyProfileID == null)
            {
                mostRecentlyProfileID = profileID;
            }
            else
            {
                DateTime mostRecentDateTime =
                    DateTime.FromBinary(ProfileGameData[mostRecentlyProfileID].lastUpdated);
                DateTime newDateTime = DateTime.FromBinary(data.lastUpdated);

                if(newDateTime > mostRecentDateTime)
                {
                    mostRecentlyProfileID = profileID;
                }
            }
        }
        return mostRecentlyProfileID;
    }
    public Dictionary<string,GameData> LoadAllProfiles()
    {
        Dictionary<string, GameData> profileDictionary = new Dictionary<string, GameData>();

        IEnumerable<DirectoryInfo> dirinfo = new DirectoryInfo(dataDirPath).EnumerateDirectories();
        foreach (DirectoryInfo dir in dirinfo)
        {
            string profileID = dir.Name;
            string fullPath = Path.Combine(dataDirPath,profileID,dataFileName);
            if (!File.Exists(fullPath))
            {
                Debug.LogWarning("Skipping directory when loading" + profileID);
                continue;
            }

            GameData profileData = Load(profileID);
            if(profileData != null)
            {
                profileDictionary.Add(profileID, profileData);
            }
            else
            {
                Debug.LogError("Tried to load profile bit something went wrong: "+ profileID);
            }
        }

        return profileDictionary;
    }
}
