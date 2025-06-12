using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DefaultNamespace.UI
{
    public class Decrypts : MonoBehaviour
    {
        public GameObject decrypts1;
        public Button decryptsBtnA;
        public Button decryptsBtnB;
        public Button decryptsBtnC;
        public Button decryptsBtnD;

        public GameObject decrypts2;
        public Button decrypts2BtnA;
        public Button decrypts2BtnB;
        public Button decrypts2BtnC;
        public Button decrypts2BtnD;

        // Error prompt
        public GameObject error;
        // Target scene to switch to
        public string levelName = "Level2";

        private void Awake()
        {
            decryptsBtnA.onClick.AddListener(() =>
            {
                error.gameObject.SetActive(true);
            });
            decryptsBtnB.onClick.AddListener(() =>
            {
                error.gameObject.SetActive(true);
            });
            decryptsBtnC.onClick.AddListener(() =>
            {
                error.gameObject.SetActive(true);
            });
            decryptsBtnD.onClick.AddListener(() =>
            {
                decrypts1.gameObject.SetActive(false);
                decrypts2.gameObject.SetActive(true);
                error.gameObject.SetActive(false);
            });

            decrypts2BtnA.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(levelName);
                GameManager.Instance.SetLevelName(levelName);
                AudioManager.Instance.SwitchBackgroundMusic(2);
            });
            decrypts2BtnB.onClick.AddListener(() =>
            {
                error.gameObject.SetActive(true);
            });
            decrypts2BtnC.onClick.AddListener(() =>
            {
                error.gameObject.SetActive(true);
            });
            decrypts2BtnD.onClick.AddListener(() =>
            {
                error.gameObject.SetActive(true);
            });
        }

        // Show decryption interface
        public void ShowDecrypts()
        {
            decrypts1.gameObject.SetActive(true);
        }
    }
}
