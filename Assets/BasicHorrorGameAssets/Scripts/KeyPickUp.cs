using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickUp : MonoBehaviour
{
    public GameObject handUI; // UI element indicating interaction
    public GameObject objToActivate; // Object to activate when the key is picked up

    private GameObject ob; // Reference to the key object itself

    private bool inReach; // Flag to check if the player is in reach

    void Start()
    {
        // Initialize UI elements and the object to be inactive at the start of the game
        handUI.SetActive(false);
        objToActivate.SetActive(false);

        // Store a reference to the key object
        ob = this.gameObject;
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
            objToActivate.SetActive(true); // Activate the specified object
            ob.GetComponent<MeshRenderer>().enabled = false; // Hide the key object
        }
    }
}