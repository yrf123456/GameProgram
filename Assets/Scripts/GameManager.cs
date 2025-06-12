using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class GameManager : MonoBehaviour
    {
        // Singleton instance
        private static GameManager _instance;

        public static GameManager Instance
        {
            get
            {
                _instance = FindObjectOfType<GameManager>();
                if (_instance == null)
                {
                    GameObject managerObject = new GameObject(nameof(GameManager));
                    _instance = managerObject.AddComponent<GameManager>();
                    DontDestroyOnLoad(managerObject);
                }

                return _instance;
            }
        }

        public GameObject PauseUI;

        public string levelName = "TeachingLevel";

        // Set the current level name
        public void SetLevelName(string name)
        {
            levelName = name;
        }

        // Current difficulty
        public float currDifficulty = 1;

        public void SetCurrDifficulty(float difficulty)
        {
            currDifficulty = difficulty;
        }

        private void Update()
        {
            // Listen for ESC key
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // Check current scene using new API
                if (SceneManager.GetActiveScene().name == "Main") return;

                PauseUI.gameObject.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }
}
