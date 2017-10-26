using UnityEngine;

public class WinConditions : MonoBehaviour {
    
    /*
     * Checks the conditions given in the parameters to see whether or not the 
     * appropriate win conditions have been met. Returns the result in true/false format.
     */
    public bool AreWinConditionsMet (string levelID1, string levelID2, string levelID3,
        string inventoryID1, string inventoryID2, string inventoryID3) {

        bool conditionsMet = false;

        //If all three predetermined win conditions match up with the inventory slots,
        //then the win conditions have been satisfied. Any time only 1 or 2 conditions are neccessary,
        //as opposed to 3, the IDs will still be checked, but must contain an empty string.
        if (levelID1.Equals(inventoryID1) && levelID2.Equals(inventoryID2)
            && levelID3.Equals(inventoryID3))
        {
            conditionsMet = true;
            //Update and Save progress
            LevelData.highestLevelCompleted++;
            PlayerPrefs.SetInt("Highest Level Completed", LevelData.highestLevelCompleted);
        }

        return conditionsMet;
	}
}
