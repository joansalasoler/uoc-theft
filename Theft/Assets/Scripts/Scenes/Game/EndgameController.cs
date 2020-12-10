using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/**
 * Endgame overlay controller.
 */
public class EndgameController : MonoBehaviour {

    /** Object to show when the player dies */
    [SerializeField] private GameObject gameOver = null;


    /**
     * Shows the game over overlay.
     */
    public void ShowGameOverOverlay() {
        gameOver.SetActive(true);
    }
}
