using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{   
    //------------ References ------------
    public GameUIManager gameUIManager;
    public SpawnsManager spawnsManager; //Manages all spawners

    //Prefabs & rooms
    public GameObject startRoom; //Start room prefab
    public GameObject endRoom; //End room prefab 
    public List<GameObject> rooms; //List of possible prefab rooms

    //------------ Private Variables ------------
    private int dungeonSize;
    private int startSize = 10;
    [SerializeField] private List<GameObject> placedRooms; //Rooms placed in dungeon 

    void Start()
    {
        spawnsManager = GameObject.Find("SpawnsManager").GetComponent<SpawnsManager>();
    }

    public IEnumerator HandleLevelGeneration(int levelNumber)
    {
        var watch = System.Diagnostics.Stopwatch.StartNew(); 
        gameUIManager.SetLoadingScreenActive();

        //Clears current dungeon
        ClearCurrentDungeon();
        yield return new WaitForSeconds(1);

        //Clear current spawns (containers & enemies from previous level)
        spawnsManager.ClearAllSpawns();
        yield return new WaitForSeconds(1);

        placedRooms = new List<GameObject>(dungeonSize + levelNumber);

        //Generates a new dungeon
        GenerateDungeon(levelNumber);
        yield return new WaitForSeconds(1);

        //Spawn enemies & loot containers
        spawnsManager.HandleAllSpawners();
        yield return new WaitForSeconds(1);

        gameUIManager.SetLoadingScreenInactive();

        watch.Stop();
        var elapsedMs = watch.ElapsedMilliseconds;
        Debug.Log("Evaluation - Generation Time (milliseconds): " + elapsedMs); //Display how long generating dungeon took (in milliseconds)
    }

    //------------ Dungeon Creation & Destruction Functions ------------
    //Clears current dungeon
    public void ClearCurrentDungeon()
    {   
        //Destroy every room
        foreach(GameObject room in placedRooms)
        {
            Destroy(room);
        }
        placedRooms.Clear(); //Clear list
        // Debug.Log("PlacedRooms Count: " + placedRooms.Count);
    }

    void RetryDungeonGeneration(int levelNumber)
    {
        StopCoroutine(HandleLevelGeneration(levelNumber));

        StartCoroutine(HandleLevelGeneration(levelNumber));
    }

    //Generates dungeon
    public void GenerateDungeon(int levelNumber)
    {   
        dungeonSize = startSize + levelNumber; //# of rooms in dungeon = startSize + levelNumber (which is the # of floors cleared) 
        GameObject currentRoom;
        GameObject currSnapPoint;
        GameObject newRoom; 
        GameObject newSnapPoint;

        int attemptCounter = 0;

        currentRoom = Instantiate(startRoom, new Vector3(0, 0, 0), Quaternion.identity); //Creates the starting room 
        placedRooms.Add(currentRoom); //Add starting room to list of placed rooms

        //Generates the dungeon randomly past the start room
        for(int i = 0; i < dungeonSize; i++)
        {   
            if(attemptCounter >= 50000) //All snap points have been blocked
            {
                RetryDungeonGeneration(levelNumber);

                //throw new System.Exception("Attempt Counter hit."); //Something has gone wrong.

                break;
            }
            attemptCounter++;

            if(RandomBacktrackChance()) //If random backtrack chance succeeds
            {
                currentRoom = Backtrack(currentRoom); //Backtrack to previous room
                i--; //Decrement counter
                continue; //Skip to next loop
            }

            currSnapPoint = PickSnapPoint(currentRoom); //Pick a random snap point

            if(currSnapPoint == null) //If no snap points available in current room
            {
                currentRoom = Backtrack(currentRoom); //Backtrack to previous room
                i--; //Decrement counter
                continue; //Skip to next loop
            }

            if (i >= dungeonSize - 1) //If last room in dungeon
            {
                newRoom = Instantiate(endRoom, new Vector3(100, 100, 100), Quaternion.identity); //Spawn end room
            } else 
            {
                newRoom = Instantiate(PickRandomRoom(), new Vector3(100, 100, 100), Quaternion.identity); //Else, spawn randomly picked room
            }

            newSnapPoint = PickSnapPoint(newRoom); //pick a random available snap point from the new room

            //Rotate new room to correct orientation (so snap points are aligned)
            newRoom.transform.rotation *= RotateNewRoom(currSnapPoint.transform, newSnapPoint.transform);
            //Move new room to correct placement (so snap points are together)
            newRoom.transform.position += currSnapPoint.transform.position - newSnapPoint.transform.position;

            //If space is already occupied
            if(newRoom.GetComponent<RoomBehaviour>().IsSpaceOccupied())
            {   
                // Retry room placement
                int retryAttempts = 0;
                while(retryAttempts < 5)
                {
                    retryAttempts++;

                    MarkSnapPointUnavailable(newSnapPoint); //Snap point didn't work, mark as unavailable
                    newSnapPoint = PickSnapPoint(newRoom); //Pick a new random available snap point from new room

                    if(newSnapPoint == null) //If all snap points tried
                    {
                        Destroy(newRoom); //Destroy the current room instance
                        i -= 1; //Decrement dungeonsize counter
                        //Debug.Log("Space occupied!");
                        break;
                    }

                    //Rotate new room to correct orientation (so snap points are aligned)
                    newRoom.transform.rotation *= RotateNewRoom(currSnapPoint.transform, newSnapPoint.transform);
                    //Move new room to correct placement (so snap points are together)
                    newRoom.transform.position += currSnapPoint.transform.position - newSnapPoint.transform.position;

                    if(!newRoom.GetComponent<RoomBehaviour>().IsSpaceOccupied()) //If new position is not occupied
                    {
                        MarkSnapPointUsed(currSnapPoint);
                        MarkSnapPointUsed(newSnapPoint);
                        placedRooms.Add(newRoom);                
                        currentRoom = newRoom; // set current room to new room
                        //Debug.Log("Room successfully placed!");
                        break;
                    }
                }
            } else
            {
                MarkSnapPointUsed(currSnapPoint);
                MarkSnapPointUsed(newSnapPoint);
                placedRooms.Add(newRoom);                
                currentRoom = newRoom; // set current room to new room
                //Debug.Log("Room successfully placed!");
            }
        }

        Debug.Log("Evaluation - Total Rooms: " + placedRooms.Count);
    }

    //Marks snap point as unavailable (something blocking it being used)
    void MarkSnapPointUnavailable(GameObject snapPoint)
    {
        snapPoint.GetComponent<SnapPointBehaviour>().SetUnavailable();
    }

    //Marks snap point as used (room has been attached to it)
    void MarkSnapPointUsed(GameObject snapPoint)
    {
        snapPoint.GetComponent<SnapPointBehaviour>().SetOpened();
    }

    //Returns rotation needed to correct new room's orientation
    Quaternion RotateNewRoom(Transform currSnapPoint, Transform newSnapPoint)
    {   
        Vector3 currSnapRot = currSnapPoint.forward; //Gets current room snap point rotation
        Vector3 newSnapRot = Vector3.ProjectOnPlane(newSnapPoint.forward, Vector3.up);
        Vector3 desiredNewSnapRot = Vector3.ProjectOnPlane(-currSnapRot, Vector3.up); //New rotation should be opposite the current room's snap point rotation (all snap points are rotated facing outward from the room)

        //Calculate signed angle around the y-axis
        float angle = Vector3.SignedAngle(newSnapRot, desiredNewSnapRot, Vector3.up);
        
        //Rotate only around y-axis
        Quaternion rotation = Quaternion.Euler(0, angle, 0);

        return rotation; 
    }

    //Chooses a random snap point for the applicable room
    GameObject PickSnapPoint(GameObject room)
    {
        //Local Variables
        List<int> availablePoints = new List<int>();
        List<GameObject> allPoints = room.GetComponent<RoomBehaviour>().snapPoints; //Get list of points

        //Finds available snap points
        for(int i = 0; i < allPoints.Count; i++)
        {
            if(allPoints[i].GetComponent<SnapPointBehaviour>().CheckAvailable())
            {
                availablePoints.Add(i); //Save index of available points to list
            }
        }

        //If there are no point available
        if(availablePoints.Count == 0)
        {
            return null;
        }
        
        //Picks random snap point from list of available points
        int index = Random.Range(0, availablePoints.Count); //Pick random index
        GameObject snapPoint = allPoints[availablePoints[index]]; //Pick index of available points with random index
        MarkSnapPointUnavailable(snapPoint);
        return snapPoint;
    }

    //Chooses a random room to be next room
    GameObject PickRandomRoom()
    {
        int index = Random.Range(0, rooms.Count);
        GameObject room = rooms[index];
        return room;
    }

    //Determines if dungeon generation should randomly backtrack
    bool RandomBacktrackChance()
    {
        if(Random.Range(0, 100) <= 5)
        {
            //Debug.Log("Randomly Backtracking!");
            return true;
        }
        return false;
    }


    //Return gameobject of room to backtrack to
    GameObject Backtrack(GameObject currentRoom)
    {
        int currentRoomIndex = placedRooms.IndexOf(currentRoom);
        int backtrackRoomIndex = currentRoomIndex - 1;

        if(backtrackRoomIndex < 0) //If index is negative
        {
            backtrackRoomIndex = placedRooms.Count - 1; //Start from last room in list
        }

        return placedRooms[backtrackRoomIndex];
    }

    
}
