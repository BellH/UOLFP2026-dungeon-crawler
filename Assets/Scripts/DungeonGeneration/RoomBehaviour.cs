using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    //------------ References ------------
    public LayerMask roomsLayerMask;
    public List<GameObject> snapPoints;

    //------------ Public Variables ------------
    public Collider roomCollider;

    // Checks if the space the room is placed is available
    public bool IsSpaceOccupied()
    {
        //Update collider positions
        Physics.SyncTransforms();

        roomCollider = GetComponent<Collider>();

        if (roomCollider == null) return false; //If no collider, assume not occupied

        roomsLayerMask = LayerMask.GetMask("Rooms");

        Vector3 center = roomCollider.bounds.center;
        Vector3 halfExtents = roomCollider.bounds.size / 2;
        Quaternion orientation = Quaternion.identity;

        //Check for colliders in space room will go
        Collider[] hitColliders = Physics.OverlapBox(center, halfExtents, orientation, roomsLayerMask);

        //For each collider hit
        foreach (Collider hitCollider in hitColliders) {
            //Debug.Log("Hit : " + hitCollider.name);
            //If collider overlap is not from the room itself or its' children
            if (!hitCollider.transform.IsChildOf(transform)) {
                return true; 
            }
        }
        return false;
    }
}
