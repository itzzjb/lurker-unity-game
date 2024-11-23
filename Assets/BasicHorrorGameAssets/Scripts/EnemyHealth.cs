using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100; // Maximum health of the enemy.
    private int currentHealth; // Current health of the enemy.

    // Called when the enemy is initialized.
    private void Start()
    {
        // Set the initial health to the maximum health.
        currentHealth = maxHealth;
    }

    // Function to apply damage to the enemy.
    public void TakeDamage(int damageAmount)
    {
        // Reduce the current health by the damage amount.
        currentHealth -= damageAmount;

        // Check if the enemy's health has reached zero or below.
        if (currentHealth <= 0)
        {
            // Call the function to handle the enemy's death.
            Die();
        }
    }

    // Function to handle the enemy's death.
    private void Die()
    {
        // Perform any death-related actions here, such as playing death animations, spawning effects, or removing the enemy from the scene.
        // You can customize this method based on your game's requirements.

        // For example, you might destroy the enemy GameObject:
        gameObject.GetComponent<Animator>().SetBool("Death", true); // Trigger the death animation.
    }
}