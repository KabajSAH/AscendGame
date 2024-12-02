using UnityEngine;
using UnityEngine.SceneManagement;

namespace UiMenu
{
    public class LevelMenu : MonoBehaviour
    {
        public void Restart()
        {
            SceneManager.LoadScene("Level 1");
        }

        public void ToMain()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void Continue()
        {
            Game.Instance.TogglePause();
        }
        
    }
}
