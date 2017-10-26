using UnityEngine;

public class PickUpObject : MonoBehaviour {

    private Transform playerRightHand;
    public Vector3 holdPosition;
    public Vector3 holdRotation;
    private new Camera camera;
    private Vector3 cameraDirection;
    private Vector3 originalPosition;
    private Vector3 originalRotation;
    private bool mouseButtonClicked = false;
    private Animator playerAnimator;

    // Use this for initialization
    void Start () {
        playerRightHand = GameObject.FindGameObjectWithTag("Right Hand").transform;
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        cameraDirection = camera.transform.TransformDirection(Vector3.forward);
        originalPosition = transform.position;
        originalRotation = transform.eulerAngles;
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }

    void Update()
    {
        //If the mouse has been clicked, meaning the death object has been selected...
        if (Input.GetMouseButtonDown(0))
        {
            mouseButtonClicked = true;
        }

        if (gameObject.layer == LayerMask.NameToLayer("Selected Outline Objects"))
        {
            transform.localPosition = holdPosition;
            transform.localEulerAngles = holdRotation;
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
   
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        ray.direction = cameraDirection;
        RaycastHit hit;
        Debug.DrawRay(ray.origin, cameraDirection);

        //If the Raycast (coming from the mouse position) hits a collider
        if (Physics.Raycast(ray, out hit)) {

            //If the collider that has been hit is not an object already in inventory
            if (gameObject.layer != LayerMask.NameToLayer("Selected Outline Objects"))
            {
                //If the collider that has been hit has the name of THIS object
                if (hit.collider.gameObject.name == gameObject.name)
                {

                    gameObject.layer = LayerMask.NameToLayer("Hover Outline Objects");

                    //If the mouse has been clicked, meaning the death object has been selected...
                    if (mouseButtonClicked)
                    {
                        //If an inventory slot is free
                        if (GameObject.FindGameObjectWithTag("Player").GetComponent<InventorySettings>().numItemsInInventory
                            < GameObject.FindGameObjectWithTag("Data").GetComponent<LevelData>().inventorySlots)
                        {
                            //If at least one hand is free
                            if (!playerAnimator.GetBool("bothHands"))
                            {
                                //Set the animator parameter dealing with 2-handed Death Objects to its appropriate value,
                                //so the player's animation can be adjusted accordingly.
                                playerAnimator.SetBool("bothHands", gameObject.GetComponent<DeathObject>().is2Handed);

                                transform.SetParent(playerRightHand);
                                transform.localPosition = holdPosition;
                                transform.localRotation = Quaternion.Euler(holdRotation.x, holdRotation.y, holdRotation.z);
                                transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

                                //Put item into inventory slot
                                GameObject.FindGameObjectWithTag("Player").GetComponent<InventorySettings>()
                                    .PutIntoInventory(gameObject, originalPosition, originalRotation);

                                gameObject.layer = LayerMask.NameToLayer("Selected Outline Objects");
                            }
                        }
                    }
                }
                else
                {
                    gameObject.layer = LayerMask.NameToLayer("Game World");
                }

            }

        }   // else {

            /*if (Physics.Raycast(ray.origin, cameraDirection, out hit, Mathf.Infinity, layerMask))
            {
                if (hit.transform.gameObject.Equals(parentObject))
                {
                    Debug.Log("Yes hitting parent object");
                }
                else
                {
                    Debug.Log("No NOT hitting parent object");
                }
            }*/

            //if (gameObject.layer != LayerMask.NameToLayer("Selected Outline Objects"))
            //{
              //  gameObject.layer = LayerMask.NameToLayer("Game World");
            //}

       // }
        

        //To reset this boolean properly
        mouseButtonClicked = false;
    }
}
