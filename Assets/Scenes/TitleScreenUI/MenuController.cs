<<<<<<< HEAD
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Make sure your Game Scene is added to the Build Settings
    // and its index (e.g., 1) is correct.
    public void PlayGame()
    {
        // VR games often use a specific "loading" or "game" scene
        // which will contain the VR rig and other necessary components.
        // Index 1 typically refers to the next scene after the Main Menu (Index 0).
        SceneManager.LoadScene(1);

        // OR, use the scene name if you prefer:
        // SceneManager.LoadScene("YourGameSceneName");
    }

    public void ExitGame()
    {
        // This command is the standard way to quit a built application.
        Application.Quit();

        // This conditional block is for testing *inside* the Unity Editor only.
        // It's a standard practice to include this when testing quit functionality.
        #if UNITY_EDITOR
            // This is the correct way to stop the game playing in the Editor.
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
=======
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Make sure your Game Scene is added to the Build Settings
    // and its index (e.g., 1) is correct.
    public void PlayGame()
    {
        // VR games often use a specific "loading" or "game" scene
        // which will contain the VR rig and other necessary components.
        // Index 1 typically refers to the next scene after the Main Menu (Index 0).
        SceneManager.LoadScene(1);

        // OR, use the scene name if you prefer:
        // SceneManager.LoadScene("YourGameSceneName");
    }

    public void ExitGame()
    {
        // This command is the standard way to quit a built application.
        Application.Quit();

        // This conditional block is for testing *inside* the Unity Editor only.
        // It's a standard practice to include this when testing quit functionality.
        #if UNITY_EDITOR
            // This is the correct way to stop the game playing in the Editor.
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
>>>>>>> 286a0b93b0241a5f8ba89463b1e08ed5e205bf6a
