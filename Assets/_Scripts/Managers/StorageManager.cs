using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using Game.Tools;
using Game.Interfaces;

namespace Game.Managers
{

    public delegate void DataUpdated();
    public class StorageManager : Singleton<StorageManager>
    {
        [SerializeField] Transform m_LevelRoot;
        public GameData GameData => m_GameData;

        private GameData m_GameData;

        public static event DataUpdated OnGameDataUpdated;

        protected override void Awake()
        {
            base.Awake();

            initialize();
        }

        private void initialize()
        {
            m_GameData = new GameData();

            var unlockables = ToolsExtensions.FindObjectsOfInterface<IUnlockable>(m_LevelRoot);

            //print(unlockables.Count);
            for (int i = 0; i < unlockables.Count; i++)
            {
                m_GameData.PaidUnlockables.Add(0);
            }
            for (int i = 0; i < 4; i++)
            {
                m_GameData.WorkersCount.Add(0);
                m_GameData.WorkerSpeedLevel.Add(0);
                m_GameData.WorkerStackLevel.Add(0);
            }

            m_GameData = LoadData();

            if (unlockables.Count > m_GameData.PaidUnlockables.Count)
            {
                DeleteData();
                initialize();
                return;
            }

            for (int i = 0; i < unlockables.Count; i++)
            {
                unlockables[i].Initialize(i);
            }
        }

        private void OnEnable()
        {
            OnGameDataUpdated += save;
        }
        private void OnDisable()
        {
            OnGameDataUpdated -= save;
        }

        private void save()
        {
            SaveData(m_GameData);
        }

        // Save the game data to a file
        public void SaveData(GameData i_GameData)
        {
            string filePath = Path.Combine(Application.persistentDataPath, $"{typeof(GameData)}.json");
            string jsonData = JsonUtility.ToJson(i_GameData);

            File.WriteAllText(filePath, jsonData);
            Debug.Log($"Saved data to: {filePath}");
        }

        // Load the game data from a file
        public GameData LoadData()
        {
            string filePath = Path.Combine(Application.persistentDataPath, $"{typeof(GameData)}.json");

            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                GameData result = JsonUtility.FromJson<GameData>(jsonData);
                Debug.Log($"Loaded data from: {filePath}");
                return result;
            }
            else
            {
                SaveData(m_GameData);
                return m_GameData;
            }
        }

#if UNITY_EDITOR
        [MenuItem("Abdul Rab/DeleteData")]
#endif
        public static void DeleteData()
        {
            File.Delete(Path.Combine(Application.persistentDataPath, $"{typeof(GameData)}.json"));
        }

        #region Settings Setter

        public void SetMasterVolume(float i_Value) { m_GameData.MasterVolume = i_Value; OnGameDataUpdated?.Invoke(); }
        public void SetSFXVolume(float i_Value) { m_GameData.SFXVolume = i_Value; OnGameDataUpdated?.Invoke(); }
        public void SetMusicVolume(float i_Value) { m_GameData.MusicVolume = i_Value; OnGameDataUpdated?.Invoke(); }
        public void SetUseHaptic(bool i_Value) { m_GameData.UseHaptic = i_Value; OnGameDataUpdated?.Invoke(); }
        #endregion

        #region PlayerData Setter

        public void SetWallet(int i_Value) { m_GameData.PlayerWallet = i_Value; OnGameDataUpdated?.Invoke(); }
        public void SetSpeedLevel(int i_Value) { m_GameData.PlayerSpeedLevel = i_Value; OnGameDataUpdated?.Invoke(); }
        public void SetStackLevel(int i_Value) { m_GameData.PlayerStackLevel = i_Value; OnGameDataUpdated?.Invoke(); }
        public void SetVIPLevel(int i_Value) { m_GameData.VIPLevel = i_Value; OnGameDataUpdated?.Invoke(); }
        #endregion

        #region LevelData Setter

        public void SetPaidUnlockables(int i_Index, int i_Value) { m_GameData.PaidUnlockables[i_Index] = i_Value; OnGameDataUpdated?.Invoke(); }
        public void SetWorkersCount(int i_Index, int i_Value) { m_GameData.WorkersCount[i_Index] = i_Value; OnGameDataUpdated?.Invoke(); }
        public void SetWorkerSpeedLevel(int i_Index, int i_Value) { m_GameData.WorkerSpeedLevel[i_Index] = i_Value; OnGameDataUpdated?.Invoke(); }
        public void SetWorkerStackLevel(int i_Index, int i_Value) { m_GameData.WorkerStackLevel[i_Index] = i_Value; OnGameDataUpdated?.Invoke(); }
        #endregion

    }
    [System.Serializable]
    public class GameData
    {
        public float MasterVolume = 1f;
        public float SFXVolume = 1f;
        public float MusicVolume = 0.6f;
        public bool UseHaptic = true;

        public int PlayerWallet = 5000;
        public int PlayerSpeedLevel = 0;
        public int PlayerStackLevel = 0;
        public int VIPLevel = 0;

        public List<int> PaidUnlockables = new List<int>();
        public List<int> WorkersCount = new List<int>();
        public List<int> WorkerSpeedLevel = new List<int>();
        public List<int> WorkerStackLevel = new List<int>();
    }
}