using TMPro;
using UnityEditor;
using UnityEngine;

public class GameManager : SingletonMonoBehavior<GameManager>
{
    [SerializeField] private int score = 0;
    [SerializeField] private CoinCounterUI coinCounter;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private GameObject settingsMenu;

    private bool isSettingsMenuActive;
    // this creates a public getter for the bool
    // this way the variable is read only
    //without makingit public
    public bool IsSettingsMenuActive => isSettingsMenuActive;
    protected override void Awake()
    {
        base.Awake();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        inputManager.OnSettingsMenu.AddListener(ToggleSettingsMenu);
        // the game starts with the settings menu disabled
        DisableSettingsMenu();
    }
    public void IncreaseScore()
    {
        score++;
        coinCounter.UpdateScore(score);
    }
    private void ToggleSettingsMenu()
    {
        if (isSettingsMenuActive) DisableSettingsMenu();
        else EnableSettingsMenu();
    }
    private void EnableSettingsMenu()
    {
        Time.timeScale = 0f;
        settingsMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isSettingsMenuActive = true;
     }
    public void DisableSettingsMenu()
    {
        Time.timeScale = 1f;
        settingsMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isSettingsMenuActive = false;
     }
    public void QuitGame()
    {
        EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
