using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public GameObject handUI; // UI element indicating interaction
    public GameObject UIText; // UI text element

    public GameObject invKey; // Key object in the inventory
    public GameObject fadeFX; // Fade effect object

    public string nextSceneName; // Name of the next scene to load

    private bool inReach; // Flag to check if the player is in reach

    void Start()
    {
        // Initialize UI elements and effects to be inactive at the start of the game
        handUI.SetActive(false);
        UIText.SetActive(false);
        invKey.SetActive(false);
        fadeFX.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player is within the interaction range
        if (other.gameObject.tag == "Reach")
        {
            inReach = true; // Set the inReach flag to true
            handUI.SetActive(true); // Show the hand UI to indicate interaction is possible
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the player has left the interaction range
        if (other.gameObject.tag == "Reach")
        {
            inReach = false; // Set the inReach flag to false
            handUI.SetActive(false); // Hide the hand UI
            UIText.SetActive(false); // Hide the text UI
        }
    }

    void Update()
    {
        // Check if the player is in reach and presses the interact button without having the key
        if (inReach && Input.GetButtonDown("Interact") && !invKey.activeInHierarchy)
        {
            handUI.SetActive(true); // Show the hand UI
            UIText.SetActive(true); // Show the text UI indicating the need for a key
        }

        // Check if the player is in reach and presses the interact button with the key
        if (inReach && Input.GetButtonDown("Interact") && invKey.activeInHierarchy)
        {
            handUI.SetActive(false); // Hide the hand UI
            UIText.SetActive(false); // Hide the text UI
            fadeFX.SetActive(true); // Show the fade effect
            StartCoroutine(ending()); // Start the coroutine to handle scene transition
        }
    }

    IEnumerator ending()
    {
        // Wait for the fade effect to complete before loading the next scene
        yield return new WaitForSeconds(.6f);
        SceneManager.LoadScene(nextSceneName); // Load the next scene specified by nextSceneName
    }
}