using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuStuff : MonoBehaviour
{
    public string nextSceneName; // Name of the next scene to load

    // Method to load the specified scene
    public void B_LoadScene()
    {
        // Load the scene with the name stored in nextSceneName
        SceneManager.LoadScene(nextSceneName);
    }

    // Method to quit the game
    public void B_QuitGame()
    {
        // Quit the application
        Application.Quit();
    }
}