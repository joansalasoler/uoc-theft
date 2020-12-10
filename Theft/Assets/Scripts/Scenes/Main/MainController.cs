using UnityEngine;
using UnityEngine.SceneManagement;


/**
 * Main menu scene controller.
 */
public class MainController : MonoBehaviour {

    /**
     * Start a new game.
     */
    public void StartNewGame() {
        SceneManager.LoadScene("Game");
    }


    /**
     * Exit the game.
     */
    public void QuitApplication() {
        Application.Quit();
    }
}
