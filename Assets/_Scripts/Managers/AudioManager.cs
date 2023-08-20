using UnityEngine;
using Game.Tools;

namespace Game.Managers
{

    public class AudioManager : Singleton<AudioManager>
    {
        private GameData m_GameData => StorageManager.Instance.GameData;

        public AudioSource MusicSource;
        public AudioSource SFXSource;

        private void OnEnable()
        {
            StorageManager.OnGameDataUpdated += updateAudio;
        }
        private void OnDisable()
        {
            StorageManager.OnGameDataUpdated -= updateAudio;
        }

        private void updateAudio()
        {
            MusicSource.volume = m_GameData.MusicVolume * m_GameData.MasterVolume;
            SFXSource.volume = m_GameData.SFXVolume * m_GameData.MasterVolume;
        }

        public void SpawnSFX(SFXType type)
        {
            switch (type)
            {
                case SFXType.Tap:

                    break;
                case SFXType.Stack:

                    break;
                case SFXType.Unstack:

                    break;
                case SFXType.Money:

                    break;
                case SFXType.Upgrade:

                    break;
            }
        }
    }
}