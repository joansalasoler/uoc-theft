using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


/**
 * Main menu scene controller.
 */
public class MainController : MonoBehaviour {

    /** Reference to the menu canvas */
    [SerializeField] private Canvas menuCanvas = null;

    /** Reference to the options canvas */
    [SerializeField] private Canvas optionsCanvas = null;

    /** Reference to the main button group */
    [SerializeField] private GameObject mainGroup = null;


    /**
     * Start a new game.
     */
    public void StartNewGame() {
        SceneManager.LoadScene("Game");
    }


    /**
     * Show the options canvas.
     */
    public void ShowOptions() {
        mainGroup.SetActive(false);
        optionsCanvas.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        optionsCanvas.GetComponentInChildren<Selectable>().Select();
    }


    /**
     * Hide the options canvas.
     */
    public void HideOptions() {
        mainGroup.SetActive(true);
        optionsCanvas.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        menuCanvas.GetComponentInChildren<Selectable>().Select();
    }


    /**
     * Exit the game.
     */
    public void QuitApplication() {
        Application.Quit();
    }
}
