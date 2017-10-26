using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour {
    
    private const float moveSpeed = 8.0f;
    private const float rotationSpeed = 14.0f;
    private const float maxEnemyDistance = 3.0f;

    public GameObject canvas;
    private GameObject enemy;
    private Animator enemyAnim;
    private Animator anim;
    private LevelData levelData;
    private CharacterController controller;
    private Transform oldPlayerTransform;
    private Vector3 oldPlayerPos;

    public static bool isPlayerAttacking = false;

    // Use this for initialization
    void Start()
	{
        oldPlayerPos = transform.position;
        oldPlayerTransform = transform;
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        enemyAnim = enemy.GetComponent<Animator>();
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        levelData = GameObject.FindGameObjectWithTag("Data").GetComponent<LevelData>();
    }

    // Update is called once per frame
    void Update () {

        //Right click to attack, as long as no other attack is currently taking place
        if (anim.GetInteger("Attack_Type") == 0 && Input.GetMouseButtonDown(1))
        {
            Attack();
        }

        //Always updating the isPlayerAttacking boolean, so that it can be accessed
        //accurately by other scripts.
        if (anim.GetInteger("Attack_Type") == 0)
        {
            isPlayerAttacking = false;
        }
        else
        {
            isPlayerAttacking = false;
        }
        
        //Move the player if appropriate
        MovePlayer();

        //Have the enemy follow the player's position, but on a delay.
        EnemyFollowsPlayer();
    }

    /*
     * Takes care of all player attacks
     */
    private void Attack()
    {
        if (GetDeathObjectName() == "clipboard")
        {
            //Clipboard attack
            anim.SetInteger("Attack_Type", 1);
        }

        if (GetDeathObjectName() == "computer_monitor")
        {
            //Clipboard attack
            anim.SetInteger("Attack_Type", 2);
        }
    }

    /*
     * Returns the name of the Death Object the player is holding in his right hand.
     * If the player is holding no DeathObject, return a default "none".
     */
    private string GetDeathObjectName()
    {
        string name = "none";

        if (GameObject.FindGameObjectWithTag("Right Hand").transform.childCount > 0)
        {
            name = GameObject.FindGameObjectWithTag("Right Hand").transform.GetChild(0).name;
        }

        return name;
    }

    /*
     * Takes cares of all player movement and idling
     */
    private void MovePlayer()
    {
        bool left = false;
        bool right = false;
        bool up = false;
        bool down = false;


        if (Input.GetKey(KeyCode.D) && transform.position.x > levelData.rightBound)
        {
            right = true;
        }

        if (Input.GetKey(KeyCode.A) && transform.position.x < levelData.leftBound)
        {
            left = true;
        }

        if (Input.GetKey(KeyCode.W) && transform.position.z > levelData.upperBound)
        {
            up = true;
        }

        if (Input.GetKey(KeyCode.S) && transform.position.z < levelData.lowerBound)
        {
            down = true;
        }

        //As long as there is some movement
        if (left || right || down || up)
        {
            anim.SetBool("isRunning", true);
        }
        //Idle
        else
        {
            anim.SetBool("isRunning", false);
        }

        float moveStateX = 0;
        float moveStateZ = 0;

        //If left
        if (left && !right)
        {
            moveStateX = 1f;
        }
        //If right
        else if (!left && right)
        {
            moveStateX = -1f;
        }

        //If up
        if (up && !down)
        {
            moveStateZ = -1f;
        }
        //If down
        else if (!up && down)
        {
            moveStateZ = 1f;
        }

        //Idle
        if ((left && right) || (down && up))
        {
            moveStateX = 0;
            moveStateZ = 0;
            anim.SetBool("isRunning", false);
        }
        
        float yState = 0;

        //If controller is not touching the ground, then implement gravity
        if (!controller.isGrounded)
        {
            yState = Physics.gravity.y;
        }

        //Move the player
        controller.Move(new Vector3(moveStateX, yState, moveStateZ) * moveSpeed * Time.deltaTime);
        
        Vector3 forwardDir = new Vector3(moveStateX, 0, moveStateZ);

        //If this if statement is not here, then there are continuos errors logged. Doesn't
        //affect gameplay at all, but it makes it annoying to deal with Debug.Log(), since
        //the console gets all clogged.
        if (forwardDir != Vector3.zero)
        {
            //Smooth character rotation
            transform.rotation = Quaternion.Slerp(transform.rotation,
                Quaternion.LookRotation(forwardDir),
                Time.deltaTime * rotationSpeed);
        }

    }

    /*
     * Takes care of the enemy following the player's movement.
     */
    private void EnemyFollowsPlayer()
    {
        StartCoroutine(SetOldPlayerData(transform, transform.position));

        //Have enemy follow player
        if (Vector3.Distance(enemy.transform.position, transform.position) > maxEnemyDistance)
        {
            enemyAnim.SetBool("isRunning", true);
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position,
                oldPlayerPos, moveSpeed * Time.deltaTime);
        }
        else
        {
            enemyAnim.SetBool("isRunning", false);
        }

        enemy.transform.LookAt(oldPlayerTransform);
    }

    /*
     * This sets the Vector3 position and the transform that needs to be looked at
     * by the enemy, but it is set with a delay so that the enemy is always running
     * towards the player's past position, and looking at the player's past position.
     * It essentially makes the enemy follow the player in a nice-looking, working way.
     */
    IEnumerator SetOldPlayerData(Transform transform, Vector3 pos)
    {
        yield return new WaitForSeconds(0.4f);
        oldPlayerPos = pos;
        oldPlayerTransform = transform;
    } 
}
