using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using System.IO;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuPanel;
    public TextMeshProUGUI highScoreText;
    public Button resumeButton;
    public float fadeDuration = 1f;
    private int currentHighScore;

    private bool isPaused = false;

    private const string WindowedPrefKey = "Windowed";

    public Toggle fullscreenToggle;

    private void Start()
    {
        HidePauseMenu();
        GetHighScore();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    private void TogglePauseMenu()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            ShowPauseMenu();
        }
    }

    private void ShowPauseMenu()
    {
        if (!isPaused)
        {
            isPaused = true;
            Time.timeScale = 0;
            pauseMenuPanel.SetActive(true);
            highScoreText = GameObject.Find("PauseHighScoreTMP").GetComponent<TextMeshProUGUI>();
            highScoreText.text = "LEVEL HIGHSCORE : " + currentHighScore;

        }
    }

    private void HidePauseMenu()
    {
        pauseMenuPanel.SetActive(false);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;
        HidePauseMenu();
    }
    
    public void ButtonLevelSelect()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1;
        SceneManager.LoadScene("LevelSelect");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting the game.");
        Application.Quit();
    }

    public void ReturnToMainMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void ToggleWindowedMode()
    {
        bool isWindowed = PlayerPrefs.GetInt(WindowedPrefKey, 0) == 1;
        isWindowed = !isWindowed;
        PlayerPrefs.SetInt(WindowedPrefKey, isWindowed ? 1 : 0);
        PlayerPrefs.Save();

        Screen.SetResolution(1920, 1080, !isWindowed);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void RestartLevel()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private int GetHighScore()
    {
        string levelToLoad = PlayerPrefs.GetString("LevelToLoad");
        string jsonFilePath = Path.Combine(Application.persistentDataPath, levelToLoad + ".json");

        if (File.Exists(jsonFilePath))
        {
            string jsonData = File.ReadAllText(jsonFilePath);
            LevelData levelData = JsonUtility.FromJson<LevelData>(jsonData);
            currentHighScore = levelData.HighScore;

        }
        else
        {
            Debug.LogWarning("JSON file not found for level: " + levelToLoad);
        }

        return currentHighScore;
    }
}
