using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public void RestartGame()
    {
        // Reloads the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        // Quits the application (will only work in a built game, not in the editor)
        Application.Quit();
        // For editor testing, uncomment the next line:
        // UnityEditor.EditorApplication.isPlaying = false;
    }
}