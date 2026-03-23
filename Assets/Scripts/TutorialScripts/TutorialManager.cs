using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    //------------ References ------------
    public GameObject player;

    public TextMeshProUGUI tutorialText;
    public GameUIManager gameUIManager;

    //------------Public Variables ------------
    public Collider moveCollider; //Collider between jail cell & chest
    public bool moveColliderPassed = false; //If moveCollider has been passed
    public GameObject jailContainer; //Collider preventing player from leaving jail (before closing inv)
    public Collider escapeCollider; //Collider between hallway & study
    public bool escapeColliderPassed = false; //If escapeCollider has been passed
    public GameObject tutEnemy; //Tutorial enemy
    public GameObject tutEnemyContainer; //Collider containing enemy till player passes escapeCollider
    public GameObject combatContainer; //Collider preventing player from leaving study till enemy is killed
    public Collider exitRoomCollider; //Collider between hallway & exit room
    public bool exitRoomColliderPassed = false; //If exitRoomCollider has been passed



    //------------Private Variables ------------
    private int taskNumber = 0;
    private List<string> tutList = new List<string>();
    readonly string lookTut = "Move your mouse to look around.";
    readonly string moveTut = "Use WASD to move.";
    readonly string findWeaponTut = "Finally free! Let's find a weapon to stay that way. Maybe in the chest?";
    readonly string interactChestTut = "Press [F] to interact with the chest.";
    readonly string openInvTut = "Something was inside! Let's see what it was. Press [E] to open your inventory.";
    readonly string gotWeaponTut = "A weapon! Yes! Click on it to equip it.";
    readonly string closeInvTut = "Press [E] again to close your inventory and let's get out of here!";
    readonly string escapeMazeTut = "Search and find a way out of the dungeon.";
    readonly string combatTut = "Oh no! A guard! Use [L-Click] to attack him!";
    readonly string enemyDead = "Whew! That was close. Let's get out of here before more show up.";
    readonly string ladderFound = "Is that a ladder? Maybe it's the way out. Press [F] to climb it.";
    readonly string finishedTut = "Tutorial complete! Press [Esc] to return to the menu.";


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartTutorial();
    }

    // Update is called once per frame
    void Update()
    {   
        GetNextTask(); 
        DisplayNextTut(taskNumber);
        if(player.GetComponent<HealthManager>().isDead)
        {
            GameOver();
        }
    }

    void StartTutorial()
    {
        Cursor.lockState = CursorLockMode.Locked;
        PopulateTutList();

        player = GameObject.FindGameObjectWithTag("Player");
    }

    //Restarts tutorial
    public void RestartGame()
    {
        SceneManager.LoadScene("Tutorial");
    }

    //Stop tutorial
    public void GameOver()
    {   
        gameUIManager.SetGameOverUI();
        Cursor.lockState = CursorLockMode.None;
    }

    //Sends player back to menu
    public void LoadMenu()
    {   
        SceneManager.LoadScene("MenuScreen");
    }

    //Adds all tutorial strings to a list for easier updating
    void PopulateTutList()
    {
        tutList.Add(lookTut); //0
        tutList.Add(moveTut); //1
        tutList.Add(findWeaponTut); //2
        tutList.Add(interactChestTut); //3
        tutList.Add(openInvTut); //4
        tutList.Add(gotWeaponTut); //5
        tutList.Add(closeInvTut); //6
        tutList.Add(escapeMazeTut); //7
        tutList.Add(combatTut); //8
        tutList.Add(enemyDead); //9
        tutList.Add(ladderFound); //10
        tutList.Add(finishedTut); //11
    }

    void DisplayNextTut(int tutIndex)
    {
        ChangeTutText(tutList[tutIndex]);
    }

    //Changes tutorial text on UI
    void ChangeTutText(string tutText)
    {
        tutorialText.text = tutText;
    }

    //Handles task completion requirements
    void GetNextTask()
    {
        if(taskNumber == 0 && Input.GetAxis("Mouse X") != 0)
        {
            taskNumber = 1;
        } else if (taskNumber == 1 && Input.GetAxis("Vertical") != 0)
        {
            taskNumber = 2;
        } else if (taskNumber == 2 && moveCollider.GetComponent<TutColliderController>().colliderPassed == true)
        {
            taskNumber = 3;
        } else if (taskNumber == 3 && Input.GetButtonDown("Interact1"))
        {
            taskNumber = 4;
        } else if (taskNumber == 4 && Input.GetButtonDown("Inventory"))
        {
            taskNumber = 5;
        } else if (taskNumber == 5 && player.GetComponent<InventoryManager>().equippedWeapon != null)
        {
            taskNumber = 6;
        } else if (taskNumber == 6 && Input.GetButtonDown("Inventory"))
        {
            taskNumber = 7;
            jailContainer.SetActive(false);
        } else if (taskNumber == 7 && escapeCollider.GetComponent<TutColliderController>().colliderPassed == true)
        {
            taskNumber = 8;
            tutEnemyContainer.SetActive(false);
        } else if (taskNumber == 8 && tutEnemy == null)
        {
            taskNumber = 9;
            combatContainer.SetActive(false);
        } else if (taskNumber == 9 && exitRoomCollider.GetComponent<TutColliderController>().colliderPassed == true)
        {
            taskNumber = 10;
        } else if (taskNumber == 10 && Input.GetButtonDown("Interact1"))
        {
            taskNumber = 11;
        }
    }
}
