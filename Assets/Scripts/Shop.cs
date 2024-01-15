using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public float maxDistance = 2f; // Distance from where you can interact with the shop keeper
    public GameObject interactSign; // The sign that will apear if you're close enough
    public GameObject shopKeeperTextUI; // UI element that will be visible if you interact with the shop keeper
    public GameObject shopUI; // Shop UI element

    // Item icons
    public Sprite[] headsForUI;
    public Sprite[] bodysForUI;
    public Sprite[] legssForUI;

    // Images that display the players look (the big player picture in the game)
    public Image headPlayerLook;
    public Image bodyPlayerLook;
    public Image legsPlayerLook;

    // Images that display the items that the player is currently wearing (on the big players left)
    public Image selectedHead;
    public Image selectedBody;
    public Image selectedLegs;

    public AudioClip selectSound; // Sound of selecting
    public AudioClip purchaseSound; // Sound of purchasing

    private AudioSource audioSource;
    private GameObject player; // Player's game object
    private bool closeEnough; // True if the player is close enough

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Set the audio source

        player = GameObject.FindGameObjectWithTag("Player"); // Sets the player game object
        interactSign.SetActive(false);
        shopKeeperTextUI.SetActive(false);
        shopUI.SetActive(false);
    }

    void Update()
    {
        // Setting apropriate sprites to the images based on the player's current skin
        headPlayerLook.sprite = headsForUI[Player.headSkin - 1];
        bodyPlayerLook.sprite = bodysForUI[Player.bodySkin - 1];
        legsPlayerLook.sprite = legssForUI[Player.legsSkin - 1];

        selectedHead.sprite = headsForUI[Player.headSkin - 1];
        selectedBody.sprite = bodysForUI[Player.bodySkin - 1];
        selectedLegs.sprite = legssForUI[Player.legsSkin - 1];

        interactSign.SetActive(closeEnough); // Sign to interact will be visible only if you are close enough

        // Checking if the player is close enough to the shop keeper
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance <= maxDistance)
            {
                closeEnough = true;
            }
            else
            {
                closeEnough = false;
            }
        }

        // If the player is interacting with the shop keeper and certain key is pressed, go to shop
        if (shopKeeperTextUI.activeSelf && (Input.GetButtonDown("Fire1") || Input.GetKeyDown("q")))
        {
            GoToShop();
        }

        // If the player is close enough and certain key is pressed, start interacting
        if (closeEnough && !shopUI.activeSelf && (Input.GetButtonDown("Fire1") || Input.GetKeyDown("q")))
        {
            StartConversation();
        }

        // If the certain key is pressed stop interacting and get out of the shop
        if (Input.GetButtonDown("Fire2") || Input.GetKeyDown("escape") || Input.GetKeyDown("e"))
        {
            GoBackToTheWorld();
        }
    }

    public void StartConversation()
    {
        if (!Player.inShop)
        {
            audioSource.PlayOneShot(selectSound); // Play select sound

            shopKeeperTextUI.SetActive(true);
        }
    }

    public void GoToShop()
    {
        audioSource.PlayOneShot(selectSound); // Play select sound

        shopKeeperTextUI.SetActive(false);
        Player.inShop = true;
        shopUI.SetActive(true);
    }

    public void GoBackToTheWorld()
    {
        shopKeeperTextUI.SetActive(false);
        Player.inShop = false;
        shopUI.SetActive(false);
    }

    // Logic for buying and selling the items
    // You can purchase something only if you don't already have it, and have enough coins
    public void BuyAnItem(int index)
    {
        if (!Player.ownershipList.Contains(index))
        {
            audioSource.PlayOneShot(purchaseSound);
            Player.ownershipList.Add(index);
            Player.coins -= 100;
        }
    }

    // You can sell something only if you already have it
    public void SellAnItem(int index)
    {
        if (Player.ownershipList.Contains(index))
        {
            audioSource.PlayOneShot(purchaseSound);
            Player.ownershipList.Remove(index);
            Player.coins += 100;
        }
    }
}
