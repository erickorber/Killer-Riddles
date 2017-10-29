using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InventorySettings : MonoBehaviour
{
    public int numItemsInInventory = 0;
    public Sprite emptySlotSprite;
    public GameObject slot1Occupier;
    public GameObject slot2Occupier;
    public GameObject slot3Occupier;
    private Vector3 slot1OriginalPos;
    private Vector3 slot2OriginalPos;
    private Vector3 slot3OriginalPos;
    private Vector3 slot1OriginalRot;
    private Vector3 slot2OriginalRot;
    private Vector3 slot3OriginalRot;
    private GameObject deathObjectParent;
    private Animator playerAnimator;
    private Image killButtonImage;

    // Use this for initialization
    private void Start()
    {
        deathObjectParent = GameObject.FindGameObjectWithTag("Death Object List");
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        StartCoroutine(InitializeUIObjects());
    }

    private IEnumerator InitializeUIObjects()
    {
        //Initialize UI only after the camera's fade in to prevent a known
        //error from occuring.
        yield return new WaitForSeconds(CameraMovement.fadeInTime + 0.1f);

        //Finally initialize the kill buttons image object
        killButtonImage = GameObject.FindGameObjectWithTag("Kill Button").GetComponent<Image>();
    }

    //Put object into an inventory slot. Assumes slot is open.
    public void PutIntoInventory (GameObject occupier, Vector3 originalPos, Vector3 originalRot)
    {
        //To prevent additions from happening more than once
        bool fulfilled = false;

        if (numItemsInInventory == 0 && !fulfilled)
        {
            GameObject.FindGameObjectWithTag("Slot 1").GetComponent<Image>().sprite = occupier.GetComponent<Image>().sprite;
            //Set visible
            GameObject.FindGameObjectWithTag("Slot 1").GetComponent<Image>().color = new Color(255f,255f,255f,255f);

            killButtonImage.color = new Color(255f, 255f, 255f, 255f);

            slot1Occupier = occupier;
            slot1OriginalPos = originalPos;
            slot1OriginalRot = originalRot;
            numItemsInInventory = 1;
            fulfilled = true;    
        }

        if (numItemsInInventory == 1 && !fulfilled)
        {
            GameObject.FindGameObjectWithTag("Slot 2").GetComponent<Image>().sprite = occupier.GetComponent<Image>().sprite;
            //Set visible
            GameObject.FindGameObjectWithTag("Slot 2").GetComponent<Image>().color = new Color(255f, 255f, 255f, 255f);

            killButtonImage.color = new Color(255f, 255f, 255f, 255f);

            slot2Occupier = occupier;
            slot2OriginalPos = originalPos;
            slot2OriginalRot = originalRot;
            numItemsInInventory = 2;
            fulfilled = true;
        }

        if (numItemsInInventory == 2 && !fulfilled)
        {
            GameObject.FindGameObjectWithTag("Slot 3").GetComponent<Image>().sprite = occupier.GetComponent<Image>().sprite;
            //Set visible
            GameObject.FindGameObjectWithTag("Slot 3").GetComponent<Image>().color = new Color(255f, 255f, 255f, 255f);

            killButtonImage.color = new Color(255f, 255f, 255f, 255f);

            slot3Occupier = occupier;
            slot3OriginalPos = originalPos;
            slot3OriginalRot = originalRot;
            numItemsInInventory = 3;
            fulfilled = true;
        }
    }

    public void RemoveFromInventory(int slot)
    {
        //To prevent removals from happening more than once
        bool fulfilled = false;

        //If removing the only item in inventory, just remove the image 
        //and reset the position for that death object. Decrease the integer tracking the amount of inventory
        if (numItemsInInventory == 1 && !fulfilled)
        {
            if (slot == 1)
            {
                //If the death object happens to be 2 Handed
                if (slot1Occupier.GetComponent<DeathObject>().is2Handed)
                {
                    playerAnimator.SetBool("bothHands", false);
                }
                slot1Occupier.transform.SetParent(deathObjectParent.transform);
                slot1Occupier.transform.position = slot1OriginalPos;
                slot1Occupier.transform.eulerAngles = slot1OriginalRot;
                slot1Occupier.layer = LayerMask.NameToLayer("Game World");
                GameObject.FindGameObjectWithTag("Slot 1").GetComponent<Image>().sprite = null;
                //Set invisible
                GameObject.FindGameObjectWithTag("Slot 1").GetComponent<Image>().color = new Color(255f, 255f, 255f, 0f);

                killButtonImage.color = new Color(255f, 255f, 255f, 0f);

                slot1Occupier = null;
                numItemsInInventory = 0;
                fulfilled = true;
            }
        }

        //If removing the second item in inventory, just remove the image, slide items down if necessary,
        //and reset the position for that death object. Decrease the integer tracking the amount of inventory
        if (numItemsInInventory == 2 && !fulfilled)
        {
            //Put stuff from slot 2 into slot 1 (sliding things down) and make slot 2 empty
            if (slot == 1)
            {
                //If the death object happens to be 2 Handed
                if (slot1Occupier.GetComponent<DeathObject>().is2Handed)
                {
                    playerAnimator.SetBool("bothHands", false);
                }
                slot1Occupier.transform.SetParent(deathObjectParent.transform);
                slot1Occupier.transform.eulerAngles = slot1OriginalRot;
                slot1Occupier.transform.position = slot1OriginalPos;
                slot1Occupier.layer = LayerMask.NameToLayer("Game World");
                GameObject.FindGameObjectWithTag("Slot 1").GetComponent<Image>().sprite = GameObject.FindGameObjectWithTag("Slot 2").GetComponent<Image>().sprite;
                GameObject.FindGameObjectWithTag("Slot 2").GetComponent<Image>().sprite = null;
                //Set invisible
                GameObject.FindGameObjectWithTag("Slot 2").GetComponent<Image>().color = new Color(255f, 255f, 255f, 0f);

                killButtonImage.color = new Color(255f, 255f, 255f, 0f);

                //Slot 2 becomes Slot 1
                slot1Occupier = slot2Occupier;
                slot1OriginalRot = slot2OriginalRot;
                slot1OriginalPos = slot2OriginalPos;

                slot2Occupier = null;
                numItemsInInventory = 1;
                fulfilled = true;
            }

            //Empty out slot 2
            if (slot == 2 && !fulfilled)
            {
                //If the death object happens to be 2 Handed
                if (slot2Occupier.GetComponent<DeathObject>().is2Handed)
                {
                    playerAnimator.SetBool("bothHands", false);
                }
                slot2Occupier.transform.SetParent(deathObjectParent.transform);
                slot2Occupier.transform.eulerAngles = slot2OriginalRot;
                slot2Occupier.transform.position = slot2OriginalPos;
                slot2Occupier.layer = LayerMask.NameToLayer("Game World");
                GameObject.FindGameObjectWithTag("Slot 2").GetComponent<Image>().sprite = null;
                //Set invisible
                GameObject.FindGameObjectWithTag("Slot 2").GetComponent<Image>().color = new Color(255f, 255f, 255f, 0f);

                killButtonImage.color = new Color(255f, 255f, 255f, 0f);

                slot2Occupier = null;
                numItemsInInventory = 1;
                fulfilled = true;
            }
        }

        //If removing the third item in inventory, just remove the image, slide items down if necessary,
        //and reset the position for that death object. Decrease the integer tracking the amount of inventory
        if (numItemsInInventory == 3 && !fulfilled)
        {
            //Put stuff from slot 2 into slot 1, and from slot 3 to slot 2 (sliding things down) and make slot 3 empty
            if (slot == 1)
            {
                //If the death object happens to be 2 Handed
                if (slot1Occupier.GetComponent<DeathObject>().is2Handed)
                {
                    playerAnimator.SetBool("bothHands", false);
                }
                slot1Occupier.transform.SetParent(deathObjectParent.transform);
                slot1Occupier.transform.eulerAngles = slot1OriginalRot;
                slot1Occupier.transform.position = slot1OriginalPos;
                slot1Occupier.layer = LayerMask.NameToLayer("Game World");
                GameObject.FindGameObjectWithTag("Slot 1").GetComponent<Image>().sprite = GameObject.FindGameObjectWithTag("Slot 2").GetComponent<Image>().sprite;
                GameObject.FindGameObjectWithTag("Slot 2").GetComponent<Image>().sprite = GameObject.FindGameObjectWithTag("Slot 3").GetComponent<Image>().sprite;
                GameObject.FindGameObjectWithTag("Slot 3").GetComponent<Image>().sprite = null;
                //Set invisible
                GameObject.FindGameObjectWithTag("Slot 3").GetComponent<Image>().color = new Color(255f, 255f, 255f, 0f);

                killButtonImage.color = new Color(255f, 255f, 255f, 0f);

                //Slot 2 becomes Slot 1
                slot1Occupier = slot2Occupier;
                slot1OriginalRot = slot2OriginalRot;
                slot1OriginalPos = slot2OriginalPos;

                //Slot 3 becomes Slot 2
                slot2Occupier = slot3Occupier;
                slot2OriginalRot = slot3OriginalRot;
                slot2OriginalPos = slot3OriginalPos;

                slot3Occupier = null;
                numItemsInInventory = 2;
                fulfilled = true;
            }

            //Put stuff from slot 3 into slot 2 (sliding things down) and make slot 3 empty
            if (slot == 2 && !fulfilled)
            {
                //If the death object happens to be 2 Handed
                if (slot2Occupier.GetComponent<DeathObject>().is2Handed)
                {
                    playerAnimator.SetBool("bothHands", false);
                }
                slot2Occupier.transform.SetParent(deathObjectParent.transform);
                slot2Occupier.transform.eulerAngles = slot2OriginalRot;
                slot2Occupier.transform.position = slot2OriginalPos;
                slot2Occupier.layer = LayerMask.NameToLayer("Game World");
                GameObject.FindGameObjectWithTag("Slot 2").GetComponent<Image>().sprite = GameObject.FindGameObjectWithTag("Slot 3").GetComponent<Image>().sprite;
                GameObject.FindGameObjectWithTag("Slot 3").GetComponent<Image>().sprite = null;
                //Set invisible
                GameObject.FindGameObjectWithTag("Slot 3").GetComponent<Image>().color = new Color(255f, 255f, 255f, 0f);

                killButtonImage.color = new Color(255f, 255f, 255f, 0f);

                //Slot 3 becomes Slot 2
                slot2Occupier = slot3Occupier;
                slot2OriginalRot = slot3OriginalRot;
                slot2OriginalPos = slot3OriginalPos;

                slot3Occupier = null;
                numItemsInInventory = 1;
                fulfilled = true;
            }

            //Empty out slot 3
            if (slot == 3 && !fulfilled)
            {
                //If the death object happens to be 2 Handed
                if (slot3Occupier.GetComponent<DeathObject>().is2Handed)
                {
                    playerAnimator.SetBool("bothHands", false);
                }
                slot3Occupier.transform.SetParent(deathObjectParent.transform);
                slot3Occupier.transform.eulerAngles = slot3OriginalRot;
                slot3Occupier.transform.position = slot3OriginalPos;
                slot3Occupier.layer = LayerMask.NameToLayer("Game World");
                GameObject.FindGameObjectWithTag("Slot 3").GetComponent<Image>().sprite = emptySlotSprite;
                //Set invisible
                GameObject.FindGameObjectWithTag("Slot 3").GetComponent<Image>().color = new Color(255f, 255f, 255f, 0f);

                killButtonImage.color = new Color(255f, 255f, 255f, 0f);

                slot3Occupier = null;
                numItemsInInventory = 2;
                fulfilled = true;
            }
        }
    }
    
}
