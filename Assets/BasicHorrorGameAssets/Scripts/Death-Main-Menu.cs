using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Method to load the game scene
    // This method is called when the player clicks the "Load Game" button in the main menu
    // It loads the scene named "Game" using the SceneManager
    public void B_LoadScene()
    {
        SceneManager.LoadScene("Game");
    }

    // Method to quit the game application
    // This method is called when the player clicks the "Quit Game" button in the main menu
    // It quits the application using Application.Quit()
    // Note: This will only work in a built application, not in the editor
    public void B_QuitGame()
    {
        Application.Quit();
    }
}