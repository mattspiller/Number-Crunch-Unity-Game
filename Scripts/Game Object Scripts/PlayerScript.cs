// Author: Matthew Spiller
// Updated: June 9, 2022

using UnityEngine;

// Responsible for moving the Player object
public class PlayerScript : MonoBehaviour
{
    private GameObject gameplayController;
    private Rigidbody2D myBody;
    private Animator anim;
    private Vector3 originalPosition;
    private float speed = 5f;
    private bool atOrigin = true;
    private bool returnToOriginInvoked = false;
    private bool hasMadeDecision = false;
    private bool choseLeftOption;
    private AudioSource walkingAudio;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        originalPosition = new Vector3(0f, -2.96f, 0f);
        gameplayController = GameObject.Find("GameplayController");
        walkingAudio = GameObject.Find("WalkingAudio").GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        MoveLeft();
        MoveRight();
        MoveToOrigin();
    }

    // Called when the game is replayed after GAME OVER 
    public void ResetPlayer()
    {
        hasMadeDecision = false;
        atOrigin = true;
        returnToOriginInvoked = false;
        transform.position = originalPosition;
    }

    // Used to change the direction the character faces
    void ChangeDirection(float direction)
    {
        Vector3 tempScale = transform.localScale;
        tempScale.x = direction;
        transform.localScale = tempScale;
    }

    // When left option is selected
    public void ChooseLeftOption()
    {
        hasMadeDecision = true;
        choseLeftOption = true;
        walkingAudio.Play();
    }

    // When right option is selected
    public void ChooseRightOption()
    {
        hasMadeDecision = true;
        choseLeftOption = false;
        walkingAudio.Play();
    }

    // Called when the player survived and needs to walk back to the center 
    public void ReturnToOrigin()
    {
        atOrigin = false;
        walkingAudio.Play();
    }

    // Moves player to the left side of the screen
    public void MoveLeft()
    {
        if(hasMadeDecision && choseLeftOption && atOrigin)
        {
            ChangeDirection(-1.4f);

            myBody.velocity = new Vector2(-speed, 0f);
            anim.SetInteger("Speed", (int)Mathf.Abs(myBody.velocity.x));

            if(transform.position.x <= -4.5f)
            {
                myBody.velocity = new Vector2(0f, 0f);
                anim.SetInteger("Speed", (int)Mathf.Abs(myBody.velocity.x));
                walkingAudio.Stop();

                if(gameplayController.GetComponent<GameplayController>().isAnswerLeftSide())
                {
                    // Prevents invoking "ReturnToOrigin" multiple times
                    if(!returnToOriginInvoked)
                    {
                        Invoke("ReturnToOrigin", 2.6f);
                        returnToOriginInvoked = true;
                    }
                }
            }
        }
    }

    // Moves player to the right side of the screen
    public void MoveRight()
    {
        if (hasMadeDecision && !choseLeftOption && atOrigin)
        {
            ChangeDirection(1.4f);

            myBody.velocity = new Vector2(speed, 0f);
            anim.SetInteger("Speed", (int)Mathf.Abs(myBody.velocity.x));

            if (transform.position.x >= 4.5f)
            {
                myBody.velocity = new Vector2(0f, 0f);
                anim.SetInteger("Speed", (int)Mathf.Abs(myBody.velocity.x));
                walkingAudio.Stop();

                if (!gameplayController.GetComponent<GameplayController>().isAnswerLeftSide())
                {
                    // Prevents invoking "ReturnToOrigin" multiple times
                    if (!returnToOriginInvoked)
                    {
                        Invoke("ReturnToOrigin", 2.6f);
                        returnToOriginInvoked = true;
                    }
                }
            }
        }
    }

    // Moves player to the center of the screen
    void MoveToOrigin()
    {
        if (!atOrigin)
        {
            if (choseLeftOption)
            {
                ChangeDirection(1.4f);
                myBody.velocity = new Vector2(speed, myBody.velocity.y);
                anim.SetInteger("Speed", Mathf.Abs((int)myBody.velocity.x));

                if (transform.position.x >= 0f)
                {
                    myBody.velocity = new Vector2(0f, myBody.velocity.y);
                    anim.SetInteger("Speed", Mathf.Abs((int)myBody.velocity.x));
                    walkingAudio.Stop();
                    atOrigin = true;
                    hasMadeDecision = false;
                    returnToOriginInvoked = false;
                }
            }
            else
            {
                ChangeDirection(-1.4f);
                myBody.velocity = new Vector2(-speed, myBody.velocity.y);
                anim.SetInteger("Speed", Mathf.Abs((int)myBody.velocity.x));

                if (transform.position.x <= 0f)
                {
                    myBody.velocity = new Vector2(0f, myBody.velocity.y);
                    anim.SetInteger("Speed", Mathf.Abs((int)myBody.velocity.x));
                    walkingAudio.Stop();
                    atOrigin = true;
                    hasMadeDecision = false;
                    returnToOriginInvoked = false;
                }
            }
        }
    }
}
