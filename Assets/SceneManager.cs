using UnityEngine;

using UnityEngine.SceneManagement;


public class MySceneManager : MonoBehaviour
{
    // Make sure your Game Scene is added to the Build Settings
    // and its index (e.g., 1) is correct.
    public void GameEnded()
    {
        // VR games often use a specific "loading" or "game" scene
        // which will contain the VR rig and other necessary components.
        // Index 1 typically refers to the next scene after the Main Menu (Index 0).
        SceneManager.LoadScene(0);

        // OR, use the scene name if you prefer:
        // SceneManager.LoadScene("YourGameSceneName");
    }
}