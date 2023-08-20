using Game.Tools;

namespace Game.Managers
{

    public class SettingsManager : Singleton<SettingsManager>
    {
        private StorageManager m_Settings => StorageManager.Instance;

        public void SetMasterVolume(int i_MasterVolume)
        {
            m_Settings.SetMasterVolume(i_MasterVolume);
        }
        public void SetSFXVolume(int i_SFXVolume)
        {
            m_Settings.SetSFXVolume(i_SFXVolume);
        }
        public void SetMusicVolume(int i_MusicVolume)
        {
            m_Settings.SetMusicVolume(i_MusicVolume);
        }
        public void SetHaptic(bool i_UseHaptic)
        {
            m_Settings.SetUseHaptic(i_UseHaptic);
        }
    }
}