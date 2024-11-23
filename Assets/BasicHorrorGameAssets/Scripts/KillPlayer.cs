using UnityEngine;
using UnityEngine.SceneManagement;

public class KillPlayer : MonoBehaviour
{
    public string nextSceneName; // Name of the next scene to load
    public float delay = 0.5f; // Delay in seconds before loading the next scene
    public GameObject fadeout; // GameObject for the fadeout effect

    private bool playerInsideTrigger = false; // Flag to check if the player is inside the trigger

    // Method called when another collider enters the trigger collider attached to this GameObject
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the player
        if (other.CompareTag("Player"))
        {
            playerInsideTrigger = true; // Set the flag to true indicating the player is inside the trigger
            fadeout.SetActive(true); // Activate the fadeout effect
            Invoke("LoadNextScene", delay); // Invoke the LoadNextScene method after the specified delay
        }
    }

    // Method to load the next scene
    private void LoadNextScene()
    {
        // Check if the player is still inside the trigger
        if (playerInsideTrigger)
        {
            // Load the next scene by name
            SceneManager.LoadScene(nextSceneName);
        }
    }
}