using UnityEngine;

public class TutColliderController : MonoBehaviour
{
    public bool colliderPassed = false;

    void OnTriggerEnter(Collider other)
    {
        colliderPassed = true;
    }
}
