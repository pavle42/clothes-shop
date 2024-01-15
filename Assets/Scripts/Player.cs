using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public static bool walk; // True if player is moving
    public static bool up; // True if player is going north
    public static bool down; // True if player is going south
    public static bool inShop; // True if player is currently in shop
    public static List<int> ownershipList = new List<int> { 1, 4, 7}; // Here are stored all the indexes from the owned skins
    public static int coins = 1000;

    // Current skins for head, body and legs
    public static int headSkin = 1;
    public static int bodySkin = 1;
    public static int legsSkin = 1;

    public float speed; // How fast the player moves through the world
    public GameObject playerInventory; // UI game object of players inventory
    public GameObject quitUI; // UI for quitting
    public TMP_Text coinsText; // Text that displays how much coins do you have
    public AudioClip selectSound; // Sound of selecting

    private AudioSource audioSource;
    private float horizontalInput;
    private float verticalInput; 
    private bool right; // True if the player is facing right
    private bool inventoryOpen; // True if the player has the inventory open
    private bool quitting; // True if quitting UI is active

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Set the audio source

        quitUI.SetActive(false);
        playerInventory.SetActive(false);

        right = true;
        down = true;
    }

    void Update()
    {
        coinsText.text = coins.ToString(); // Setting the coins text value

        // Getting horizontal and vertical input
        horizontalInput = Input.GetAxis("Horizontal"); 
        verticalInput = Input.GetAxis("Vertical");

        // Oppening and closing player inventory
        if (Input.GetButtonDown("Fire3") || Input.GetKeyDown("r"))
        {
            audioSource.PlayOneShot(selectSound); // Play select sound

            if (inventoryOpen)
            {
                playerInventory.SetActive(false);
                inventoryOpen = false;
                inShop = false;
            }
            else
            {
                playerInventory.SetActive(true);
                inventoryOpen = true;
                inShop = true;
            }
        }

        // Activating quit UI
        if (!inShop && Input.GetKeyDown("escape"))
        {
            quitUI.SetActive(true);
            quitting = true;
        }

        // If the quitting UI is activated quit if "q" is pressed, or close quit UI if "e" is pressed
        if (quitting)
        {
            if (Input.GetKeyDown("q"))
            {
                Application.Quit();
            }
            else if (Input.GetKeyDown("e"))
            {
                quitting = false;
                quitUI.SetActive(false);
            }
        }
    }

    void FixedUpdate()
    {
        if (!inShop) // Checks if the player is currently in shop and allown moving only if the player is NOT in the shop
        {
            if (horizontalInput > 0)
            {
                walk = true;
                up = false;
                down = false;

                // Rotate if the player is not facing right
                if (!right)
                {
                    transform.Rotate(0, 180, 0);
                    right = true;
                }

                // Check if there is any vertical input at the same time so the player can move on 45 degree angle
                if (verticalInput > 0)
                {
                    // The speed is devided by square root of 2 so that it has the same speed as if it was going straight in the one direction
                    // If this wasn't done, the player would be moving faster when going in 45 degree angle
                    transform.position = new Vector3(transform.position.x + (speed / Mathf.Sqrt(2f)), transform.position.y + (speed / Mathf.Sqrt(2f)), transform.position.z); 
                }
                else if (verticalInput < 0)
                {
                    // The speed is devided by square root of 2 so that it has the same speed as if it was going straight in the one direction
                    // If this wasn't done, the player would be moving faster when going in 45 degree angle
                    transform.position = new Vector3(transform.position.x + (speed / Mathf.Sqrt(2f)), transform.position.y - (speed / Mathf.Sqrt(2f)), transform.position.z);
                }
                else
                {
                    // If there's not any vertical input, the player just goes right
                    transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);
                }
            }
            else if (horizontalInput < 0)
            {
                walk = true;
                up = false;
                down = false;

                // Rotate if the player is facing right
                if (right)
                {
                    transform.Rotate(0, 180, 0);
                    right = false;
                }

                // Check if there is any vertical input at the same time so the player can move on 45 degree angle
                if (verticalInput > 0)
                {
                    // The speed is devided by square root of 2 so that it has the same speed as if it was going straight in the one direction
                    // If this wasn't done, the player would be moving faster when going in 45 degree angle
                    transform.position = new Vector3(transform.position.x - (speed / Mathf.Sqrt(2f)), transform.position.y + (speed / Mathf.Sqrt(2f)), transform.position.z);
                }
                else if (verticalInput < 0)
                {
                    // The speed is devided by square root of 2 so that it has the same speed as if it was going straight in the one direction
                    // If this wasn't done, the player would be moving faster when going in 45 degree angle
                    transform.position = new Vector3(transform.position.x - (speed / Mathf.Sqrt(2f)), transform.position.y - (speed / Mathf.Sqrt(2f)), transform.position.z);
                }
                else
                {
                    // If there's not any vertical input, the player just goes left
                    transform.position = new Vector3(transform.position.x - speed, transform.position.y, transform.position.z);
                }
            }
            else if (verticalInput > 0)
            {
                walk = true;
                up = true;
                down = false;

                // Moving north
                transform.position = new Vector3(transform.position.x, transform.position.y + speed, transform.position.z);
            }
            else if (verticalInput < 0)
            {
                walk = true;
                up = false;
                down = true;

                // Moving south
                transform.position = new Vector3(transform.position.x, transform.position.y - speed, transform.position.z);
            }
            else
            {
                walk = false;
            }
        }
    }

    // The functions below are for buttons in the inventory UI. They take an int that represents skin index, and if the player owns a skin with that index, that skin is now on the player 
    public void SetHead(int index)
    {
        if (ownershipList.Contains(index))
        {
            audioSource.PlayOneShot(selectSound);
            headSkin = index;
        }
    }

    public void SetBody(int index)
    {
        if (ownershipList.Contains(index + 3))
        {
            audioSource.PlayOneShot(selectSound);
            bodySkin = index;
        }
    }

    public void SetLegs(int index)
    {
        if (ownershipList.Contains(index + 6))
        {
            audioSource.PlayOneShot(selectSound);
            legsSkin = index;
        }
    }
}
