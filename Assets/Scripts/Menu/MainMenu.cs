using UnityEngine;
using UnityEngine.SceneManagement; // Needed to load scenes

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // Loads the next scene in the Build Settings list (Index 1)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!"); // Verify this works in the editor
        Application.Quit(); // Closes the game (only works in the built .exe)
    }
}
