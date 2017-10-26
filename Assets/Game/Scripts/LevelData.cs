using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelData : MonoBehaviour {

    public static int level = 1;
    public int inventorySlots;
    public string riddle;
    public Text riddleUIText;
    public string winID1;
    public string winID2;
    public string winID3;
    public float rightBound;
    public float leftBound;
    public float upperBound;
    public float lowerBound;
    public GameObject canvas;
    public CameraMovement camMovement;
    public static int highestLevelCompleted;

    private void Start ()
    {
        //Data for all levels
        if (level == 1)
        {
            inventorySlots = 1;
            riddle = "If you want to get to my level, take notes! HAHA Don't worry though, that'll never happen!";
            winID1 = "clipboard";
            rightBound = 156.5f;
            leftBound = 278.0f;
            upperBound = 0.5f;
            lowerBound = 19.0f;
        }
        
        StartCoroutine(ExecuteAfterFade());
    }

    IEnumerator ExecuteAfterFade()
    {
        yield return new WaitForSeconds(camMovement.fadeInTime);

        //Set the UI Riddle Text to the predetermined riddle
        riddleUIText.text = "" + riddle;
    }
}
