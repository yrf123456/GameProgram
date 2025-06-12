using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class EndUI : MonoBehaviour
    {
        public GameObject MainUI;
        public GameObject LevelsUI;

        public Button selectBtn;
        public Button level1Btn;
        public Button level2Btn;
        public Button level3Btn;
        public Text currlevelText;
        public Text mainCurrlevelText;

        public Slider slider;

        public Text difficultyText;

        public float currDifficulty;

        private string currLevel = "TeachingLevel";

        private void Awake()
        {
            // Difficulty slider callback
            slider.onValueChanged.AddListener(value =>
            {
                currDifficulty = value;
                difficultyText.text = currDifficulty + "";
                GameManager.Instance.SetCurrDifficulty(currDifficulty);
            });

            // Show level selection UI
            selectBtn.onClick.AddListener(() =>
            {
                MainUI.SetActive(false);
                LevelsUI.SetActive(true);
            });

            // Level 1 selected
            level1Btn.onClick.AddListener(() =>
            {
                currLevel = "TeachingLevel";
                currlevelText.text = "Current: " + currLevel;
                mainCurrlevelText.text = currLevel;
                GameManager.Instance.SetLevelName(currLevel);
                MainUI.SetActive(true);
                LevelsUI.SetActive(false);
            });

            // Level 2 selected
            level2Btn.onClick.AddListener(() =>
            {
                currLevel = "Level1";
                currlevelText.text = "Current: " + currLevel;
                mainCurrlevelText.text = currLevel;
                GameManager.Instance.SetLevelName(currLevel);
                MainUI.SetActive(true);
                LevelsUI.SetActive(false);
            });

            // Level 3 selected
            level3Btn.onClick.AddListener(() =>
            {
                currLevel = "Level2";
                currlevelText.text = "Current: " + currLevel;
                mainCurrlevelText.text = currLevel;
                GameManager.Instance.SetLevelName(currLevel);
                MainUI.SetActive(true);
                LevelsUI.SetActive(false);
            });

            // Set current level display at startup
            currlevelText.text = "Current: " + GameManager.Instance.levelName;
            mainCurrlevelText.text = GameManager.Instance.levelName;
        }

        private void Start()
        {
            // Initialize difficulty slider value
            slider.value = GameManager.Instance.currDifficulty;
        }

        // Reload current level
        public void OnClickReset()
        {
            string levelName = GameManager.Instance.levelName;
            SceneManager.LoadScene(levelName);
        }

        // Exit the game
        public void ExitGame()
        {
            Application.Quit(); // Actually quit the game

            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in editor
            #endif
        }
    }
}
