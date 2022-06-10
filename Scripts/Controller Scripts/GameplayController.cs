// Author: Matthew Spiller
// Updated: June 9, 2022

using System.Collections;
using UnityEngine;

// The primary Controller - responsible for game logic, prompting UI updates, and prompting player movement
public class GameplayController : MonoBehaviour
{
    private GameObject player;
    private GameObject stoneInstantiator;
    private AudioSource hit1BackgroundAudio;
    private AudioSource hit2BackgroundAudio;
    private AudioSource fallingRockCorrect;
    private AudioSource fallingRockIncorrect;
    private bool answerIsLeftSide;
    private int timer = 10;
    private int score = 0;
    private UIScript ui;
    private bool responseReceived = false;
    private bool userClickedLeft;

    void Start()
    {
        player = GameObject.Find("Player");
        stoneInstantiator = GameObject.Find("StoneInstantiator");
        hit1BackgroundAudio = GameObject.Find("Hit1BackgroundAudio").GetComponent<AudioSource>();
        hit2BackgroundAudio = GameObject.Find("Hit2BackgroundAudio").GetComponent<AudioSource>();
        fallingRockCorrect = GameObject.Find("FallingRockCorrect").GetComponent<AudioSource>();
        fallingRockIncorrect = GameObject.Find("FallingRockIncorrect").GetComponent<AudioSource>();

        ui = GameObject.Find("UI Controller").GetComponent<UIScript>();
        ui.SetText(MyTags.HIGH_SCORE, "HIGH SCORE: " + PlayerPrefs.GetInt("HighScore", 0).ToString());
        ui.ResetUIComponents(timer);

        GenerateQuestion();
    }

    public void resetGame()
    {
        timer = 10;
        score = 0;
        responseReceived = false;

        player.SetActive(true);
        player.GetComponent<PlayerScript>().ResetPlayer();
        ui.ResetUIComponents(timer);

        GenerateQuestion();
    }

    // Used by PlayerScript to determine if returning to The Player's original position is necessary
    public bool isAnswerLeftSide()
    {
        return answerIsLeftSide;
    }

    // Used by PlayerScript to prevent The Player from moving from the center if they run out of time
    public bool canMove()
    {
        if (timer == 0)
            return false;
        else
            return true;
    }

    // When user clicks the left option
    public void leftClick()
    {
        responseReceived = true;
        userClickedLeft = true;
        player.GetComponent<PlayerScript>().ChooseLeftOption();
    }

    // When user clicks the right option
    public void rightClick()
    {
        responseReceived = true;
        userClickedLeft = false;
        player.GetComponent<PlayerScript>().ChooseRightOption();
    }

    // Generates the question to be answered by the user
    private void GenerateQuestion()
    {
        // Create 2 random integers between 0 and 50
        int questionNumber1 = Random.Range(0, 51);
        int questionNumber2 = Random.Range(0, 51);
        int answer;

        // 50% chance of addition
        if (Random.Range(0, 2) == 0)
        {
            ui.SetText(MyTags.QUESTION, questionNumber1.ToString() + " + " + questionNumber2.ToString());
            answer = questionNumber1 + questionNumber2;
        }

        // 50% chance of subtraction
        else
        {
            // Prevents negative answers when the question is subtraction
            if (questionNumber2 > questionNumber1)
            {
                int temp = questionNumber2;
                questionNumber2 = questionNumber1;
                questionNumber1 = temp;
            }

            ui.SetText(MyTags.QUESTION, questionNumber1.ToString() + " - " + questionNumber2.ToString());
            answer = questionNumber1 - questionNumber2;
        }

        // Generate incorrect answer within 3 integers of the correct answer
        int answerOffset = Random.Range(-3, 4);
        if (answerOffset == 0)
        {
            answerOffset = 1;
        }
        int incorrectAnswer = answer + answerOffset;

        // If the incorrect answer happens to be negative, make it 0 or 1 instead
        if (incorrectAnswer < 0)
        {
            if (answer != 0)
            {
                incorrectAnswer = 0;
            }
            else
            {
                incorrectAnswer = 1;
            } 
        }

        // 50% chance that the answer is on the left side
        if (Random.Range(0, 2) == 0)
        {
            answerIsLeftSide = true;
            ui.SetText(MyTags.OPTION1, answer.ToString());
            ui.SetText(MyTags.OPTION2, incorrectAnswer.ToString());
        }

        // 50% chance that the answer is on the left side
        else
        {
            answerIsLeftSide = false;
            ui.SetText(MyTags.OPTION1, incorrectAnswer.ToString());
            ui.SetText(MyTags.OPTION2, answer.ToString());
        }

        StartCoroutine("WaitForResponse");
    }

