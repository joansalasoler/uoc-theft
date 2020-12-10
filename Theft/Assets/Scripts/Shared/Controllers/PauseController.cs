using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

namespace Game.Shared {

    /**
     * Controller for the pause overlay.
     */
    public class PauseController : MonoBehaviour {

        /** Pause screen overlay */
        [SerializeField] private GameObject overlay = null;

        /** Name of the scene to load on exit */
        [SerializeField] private string menuScene = "Main";

        /** Name of the scene to load on restart */
        [SerializeField] private string gameScene = "Game";

        /** Wether the pause overlay is shown */
        private bool isPaused = false;


        /**
         * Set the pause status on start.
         */
        private void Start() {
            SetPause(overlay.activeSelf);
        }


        /**
         * Toggle the pause when the Cancel is pressed.
         */
        private void Update() {
            if (Input.GetButtonUp("Cancel")) {
                SetPause(!isPaused);
            }
        }


        /**
         * Pause the game.
         */
        public void Pause() {
            SetPause(true);
        }


        /**
         * Unpause the game.
         */
        public void Resume() {
            SetPause(false);
        }


        /**
         * Restart the game scene.
         */
        public void Restart() {
            SetPause(false);
            SceneManager.LoadScene(gameScene);
        }


        /**
         * Load the menu scene.
         */
        public void Exit() {
            SetPause(false);
            SceneManager.LoadScene(menuScene);
        }


        /**
         * Toggle the game pause overlay.
         */
        public void SetPause(bool mustPause) {
            isPaused = mustPause;
            AudioListener.pause = mustPause;
            Time.timeScale = mustPause ? 0.0f : 1.0f;
            overlay.SetActive(mustPause);
            if (isPaused) SelectFirstButton();
        }


        /**
         * Select the first button of the pause overlay.
         */
        private void SelectFirstButton() {
            EventSystem.current.SetSelectedGameObject(null);
            overlay.GetComponentInChildren<Button>().Select();
        }
    }
}
