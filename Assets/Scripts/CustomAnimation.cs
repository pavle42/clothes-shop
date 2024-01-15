using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomAnimation : MonoBehaviour
{
    public string type; // Head or Body or Legs

    // All the frames of the first color
    public Sprite[] upIdleAnimation1;
    public Sprite[] upWalkAnimation1; 
    public Sprite[] downIdleAnimation1;
    public Sprite[] downWalkAnimation1; 
    public Sprite[] sideIdleAnimation1;
    public Sprite[] sideWalkAnimation1;

    // All the frames of the second color
    public Sprite[] upIdleAnimation2;
    public Sprite[] upWalkAnimation2;
    public Sprite[] downIdleAnimation2;
    public Sprite[] downWalkAnimation2;
    public Sprite[] sideIdleAnimation2;
    public Sprite[] sideWalkAnimation2;

    // All the frames of the third color
    public Sprite[] upIdleAnimation3;
    public Sprite[] upWalkAnimation3;
    public Sprite[] downIdleAnimation3;
    public Sprite[] downWalkAnimation3;
    public Sprite[] sideIdleAnimation3;
    public Sprite[] sideWalkAnimation3;

    public AudioClip woodStepSound; // Sound of stepping on wood

    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer; // Sprite renderer of animated object

    // Arrays of the right directional frames
    private Sprite[] sideIdleAnimation;
    private Sprite[] upIdleAnimation;
    private Sprite[] downIdleAnimation;
    private Sprite[] sideWalkAnimation;
    private Sprite[] upWalkAnimation;
    private Sprite[] downWalkAnimation;

    private Sprite[] idleAnimation; // Main sprite array that will be used for idle animation
    private Sprite[] walkAnimation; // Main sprite array that will be used for walk animation
    private float animationTimer; // Timer for checking if the frame should be changed
    private int currentFrame;
    private int currentSkin;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (type == "Legs") audioSource = GetComponent<AudioSource>(); // Get the Audio Source if the type is legs

        // Setting the main arrays to side animations of the first skin
        idleAnimation = sideIdleAnimation1; 
        walkAnimation = sideWalkAnimation1;
    }

    void Update()
    {
        // Check which type of object this is and get the right current skin
        switch (type)
        {
            case "Legs":
                currentSkin = Player.legsSkin;
                break;
            case "Body":
                currentSkin = Player.bodySkin;
                break;
            case "Head":
                currentSkin = Player.headSkin;
                break;
        }

        // Set the right animation frames based on the current skin
        switch (currentSkin)
        {
            case 1:

                sideIdleAnimation = sideIdleAnimation1;
                upIdleAnimation = upIdleAnimation1;
                downIdleAnimation = downIdleAnimation1;

                sideWalkAnimation = sideWalkAnimation1;
                upWalkAnimation = upWalkAnimation1;
                downWalkAnimation = downWalkAnimation1;

                break;
            case 2:

                sideIdleAnimation = sideIdleAnimation2;
                upIdleAnimation = upIdleAnimation2;
                downIdleAnimation = downIdleAnimation2;

                sideWalkAnimation = sideWalkAnimation2;
                upWalkAnimation = upWalkAnimation2;
                downWalkAnimation = downWalkAnimation2;

                break;
            case 3:

                sideIdleAnimation = sideIdleAnimation3;
                upIdleAnimation = upIdleAnimation3;
                downIdleAnimation = downIdleAnimation3;

                sideWalkAnimation = sideWalkAnimation3;
                upWalkAnimation = upWalkAnimation3;
                downWalkAnimation = downWalkAnimation3;

                break;
            default:
                break;
        }

        // Checking what direction is the player facing so that the right frames are displayed
        if (Player.up)
        {
            idleAnimation = upIdleAnimation;
            walkAnimation = upWalkAnimation;
        }
        else if (Player.down)
        {
            idleAnimation = downIdleAnimation;
            walkAnimation = downWalkAnimation;
        }
        else
        {
            idleAnimation = sideIdleAnimation;
            walkAnimation = sideWalkAnimation;
        }

        // Changing the frames on the sprite renderer
        if (Player.walk)
        {
            // if the player is moving sprite renderer will show frames from the walking animation
            spriteRenderer.sprite = walkAnimation[currentFrame];
        }
        else
        {
            // if the current frame is greater then 5 (which can happen because walk animation has 8 frames),
            // set the current frame to 0 to avoid going out of the array bounds (idle animation has 6 frames(max index 5))
            if (currentFrame > 5)
            {
                currentFrame = 0;
            }

            // if the player is not moving sprite renderer will show frames from the idle animation
            spriteRenderer.sprite = idleAnimation[currentFrame];
        }

        // If the timer is greater than 1/15 (this number is because I found that 15 frames per second looks good) increase current frame
        if (animationTimer >= 1f / 15f)
        {
            if (currentFrame < (Player.walk ? 7 : 5))
            {
                currentFrame++;

                // Make a sound every time the frame displayed is touching the ground
                if (type == "Legs" && Player.walk)
                {
                    if (currentFrame == 3 || currentFrame == 7)
                    {
                        audioSource.PlayOneShot(woodStepSound);
                    }
                }
            }
            else
            {
                currentFrame = 0;
            }

            animationTimer = 0f;
        }
        else
        {
            animationTimer += Time.deltaTime;
        }
    }

}
