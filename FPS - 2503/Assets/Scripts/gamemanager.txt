using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gamemanager : MonoBehaviour
{
    public static gamemanager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] TMP_Text goalCountText;
    [SerializeField] TMP_Text timerText;
    [SerializeField] GameObject menuConsole;


    [Range(60,360)] public float targetTime;
    public Image playerHPBar;
    public Image playerShieldBar;
    public GameObject playerDamageScreen;
    public GameObject player;
    public playerController playerScript;
    public TMP_InputField consoleInputField;

    public bool isPaused;
    public bool godMode = false;

    int goalCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
        timerText.text = targetTime.ToString("F0");
        playerScript = player.GetComponent<playerController>();
    }

    // Update is called once per frame
    void Update()
    {
        TogglePause();
        ToggleConsole();
        UpdateTimer();
    }

    public void statePause()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void stateUnpause()
    {
        isPaused = !isPaused;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        menuActive.SetActive(false);
        menuActive = null;
    }

    public void updateGameGoal(int amount)
    {
        goalCount += amount;
        goalCountText.text = goalCount.ToString("F0");

        if (goalCount <= 0)
        {
            statePause();
            menuActive = menuWin;
            menuActive.SetActive(true);
        }
    }

    public void youLose()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(true);
    }

    public void ToggleGodMode(bool state)
    {
        godMode = state;

        if (godMode)
        {
            playerDamageScreen.SetActive(false);
        } else
        {
            playerDamageScreen.SetActive(true);
        }
    }

    public void TogglePause()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                statePause();
                menuActive = menuPause;
                menuActive.SetActive(true);
            } else if (menuActive == menuPause)
            {
                stateUnpause();
            }
        }
    }

    public void ToggleConsole()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            if (menuActive == null)
            {
                consoleInputField.ActivateInputField();
                statePause();
                menuActive = menuConsole;
                menuActive.SetActive(true);
            } else if (menuActive == menuConsole)
            {
                stateUnpause();
            }
        }
    }

    public void UpdateTimer()
    {
        targetTime -= Time.deltaTime;
        timerText.text = targetTime.ToString("F0"); 
        if (targetTime <= 0.0f)  //Bring up lose screen when player runs out of time
        {
            youLose();
        }
    }
}
