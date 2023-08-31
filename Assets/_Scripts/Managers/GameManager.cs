using Game.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace Game.Managers
{

    public class GameManager : Singleton<GameManager>
    {
        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(1))
                reloadLevel();
#endif
        }

        private void InitializeGame()
        {
        }

        public void GameOver()
        {
        }

        private void reloadLevel()
        {
            DOTween.KillAll();
            SceneManager.LoadScene(0);
        }
    }
}