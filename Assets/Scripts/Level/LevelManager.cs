
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<LevelConfig> ConfigList;
    [HideInInspector]
    public GameData CurrentData;
    // Start is called before the first frame update
    void Start()
    {
        Load();
        bool IsNewLevelAdded = false;
        for(int i = 0; i < ConfigList.Count; i ++)
        {
            if(i > (CurrentData.LevelList.Count - 1))
            {
                IsNewLevelAdded = true;
                CurrentData.LevelList.Add(new LevelData { IsCompleted = false, IsLocked = true});
            }
        }
        if(IsNewLevelAdded)
        {

            OnNewLevelAddedOrLevelPassed();
        }
    }
    public void Save(GameData Data)
    {
        string JsonFile = JsonUtility.ToJson(Data);
        File.WriteAllText(Application.persistentDataPath + "/GameData.json", JsonFile);
    }
    public void Load() 
    {

        string FilePath = Application.persistentDataPath + "/GameData.json";
        if (File.Exists(FilePath))
        {
            string JsonFile = File.ReadAllText(FilePath);
            CurrentData = JsonUtility.FromJson<GameData>(JsonFile);
        }
    }
    public void LevelPassed(LevelConfig config)
    {
        int Index = ConfigList.FindIndex(a => a == config);
        if(Index >= 0 && Index < CurrentData.LevelList.Count)
        {
            CurrentData.LevelList[Index].IsCompleted = true;
        }
        OnNewLevelAddedOrLevelPassed();
    }
    public void OnNewLevelAddedOrLevelPassed()
    {
        for(int i = 0; i< CurrentData.LevelList.Count - 1; i ++)
        {
            if(i == 0 && CurrentData.LevelList[i].IsLocked)
            {
                CurrentData.LevelList[i].IsLocked = false;
            }
            if (CurrentData.LevelList[i].IsCompleted && CurrentData.LevelList[i + 1].IsLocked)
            {
                CurrentData.LevelList[i + 1].IsLocked = false;
            }
        }
        Save(CurrentData);
    }
    public int FindNextLevel(LevelConfig config)
    {
        int Index = ConfigList.FindIndex(a => a == config);
        if(Index >= 0 && (Index + 1) < ConfigList.Count)
        {
            return Index + 1;
        }
        return -1;
    }
}
