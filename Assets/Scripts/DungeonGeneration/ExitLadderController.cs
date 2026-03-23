using UnityEngine;

public class ExitLadderController : MonoBehaviour, IInteractable
{
    //------------ References ------------
    public GameObject gameManagerObj;
    public GameManager gameManager;

    //------------ Private Variables ------------
    string exitLadderPrompt = "Press [F] to enter next level";

    void Start()
    {
        gameManagerObj = GameObject.Find("GameManager");
        gameManager = gameManagerObj.GetComponent<GameManager>();
    }

    //When player interacts with ladder
    public void Interact()
    {   
        gameManager.LoadNextLevel(); //Load next level
    }

    public string GetInteractPrompt()
    {
        return exitLadderPrompt;
    }
}