    // Waits for user response or until the timer runs out
    IEnumerator WaitForResponse()
    {
        yield return new WaitForSeconds(1f);

        // End the coroutine if The Player has picked an answer
        if (responseReceived)
        {
            Invoke("OutcomeOfResponse", 1.5f);
            yield break;
        }

        // Otherwise
        else
        {
            if(timer > 0)
            {
                // Play one of these sounds effects each second 
                if (timer % 2 == 0)
                {
                    hit1BackgroundAudio.Play(); 
                }
                else
                {
                    hit2BackgroundAudio.Play();
                }

                // Reduce the timer
                timer--;
                ui.SetText(MyTags.TIMER, "TIMER: " + timer.ToString());
            }

            // The player has run out of time
            else
            {
                // Prevents the user from picking a side after time has run out
                ui.DisableButtons();

                // Play falling rock sound effect
                fallingRockIncorrect.Play();

                // Present the players death animation
                player.GetComponent<Animator>().Play("PlayerDead");

                // Drop a stone in the center (because the player hasn't moved)
                stoneInstantiator.GetComponent<StoneInstantiatorScript>().InstantiateCenter();

                Invoke("GameOver", 1.15f);
                yield break;
            }
        }
        StartCoroutine("WaitForResponse");
    }

    // Determines if player gains a point or loses the game
    void OutcomeOfResponse()
    {
        // Shows correct answer in green and incorrect in red
        ui.ShowAnswer(answerIsLeftSide);

        if (answerIsLeftSide)
        {
            // If answer is on the left side, drop a stone on the right side
            stoneInstantiator.GetComponent<StoneInstantiatorScript>().InstantiateRight();

            if (userClickedLeft)
            {
                fallingRockCorrect.Play();

                score++;
                ui.SetText(MyTags.SCORE, "SCORE: " + score.ToString());

                // Increase the high score
                if (score > PlayerPrefs.GetInt("HighScore", 0))
                {
                    PlayerPrefs.SetInt("HighScore", score);
                    ui.SetText(MyTags.HIGH_SCORE, "HIGH SCORE: " + score.ToString());
                }

                Invoke("NextQuestion", 3f);
            }
            else
            {
                fallingRockIncorrect.Play();
                player.GetComponent<Animator>().Play("PlayerDead");
                Invoke("GameOver", 1.15f);
            }
        }
        else
        {
            // If answer is on the right side, drop a stone on the left side
            stoneInstantiator.GetComponent<StoneInstantiatorScript>().InstantiateLeft();

            if (!userClickedLeft)
            {
                fallingRockCorrect.Play();

                score++;
                ui.SetText(MyTags.SCORE, "SCORE: " + score.ToString());

                // Increase the high score
                if (score > PlayerPrefs.GetInt("HighScore", 0))
                {
                    PlayerPrefs.SetInt("HighScore", score);
                    ui.SetText(MyTags.HIGH_SCORE, "HIGH SCORE: " + score.ToString());
                }

                Invoke("NextQuestion", 3f);
            }
            else
            {
                fallingRockIncorrect.Play();
                player.GetComponent<Animator>().Play("PlayerDead");
                Invoke("GameOver", 1.15f);
            }
        }
    }

    // Called when the response to the previous question was correct.
    void NextQuestion()
    {
        // Reset timer
        timer = 10;
        ui.SetText(MyTags.TIMER, "TIMER: " + timer);

        // reset response boolean
        responseReceived = false;
        
        // get UI ready for the next question
        ui.NextQuesiton();

        // Generate the next question
        GenerateQuestion();
    }

    // Presents the GAME OVER screen on the UI
    void GameOver()
    {
        ui.GameOver();
    }
}
