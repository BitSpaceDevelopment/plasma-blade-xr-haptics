using System.Collections;
using TMPro;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [Header("Buttons")]
    public GameObject StartMenu;
    public GameObject GameOverMenu;
    public GameObject BossTutorialMenu;
    public GameObject pauseUIButton;
    public GameObject CreditsMenu;
    public bool uiDelay = false;

    [Header("Player")]
    public GameObject playerUI;


    [Header("Text")]
    public TextMeshProUGUI droidDestroyText;
    public TextMeshProUGUI timeText;

    [Header("Debug")]
    public bool pressStart = false;
    public bool pressRestart = false;
    public bool pressBackToMenu = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void FixedUpdate()
    {
        if (pressStart)
        {
            Play();
            pressStart = false;
        }
        if(pressRestart)
        {
            Restart();
            pressRestart = false;
        }
        if (pressBackToMenu)
        {
            BackToMenu();
            pressBackToMenu = false;
        }
    }

    #region GameOverStats
    public void GameoverScreen()
    {
        GameOverMenu.SetActive(true);
        droidDestroyText.text = "Droid's Destroyed: " + DroidController.Instance.DroidsKilledTotal;
        timeText.text = "Time: " + FormatTime(GameController.Instance.GameTime);
    }

    string FormatTime(float timeInSeconds)
    {
        int hours = (int)(timeInSeconds / 3600);
        int minutes = (int)((timeInSeconds % 3600) / 60);
        int seconds = (int)(timeInSeconds % 60);

        return string.Format("{0:D2}h:{1:D2}m:{2:D2}s", hours, minutes, seconds);
    }
    #endregion

    #region MenuButtons
    public void Play()
    {
        StartMenu.SetActive(false);
        playerUI.SetActive(true);
        GameController.Instance.StartGame();
    }

    public void Restart()
    {
        GameOverMenu.SetActive(false);
        GameController.Instance.ResetGame();
        GameController.Instance.StartGame();
    }

    public void BackToMenu()
    {
        GameOverMenu.SetActive(false);
        CreditsMenu.SetActive(false);
        playerUI.SetActive(false);
        StartMenu.SetActive(true);
        GameController.Instance.ResetGame();
        DelayUIPress();
    }

    public void ShowBossMenu()
    {
        BossTutorialMenu.SetActive(true);
        GameController.Instance.PauseGame();
    }

    public void CloseBossMenu()
    {
        BossTutorialMenu.SetActive(false);
        GameController.Instance.ResetGameSpeed();
    }

    public void SwitchButton(bool isActive)
    {
        pauseUIButton.SetActive(isActive);
    }

    public void Quit()
    {
        Application.Quit();
    }
    #endregion

    #region UIInputDelay
    public void DelayUIPress()
    {
        StartCoroutine(DelayUI());
    }

    IEnumerator DelayUI()
    {
        uiDelay = true;
        yield return new WaitForSeconds(1f);
        uiDelay = false;
    }
    #endregion
}
