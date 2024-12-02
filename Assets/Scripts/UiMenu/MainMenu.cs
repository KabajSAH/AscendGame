using UnityEngine;
using UnityEngine.SceneManagement;

namespace UiMenu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject main;
        [SerializeField] private GameObject level;
        [SerializeField] private GameObject controls;
        // Start is called before the first frame update
        void Start()
        {
            main.SetActive(true);
            level.SetActive(false);
            controls.SetActive(false);
        }

        public void ToLevelSelect()
        {
            main.SetActive(false);
            level.SetActive(true);
            controls.SetActive(false);
        }

        public void ToControles()
        {
            main.SetActive(false);
            level.SetActive(false);
            controls.SetActive(true);
        }

        public void GoBack()
        {
            main.SetActive(true);
            level.SetActive(false);
            controls.SetActive(false);
        }
    
        public void ToQuit()
        {
            Application.Quit();
        }

        public void ToLevel1()
        {
            SceneManager.LoadScene("Level 1");
        }

        public void ToLevel2()
        {
        
        }
    }
}
