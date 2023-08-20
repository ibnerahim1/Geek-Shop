using MoreMountains.NiceVibrations;
using Game.Tools;

namespace Game.Managers
{

    public class HapticManager : Singleton<HapticManager>
    {
        private GameData m_GameData => StorageManager.Instance.GameData;

        public void Play(HapticTypes type)
        {
            if (m_GameData.UseHaptic)
            {
                switch (type)
                {
                    case HapticTypes.Selection:
                        MMVibrationManager.Haptic(HapticTypes.Selection);
                        break;
                    case HapticTypes.Success:
                        MMVibrationManager.Haptic(HapticTypes.Success);
                        break;
                    case HapticTypes.Warning:
                        MMVibrationManager.Haptic(HapticTypes.Warning);
                        break;
                    case HapticTypes.Failure:
                        MMVibrationManager.Haptic(HapticTypes.Failure);
                        break;
                    case HapticTypes.LightImpact:
                        MMVibrationManager.Haptic(HapticTypes.LightImpact);
                        break;
                    case HapticTypes.MediumImpact:
                        MMVibrationManager.Haptic(HapticTypes.MediumImpact);
                        break;
                    case HapticTypes.HeavyImpact:
                        MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
                        break;
                    case HapticTypes.RigidImpact:
                        MMVibrationManager.Haptic(HapticTypes.RigidImpact);
                        break;
                    case HapticTypes.SoftImpact:
                        MMVibrationManager.Haptic(HapticTypes.SoftImpact);
                        break;
                }

            }
            else
                print("Haptic Turned OFF");
        }
    }
}