using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class GameSaveData : MonoBehaviour
{
    public bool saveNow;
    public SelectedObjectsBehavior[] objectsDataInGame;
    public bool gliderBroken;
    public string sceneToLoad;
    public class SavedGameData
    {
        public string sceneName;
        public bool gliderBroken;
    }
    public class InteractedData
    {
        public bool previouslySelected;
    }
    

    void Start()
    {
        objectsDataInGame = FindObjectsOfType<SelectedObjectsBehavior>();
        if (Directory.Exists(Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData/" + SceneManager.GetActiveScene().name))
        {
            Load();
        }
        else
        {
            Save();
            Load();
        }

        if (File.Exists(Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData/" + "GliderData.json"))
        {
            LoadGliderData();
        }
        else
        {
            SaveGliderData();
            LoadGliderData();
        }

    }

    public void NewGame()
    {
        string savePath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData" + "/";
        Directory.Delete(savePath, true);
        Save();
        Load();
    }
    public void Save()
    {
        string savePath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData" + "/";
        InteractedData data = new InteractedData();
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
        if (!Directory.Exists(savePath + SceneManager.GetActiveScene().name))
        {;
            Directory.CreateDirectory(savePath + SceneManager.GetActiveScene().name);
        }
        for (int i =0; i < objectsDataInGame.Length; i++)
        {
            savePath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData" + "/" + SceneManager.GetActiveScene().name + "/";
            savePath = savePath + objectsDataInGame[i].transform.gameObject.name + ".json";

            data.previouslySelected = objectsDataInGame[i].previouslySelected;
            string json = JsonUtility.ToJson(data);
            using StreamWriter writer = new StreamWriter(savePath);
            writer.Write(json);
            writer.Close();
        }
        SaveGliderData();
    }
    public void Load()
    {
        for (int i = 0; i < objectsDataInGame.Length; i++)
        {
            string savePath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData" + "/" + SceneManager.GetActiveScene().name + "/";
            savePath = savePath + objectsDataInGame[i].transform.gameObject.name + ".json";
            using StreamReader reader = new StreamReader(savePath);
            string json = reader.ReadToEnd();
            InteractedData loadedData = JsonUtility.FromJson<InteractedData>(json);
            objectsDataInGame[i].previouslySelected = loadedData.previouslySelected;
        }
        LoadGliderData();
    }
    public void OnApplicationQuit()
    {
        if (SceneManager.GetActiveScene().name != "MainMenuScene")
        {
            Save();
        }
    }

    public void SaveGliderData()
    {
        SavedGameData data = new SavedGameData();
        string savePath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData/" + "GliderData.json";
        data.gliderBroken = gliderBroken;
        data.sceneName = SceneManager.GetActiveScene().name;
        string json = JsonUtility.ToJson(data);
        using StreamWriter writer = new StreamWriter(savePath);
        writer.Write(json);
        writer.Close();
    }
    public void LoadGliderData()
    {
        string savePath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData/" + "GliderData.json";
        using StreamReader streamReader = new StreamReader(savePath);
        string json = streamReader.ReadToEnd();
        SavedGameData loadedData = JsonUtility.FromJson<SavedGameData>(json);
        gliderBroken = loadedData.gliderBroken;
        sceneToLoad = loadedData.sceneName;
    }
}
