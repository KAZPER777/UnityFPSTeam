using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    public void resume()
    {
        gamemanager.instance.stateUnpause();
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gamemanager.instance.stateUnpause();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    //Upgrade Menu Buttons
    public void healthUp()
    {
        if (gamemanager.instance.playerScript.XP >= 100)
        {
            gamemanager.instance.playerScript.upgrade("HP");
        }
    }
    public void dmgUp()
    {
        if (gamemanager.instance.playerScript.XP >= 100)
        {
            gamemanager.instance.playerScript.upgrade("DMG");
        }
    }
}
