using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseChest : MonoBehaviour
{
    private GameObject OB; // Reference to the chest object itself
    public GameObject handUI; // UI element indicating interaction
    public GameObject objToActivate; // Object to activate when the chest is used

    private bool inReach; // Flag to check if the player is in reach

    void Start()
    {
        // Store a reference to the chest object
        OB = this.gameObject;

        // Initialize UI elements and the object to be inactive at the start of the game
        handUI.SetActive(false);
        objToActivate.SetActive(false);
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
            OB.GetComponent<Animator>().SetBool("open", true); // Trigger the open animation for the chest
            OB.GetComponent<BoxCollider>().enabled = false; // Disable the chest's collider to prevent further interaction
        }
    }
}