using UnityEngine;
using UnityEngine.SceneManagement;
using Game.Shared;


/**
 * Game scene controller.
 */
public class GameController : MonoBehaviour {

    /** Player instance */
    public PlayerController player;

    /** Pause overlay controller */
    public PauseController pause;

    /** Game over overlay controller */
    public EndgameController endgame;


    /**
     * Initialize the game.
     */
    private void Start() {
        player.status.Reset();
        MessageService.Push("GOTTA FIND THE BOOKS BEFORE IT GETS TOO LATE", 1.0f);
    }


    /**
     * Loads the main menu scene.
     */
    public void LoadMainScene() {
        SceneManager.LoadScene("Main");
    }


    /**
     * Shows the game over overlay.
     */
    public void ShowGameOverOverlay() {
        endgame.ShowGameOverOverlay();
    }


    /**
     * Attach the events.
     */
    private void OnEnable() {
        player.playerKilled += OnPlayerKilled;
    }


    /**
     * Detach the events.
     */
    private void OnDisable() {
        player.playerKilled -= OnPlayerKilled;
    }


    /**
     * Handle when the player dies.
     */
    public void OnPlayerKilled(PlayerController player) {
        pause.enabled = false;
        AudioService.StopLoop(gameObject);
        Invoke("ShowGameOverOverlay", 2.5f);
        Invoke("LoadMainScene", 4.5f);
    }
}
