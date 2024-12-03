using System;
using System.Collections;
using PlayerControl;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace UiMenu
{
    public class Game : MonoBehaviour
    {
    
        public static Game Instance { get; private set; } // Référence statique à l'instance unique.
        
        [SerializeField] private GameObject pauseMenu;
        private InputAction _pauseAction;
        private bool _isPaused;
        private bool _pauseButtonReleased;
        
        private void Awake()
        {
            // Vérifie s'il existe déjà une instance et la détruit si nécessaire.
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }
        
        private void OnEnable()
        {
            var inputActions = InputSystem.actions;
            _pauseAction = inputActions.FindAction("Pause");
            if (_pauseAction == null)
            {
                Debug.LogError("Action 'Pause' introuvable dans l'Input System.");
                return;
            }

            _pauseAction.performed += OnPausePerformed;
            _pauseAction.canceled += OnPauseCanceled; // Détecte le relâchement du bouton
            _pauseAction.Enable();
            pauseMenu.SetActive(false);
            _pauseButtonReleased = true;
            ResumeGame();
        }
    
        public static IEnumerator OnDeath()
        {
            var p = FindAnyObjectByType<Player>();
            var a = p.Animator.GetCurrentAnimatorStateInfo(2).length;
            var es = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            p.enabled = false;
            foreach (var e in es)
            {
                e.enabled = false;
            }
            yield return new WaitForSeconds(a + 0.1f);
            SceneManager.LoadScene("Level 1");
        }

        private void OnDisable()
        {
            if (_pauseAction == null) return;
            _pauseAction.performed -= OnPausePerformed;
            _pauseAction.canceled -= OnPauseCanceled;
            _pauseAction.Disable();
            PauseGame();
        }
        
        private void OnPauseCanceled(InputAction.CallbackContext context)
        {
            _pauseButtonReleased = true; // Permet à nouveau de réagir au prochain appui.
        }
        
        private void OnPausePerformed(InputAction.CallbackContext context)
        {
            if (!_pauseButtonReleased) return; // Ignore si le bouton est encore maintenu.
        
            _pauseButtonReleased = false; // Verrouille la pause jusqu'à ce que le bouton soit relâché.
            TogglePause();
        }

        public void TogglePause()
        {
            _isPaused = !_isPaused;

            if (_isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }

        private void PauseGame()
        {
            Time.timeScale = 0f; // Stoppe le temps de jeu.
            Cursor.lockState = CursorLockMode.None; // Libérer le curseur.
            Cursor.visible = true;
            var playerAction = InputSystem.actions.FindActionMap("Player");
            playerAction?.Disable();
            if (pauseMenu != null)
            {
                pauseMenu.SetActive(true);
            }
        }
        
        private void ResumeGame()
        {
            Time.timeScale = 1f; // Reprend le temps de jeu.
            Cursor.lockState = CursorLockMode.Locked; // Re-verrouiller le curseur.
            Cursor.visible = false;
            var playerAction = InputSystem.actions.FindActionMap("Player");
            playerAction?.Enable();
            if (pauseMenu != null)
            {
                pauseMenu.SetActive(false);
            }
        }
        
    }
}
