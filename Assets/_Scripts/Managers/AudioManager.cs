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

        public void SpawnSFX(eSFXType type)
        {
            switch (type)
            {
                case eSFXType.Tap:

                    break;
                case eSFXType.Stack:

                    break;
                case eSFXType.Unstack:

                    break;
                case eSFXType.Money:

                    break;
                case eSFXType.Upgrade:

                    break;
            }
        }
    }
}