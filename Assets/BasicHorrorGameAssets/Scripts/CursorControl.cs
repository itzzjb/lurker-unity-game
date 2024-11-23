using UnityEngine;

public class CursorControl : MonoBehaviour
{
    private void Start()
    {
        // Make the cursor visible on the screen
        Cursor.visible = true;
        
        // Unlock the cursor so it can move freely within the game window
        // CursorLockMode.None means the cursor is not confined to the game window and can move freely
        Cursor.lockState = CursorLockMode.None;
    }
}