// Author: Matthew Spiller
// Updated: June 9, 2022

using UnityEngine;
using UnityEngine.SceneManagement;

// Responsible for changing menu screens, reseting the High Score, and exiting the application
public class MenuController : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Settings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void ResetHighScore()
    {
        PlayerPrefs.DeleteKey("HighScore");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
