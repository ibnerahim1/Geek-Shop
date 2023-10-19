using Game.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;
using GameAnalyticsSDK;

namespace Game.Managers
{

    public class GameManager : Singleton<GameManager>
    {
        private void Start()
        {
            GameAnalytics.Initialize();
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "World0001");
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(1))
                reloadLevel();
#endif
        }

        public void GameOver()
        {
        }

        private void reloadLevel()
        {
            SceneManager.LoadScene(0);
        }
    }
}