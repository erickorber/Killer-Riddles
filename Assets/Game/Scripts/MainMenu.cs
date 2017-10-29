using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    public Button playButton;
    private GameObject[] levelButtons;

    private void Start()
    {
        //Loads the highest level saved. If no save has ever been made, it will load
        //the int 0 by default, which happens to be what's needed.
        LevelData.highestLevelCompleted = PlayerPrefs.GetInt("Highest Level Completed");

        iTween.CameraFadeAdd();
        iTween.CameraFadeFrom(iTween.Hash("amount", 1, "time", 1.2f, "oncompletetarget", gameObject,
            "oncomplete", "NowPlayable"));
        levelButtons = GameObject.FindGameObjectsWithTag("Main Menu Level Button");
    }

    public void OnPlayButtonClick()
    {
        playButton.interactable = false;
        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath("Play Button To Level Select"), 
            "time", 2, "easetype", "easeInOutQuad", "delay", 0.3,"oncompletetarget", gameObject,
            "oncomplete", "MakeInteractable", "oncompleteparams", playButton));
    }

    public void OnLevelSelectButtonClick(int level)
    {
        for(int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].GetComponent<Button>().interactable = false;
        }

        iTween.CameraFadeTo(iTween.Hash("amount", 1, "time", 0.5f, "oncompletetarget", gameObject,
            "oncomplete", "LoadLevel", "oncompleteparams", level));
    }

    /*
     * When the scene starts, as the shot is fading in, the play button is non-interactable, but once the fade
     * has finished, this function is called to make the play button interactable. Just a stylistic touch.
     */
    private void NowPlayable()
    {
        playButton.interactable = true;
    }

    /*
     * When the main menu PLAY BUTTON is pressed AND the tween (moving the camera over to the level selection menu)
     * has been completed, then this function is called
     */
    private void MakeInteractable(Button button)
    {
        if(!button.IsInteractable())
        {
            button.interactable = true;
        }

        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].GetComponent<Button>().interactable = true;
        }
    }

    private void LoadLevel(int level)
    {
        LevelData.level = level;
        SceneManager.LoadScene("Office");
    }
}
