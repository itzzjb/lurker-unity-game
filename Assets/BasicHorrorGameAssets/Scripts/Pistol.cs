using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    public int maxAmmoInMag = 10;       // Maximum ammo capacity in the magazine
    public int maxAmmoInStorage = 30;   // Maximum ammo capacity in the storage
    public float shootCooldown = 0.5f;  // Cooldown time between shots
    public float reloadCooldown = 0.5f; // Cooldown time for reloading
    private float switchCooldown = 0.5f; // Cooldown time for switching actions
    public float shootRange = 100f;     // Range of the raycast

    public ParticleSystem impactEffect; // Particle effect for impact

    public int currentAmmoInMag;        // Current ammo in the magazine
    public int currentAmmoInStorage;    // Current ammo in the storage
    public int damager;                 // Damage value for the pistol
    public bool canShoot = true;        // Flag to check if shooting is allowed
    public bool canSwitch = true;       // Flag to check if switching actions is allowed
    private bool isReloading = false;   // Flag to check if reloading is in progress
    private float shootTimer;           // Timer for shoot cooldown

    public Transform cartridgeEjectionPoint; // Ejection point of the cartridge
    public GameObject cartridgePrefab;       // Prefab of the cartridge
    public float cartridgeEjectionForce = 5f; // Force applied to the cartridge

    public Animator gun;                // Animator for the gun
    public ParticleSystem muzzleFlash;  // Particle system for muzzle flash
    public GameObject muzzleFlashLight; // Light for muzzle flash
    public AudioSource shoot;           // Audio source for shooting sound

    void Start()
    {
        // Initialize ammo counts and set initial states
        currentAmmoInMag = maxAmmoInMag;
        currentAmmoInStorage = maxAmmoInStorage;
        canSwitch = true;
        muzzleFlashLight.SetActive(false);
    }

    void Update()
    {
        // Clamp ammo counts to their maximum values
        currentAmmoInMag = Mathf.Clamp(currentAmmoInMag, 0, maxAmmoInMag);
        currentAmmoInStorage = Mathf.Clamp(currentAmmoInStorage, 0, maxAmmoInStorage);

        // Check for shoot input
        if (Input.GetButtonDown("Fire1") && canShoot && !isReloading)
        {
            switchCooldown = shootCooldown;
            Shoot();
        }

        // Check for reload input
        if (Input.GetKeyDown(KeyCode.R))
        {
            switchCooldown = reloadCooldown;
            Reload();
        }

        // Update the shoot timer
        if (shootTimer > 0f)
        {
            shootTimer -= Time.deltaTime;
        }
    }

    void Shoot()
    {
        // Check if there is ammo in the magazine and shoot cooldown has elapsed
        if (currentAmmoInMag > 0 && shootTimer <= 0f)
        {
            canSwitch = false;
            shoot.Play(); // Play shooting sound
            muzzleFlash.Play(); // Play muzzle flash effect
            muzzleFlashLight.SetActive(true); // Enable muzzle flash light
            gun.SetBool("shoot", true); // Trigger shoot animation

            // Perform the shoot action using a raycast
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootRange))
            {
                // Check if the hit object has the "Enemy" tag
                if (hit.collider.CompareTag("Enemy"))
                {
                    // Get the EnemyHealth component from the hit object
                    EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();

                    // Check if the enemy has the EnemyHealth component
                    if (enemyHealth != null)
                    {
                        // Apply damage to the enemy
                        enemyHealth.TakeDamage(damager);
                    }
                }

                // Instantiate impact effect at the hit point
                Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }

            // Instantiate the empty cartridge
            GameObject cartridge = Instantiate(cartridgePrefab, cartridgeEjectionPoint.position, cartridgeEjectionPoint.rotation);
            Rigidbody cartridgeRigidbody = cartridge.GetComponent<Rigidbody>();

            // Apply force to eject the cartridge
            cartridgeRigidbody.AddForce(cartridgeEjectionPoint.right * cartridgeEjectionForce, ForceMode.Impulse);

            // Start coroutines to handle end of animations and cooldowns
            StartCoroutine(endAnimations());
            StartCoroutine(endLight());
            StartCoroutine(canswitchshoot());

            // Reduce ammo count
            currentAmmoInMag--;

            // Start the shoot cooldown
            shootTimer = shootCooldown;
        }
        else
        {
            // Out of ammo in the magazine or shoot on cooldown
            Debug.Log("Cannot shoot");
        }
    }

    void Reload()
    {
        // Check if already reloading or out of ammo in the storage
        if (isReloading || currentAmmoInStorage <= 0)
            return;

        // Calculate the number of bullets to reload
        int bulletsToReload = maxAmmoInMag - currentAmmoInMag;

        // Check if there is enough ammo in the storage for reloading
        if (bulletsToReload > 0)
        {
            gun.SetBool("reload", true); // Trigger reload animation
            StartCoroutine(endAnimations());

            // Determine the actual number of bullets to reload based on available ammo
            int bulletsAvailable = Mathf.Min(bulletsToReload, currentAmmoInStorage);

            // Update ammo counts
            currentAmmoInMag += bulletsAvailable;
            currentAmmoInStorage -= bulletsAvailable;

            Debug.Log("Reloaded " + bulletsAvailable + " bullets");

            // Start the reload cooldown
            StartCoroutine(ReloadCooldown());
        }
        else
        {
            Debug.Log("Cannot reload");
        }
    }

    IEnumerator ReloadCooldown()
    {
        isReloading = true; // Set reloading flag
        canShoot = false;   // Disable shooting
        canSwitch = false;  // Disable switching actions

        yield return new WaitForSeconds(reloadCooldown); // Wait for reload cooldown

        isReloading = false; // Reset reloading flag
        canShoot = true;     // Enable shooting
        canSwitch = true;    // Enable switching actions
    }

    IEnumerator endAnimations()
    {
        yield return new WaitForSeconds(.1f); // Wait for a short duration
        gun.SetBool("shoot", false); // Reset shoot animation flag
        gun.SetBool("reload", false); // Reset reload animation flag
    }

    IEnumerator endLight()
    {
        yield return new WaitForSeconds(.1f); // Wait for a short duration
        muzzleFlashLight.SetActive(false); // Disable muzzle flash light
    }

    IEnumerator canswitchshoot()
    {
        yield return new WaitForSeconds(shootCooldown); // Wait for shoot cooldown
        canSwitch = true; // Enable switching actions
    }
}