using UnityEngine;

using TMPro; 
using UnityEngine.UI; 
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{   
    //------------ References ------------
    public GameUIManager gameUIManager;
    public DungeonGenerator dungeonGenerator;
    public PlayerController playerController;

    //------------ Private Variables ------------
    private bool isGameActive;
    private int levelCounter = 0; //Dungeon size is influenced by level counter (# of dungeons the player has cleared) 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        StartGame();
    }

    void StartGame()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>(); //Find player controller

        LoadNextLevel(); 
        isGameActive = true;
        gameUIManager.SetPlayerUIActive();
    }

    // //Implemented for testing Dungeon Generation Times (Uncomment to test)
    // void Update()
    // {
    //     if(Input.GetKeyDown(KeyCode.Space))
    //     {
    //         LoadNextLevel();
    //     }
    // }

    //Resets dungeon and moves player to 'next level'
    public void LoadNextLevel()
    { 
        Cursor.lockState = CursorLockMode.Locked;

        levelCounter += 1;
        StartCoroutine(dungeonGenerator.HandleLevelGeneration(levelCounter));
        playerController.MovePlayerToStart(); //Move player to start of game
        gameUIManager.UpdateFloorCounterText(levelCounter);
    }

    //Restarts game
    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }

    //Stop game
    public void GameOver()
    {   
        isGameActive = false;
        gameUIManager.SetGameOverUI();
        Cursor.lockState = CursorLockMode.None;
    }

    //Sends player back to menu
    public void LoadMenu()
    {   
        SceneManager.LoadScene("MenuScreen");
    }
}
