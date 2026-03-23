using System.Collections.Generic;
using UnityEngine;

public class ContainerController : MonoBehaviour, IInteractable
{   
    //------------ References ------------
    public InventoryManager inventoryManager;
    public LootPool containerLootPool; //Pool of loot that can potentially be found in containers

    //------------ Private Variables ------------
    public List<ItemInstance> contents;
    [SerializeField] bool playerInRange = false;
    string containerPrompt = "Press [F] to open chest";

    int minContents = 1; //Minimum number of items per container
    int maxContents = 4; //Maximum number of items per container

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        PopulateContainer();
        inventoryManager = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryManager>();
    }

    //Removes item from container & adds to player inventory
    public void Interact()
    {   
        for(int i = 0; i < contents.Count; i++)
        {
            inventoryManager.AddToInventory(contents[i]);
            RemoveItem(i);
        }
    }

    public string GetInteractPrompt()
    {
        return containerPrompt;
    }

    //Determines what loot will be in the container
    void PopulateContainer()
    {   
        int itemIndex;

        //Pick random number of items to be in container
        int numItems = Random.Range(minContents, maxContents); 

        for(int i = 0; i < numItems; i++) //For number of items
        {   
            itemIndex = Random.Range(0, containerLootPool.loot.Count); //Pick random item index
            contents.Add(new ItemInstance(containerLootPool.loot[itemIndex])); //place random item into contents
        }
    }

    //Removes items from container
    void RemoveItem(int index)
    {
        contents.Remove(contents[index]);
    }

    //Determines if player is close enough to open the container
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    //Determines if player is close enough to open the container
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
