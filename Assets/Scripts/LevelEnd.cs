using DefaultNamespace.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DefaultNamespace
{
    public class LevelEnd : MonoBehaviour
    {
        public LayerMask targetLayers; // Select target layers in the Inspector

        public bool isChangeAudio;

        // Target scene to switch to
        public string levelName;

        public GameObject Decrypts;

        void OnCollisionEnter2D(Collision2D collision)
        {
            // Check if the collided object belongs to the target layers
            if ((targetLayers & (1 << collision.gameObject.layer)) == 0)
                return;

            if (Decrypts != null)
            {
                Decrypts.GetComponent<Decrypts>().ShowDecrypts();
            }
            else
            {
                // Switch scene
                SceneManager.LoadScene(levelName);
                GameManager.Instance.SetLevelName(levelName);

                // Switch background music
                if (isChangeAudio)
                {
                    AudioManager.Instance.SwitchBackgroundMusic(2);
                } 
            }
        }
    }
}
