using UnityEngine;

public class SnapPointBehaviour : MonoBehaviour
{
    public GameObject pointBlocked;
    [SerializeField] private bool isAvailable = true;

    //Checks if snap point is available
    public bool CheckAvailable()
    {
        if (isAvailable)
        {
            return true;
        } else
        {
            return false;
        }
    }

    //Sets snap point to unavailable (if snap point is blocked or snap point was successfully connected to another room)
    public void SetUnavailable()
    {
        isAvailable = false;
    }

    //Opens passage (if snap point was successfully connected to another room)
    public void SetOpened()
    {
        pointBlocked.SetActive(false);
    }
}
