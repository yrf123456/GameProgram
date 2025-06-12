using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class PauseUI : MonoBehaviour
    {
        public Button ContinueGameBtn;
        public Button BackBtn;

        private void Awake()
        {
            ContinueGameBtn.onClick.AddListener(() =>
            {
                Time.timeScale = 1f; // Resume the game
                this.gameObject.SetActive(false);
            });

            BackBtn.onClick.AddListener(() =>
            {
                Time.timeScale = 1f; // Resume the game before switching scenes
                this.gameObject.SetActive(false);
                GameManager.Instance.SetLevelName("TeachingLevel");
                SceneManager.LoadScene("Main");
            });
        }
    }
}
