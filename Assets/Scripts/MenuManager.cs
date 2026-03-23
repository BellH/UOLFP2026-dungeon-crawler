using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    //Loads player into game scene
    public void StartGame()
    {   
        SceneManager.LoadScene("Game");
    }

    //Loads player into tutorial scene
    public void StartTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

}
