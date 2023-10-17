using Game.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

namespace Game.Managers
{

    public class GameManager : Singleton<GameManager>
    {
        public TextMeshProUGUI log;

        private void OnEnable()
        {
            Player.onTrigger += Triggered;
        }
        private void OnDisable()
        {
            Player.onTrigger -= Triggered;
        }

        public void Triggered(Counter tra)
        {
            log.text = $"{tra.name} enabled: {tra.enabled}";
        }

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
            SceneManager.LoadScene(0);
        }
    }
}