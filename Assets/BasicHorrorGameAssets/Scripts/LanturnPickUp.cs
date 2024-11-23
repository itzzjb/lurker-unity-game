using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanturnPickUp : MonoBehaviour
{
    private GameObject OB; // Reference to the lantern object itself
    public GameObject handUI; // UI element indicating interaction
    public GameObject lanturn; // Lantern object to activate when picked up

    private bool inReach; // Flag to check if the player is in reach

    void Start()
    {
        // Store a reference to the lantern object
        OB = this.gameObject;

        // Initialize UI elements and the lantern to be inactive at the start of the game
        handUI.SetActive(false);
        lanturn.SetActive(false);
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
        }
    }

    void Update()
    {
        // Check if the player is in reach and presses the interact button
        if (inReach && Input.GetButtonDown("Interact"))
        {
            handUI.SetActive(false); // Hide the hand UI
            lanturn.SetActive(true); // Activate the lantern object
            StartCoroutine(end()); // Start the coroutine to handle the end of the interaction
        }
    }

    IEnumerator end()
    {
        // Wait for a short duration before destroying the lantern object
        yield return new WaitForSeconds(.01f);
        Destroy(OB); // Destroy the lantern object
    }
}