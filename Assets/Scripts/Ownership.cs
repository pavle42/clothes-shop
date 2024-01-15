using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ownership : MonoBehaviour
{
    // This script is for item boxes in shop and inventory. It will show the picture of the idem if you own it, and it won't if you don't.

    public bool isShop; // True if the item box is in the shop
    public int index; // Index of the item
    public GameObject image; // The image that should be shown or not

    void Update()
    {
        if (Player.ownershipList.Contains(index)) // Check if you own the item
        {
            image.SetActive((isShop ? false : true)); // If this item box is in the shop space and you own the item, it won't display the image, but if it is in inventory it will display it. 
        }
        else
        {
            image.SetActive((isShop ? true : false)); // If this item box is in the shop space and you don't own the item, it will display the image, but if it is in inventory it won't display it.
        }
    }
}
