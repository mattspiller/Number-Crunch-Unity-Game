// Author: Matthew Spiller
// Updated: June 9, 2022

using UnityEngine;
using UnityEngine.UI;

// Manages UI assets
public class UIScript : MonoBehaviour
{
    private Text questionText;
    private Text option1Text;
    private Text option2Text;
    private Text timerText;
    private Text scoreText;
    private Text highScoreText;
    private GameObject divider;
    private Vector3 questionTextOriginalPosition;
    private GameObject runningMainMenuButton;
    private GameObject replayButton;
    private GameObject mainMenuButton;
    private GameObject option1Button;
    private GameObject option2Button;
    private GameplayController controller;
    private bool mouseEnteredRightButtonArea = false;
    private bool mouseEnteredLeftButtonArea = false;
    private bool isFading = true;

    private void Start()
    {
        questionText = GameObject.Find("Question").GetComponent<Text>();
        option1Text = GameObject.Find("Option1Text").GetComponent<Text>();
        option2Text = GameObject.Find("Option2Text").GetComponent<Text>();
        timerText = GameObject.Find("Timer").GetComponent<Text>();
        scoreText = GameObject.Find("Score").GetComponent<Text>();
        highScoreText = GameObject.Find("HighScore").GetComponent<Text>();
        divider = GameObject.Find("Divider");
        runningMainMenuButton = GameObject.Find("RunningMainMenuButton");
        replayButton = GameObject.Find("ReplayButton");
        mainMenuButton = GameObject.Find("MainMenuButton");
        option1Button = GameObject.Find("Option1Button");
        option2Button = GameObject.Find("Option2Button");
        controller = GameObject.Find("GameplayController").GetComponent<GameplayController>();
        questionTextOriginalPosition = questionText.transform.position;
    }

    private void FixedUpdate()
    {
        if (mouseEnteredRightButtonArea)
            OptionTextFade(option2Text);
        else if (mouseEnteredLeftButtonArea)
            OptionTextFade(option1Text);
    }

    // When the mouse is hovering over the right side option
    public void EnterRightButtonArea()
    {
        mouseEnteredRightButtonArea = true;
    }

    // When the mouse is hovering over the left side option
    public void EnterLeftButtonArea()
    {
        mouseEnteredLeftButtonArea = true;
    }

    // When the mouse stops hovering over the right side option
    public void ExitRightButtonArea()
    {
        mouseEnteredRightButtonArea = false;
        option2Text.color = Color.white;
    }

    // When the mouse stops hovering over the left side option
    public void ExitLeftButtonArea()
    {
        mouseEnteredLeftButtonArea = false;
        option1Text.color = Color.white;
    }

    // When the user clicks on the right side option
    public void ClickedRightButtonArea()
    {
        controller.rightClick();
        option1Button.SetActive(false);
        option2Button.SetActive(false);
        ExitRightButtonArea();
    }

    // When the user clicks on the left side option
    public void ClickedLeftButtonArea()
    {
        controller.leftClick();
        option1Button.SetActive(false);
        option2Button.SetActive(false);
        ExitLeftButtonArea();
    }

    // Prevents user for picking a side after the timer runs out
    public void DisableButtons()
    {
        option1Button.SetActive(false);
        option2Button.SetActive(false);
        ExitLeftButtonArea();
        ExitRightButtonArea();
    }

    // Allows the Controller to change UI text elements
    public void SetText (string tag, string text)
    {
        if (tag == MyTags.QUESTION)
            questionText.text = text;
        else if (tag == MyTags.OPTION1)
            option1Text.text = text;
        else if (tag == MyTags.OPTION2)
            option2Text.text = text;
        else if (tag == MyTags.TIMER)
            timerText.text = text;
        else if (tag == MyTags.SCORE)
            scoreText.text = text;
        else if (tag == MyTags.HIGH_SCORE)
            highScoreText.text = text;
    }

    // Reveals the correct and incorrect answers in green and red, respectively
    public void ShowAnswer (bool answerIsLeftSide)
    {
        if (answerIsLeftSide)
        {
            option1Text.color = Color.green;
            option2Text.color = Color.red;
        }
        else
        {
            option1Text.color = Color.red;
            option2Text.color = Color.green;
        }
    }

    // Prepares UI elements for the next question
    public void NextQuesiton ()
    { 
        // Text returns to white
        option1Text.color = Color.white;
        option2Text.color = Color.white;

        // Left and right options are reactivated
        option1Button.SetActive(true);
        option2Button.SetActive(true);
    }

    // When the mouse hovers over one option, the corresponding text for that option fades in and out
    // to let the user know that they are hovering over it
    void OptionTextFade (Text fadingText)
    {
        if (isFading)
        {
            if (fadingText.color.a >= 0.2f)
            {
                Color temp = fadingText.color;
                temp.a -= 0.02f;
                fadingText.color = temp;
            }
            else
            {
                isFading = false;
            }
        }
        else
        {
            if (fadingText.color.a < 1f)
            {
                Color temp = fadingText.color;
                temp.a += 0.02f;
                fadingText.color = temp;
            }
            else
            {
                isFading = true;
            }
        }
    }

    // GAME OVER screen
    public void GameOver ()
    {
        divider.SetActive(false);
        option1Text.gameObject.SetActive(false);
        option2Text.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        runningMainMenuButton.SetActive(false);
        replayButton.SetActive(true);
        mainMenuButton.SetActive(true);
        questionText.text = "GAME OVER";
        questionText.transform.position = new Vector3(0, 2.18f, 0);
    }

    // Reset UI components for the next game
    public void ResetUIComponents (int timer)
    {
        scoreText.text = "SCORE: 0";
        timerText.text = "TIMER: " + timer.ToString();
        option1Text.color = Color.white;
        option2Text.color = Color.white;
        questionText.transform.position = questionTextOriginalPosition;
        divider.SetActive(true);
        option1Text.gameObject.SetActive(true);
        option2Text.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);
        runningMainMenuButton.SetActive(true);
        option1Button.SetActive(true);
        option2Button.SetActive(true);
        replayButton.SetActive(false);
        mainMenuButton.SetActive(false);
    }
}
