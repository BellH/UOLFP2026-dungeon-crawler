using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using Unity.VisualScripting;

public class GameUIManager : MonoBehaviour
{
    //------------ References ------------
    public GameObject playerUI; //Player UI parent element
    public GameObject loadingScreen;
    public GameObject escapeMenu;
    public GameObject deathScreen;
    public TextMeshProUGUI floorCounterText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI interactText;
    
    //Inventory elements
    public InventoryManager inventoryManager;
    public Inventory inventory;
    public GameObject inventoryScreen;
    public GameObject weaponsListScrollView; //Weapons panel main parent
    public GameObject consumablesListScrollView; //Consumabels panel main parent

    public GameObject weaponsContent; //Weapons panel parent for list of items
    public GameObject consumablesContent; //Consumables panel parent for list of items

    public GameObject itemBar; //Prefab of item bar for displaying inventory contents

    //------------ Private Variables ------------
    private List<GameObject> itemList; 
    bool escapeMenuOpen = false;
    bool inventoryOpen = false;
    bool firstOpen = true;

    void Start()
    {
        itemList = new List<GameObject>();
    }

    //------------ Inventory UI Methods ------------
    //Toggles inventory visibility
    public void ToggleInventoryUI()
    {
        if(firstOpen) //Ensures inventory is accessed only after it has been created
        {
            GetPlayerInventory();
            firstOpen = false;
        }

        //Toggles inventory visibility
        if(!inventoryOpen)
        {   
            inventoryOpen = true;
            DisplayInventory();
        } else if (inventoryOpen)
        {
            HideInventory();
            inventoryOpen = false;
        }
    }

    void GetPlayerInventory()
    {
        inventoryManager = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryManager>();
        inventory = inventoryManager.inventory;
    }

    //Displays inventory GUI
    public void DisplayInventory()
    {
        Cursor.lockState = CursorLockMode.None;
        inventoryScreen.SetActive(true);
        ShowWeaponsList();
    }

    //Hides inventory GUI
    public void HideInventory()
    {   
        Cursor.lockState = CursorLockMode.Locked;
        inventoryScreen.SetActive(false);
        DepopulateItemList();
    }

    //Shows weapons list on weapon button press
    public void ShowWeaponsList()
    {   
        consumablesListScrollView.SetActive(false); //Hide other item list
        DepopulateItemList(); //Reset item list

        //Populate item list with weapons
        if(inventory.weapons != null) 
        {
            foreach(ItemInstance weapon in inventory.weapons)
            {
                PopulateItemList(weapon, weaponsContent);
            }
        }

        weaponsListScrollView.SetActive(true); //Show weapons list
    }

    //Shows consumables list on consumables button press
    public void ShowConsumablesList()
    {

        weaponsListScrollView.SetActive(false); //Hide other item list
        DepopulateItemList(); //Reset item list

        //Populate item list with consumables
        if(inventory.consumables != null) 
        {
            foreach(ItemInstance consumable in inventory.consumables)
            {
                PopulateItemList(consumable, consumablesContent);
            }
        }

        consumablesListScrollView.SetActive(true); //Show consumables list
    }

    //Creates an item button for each item passed to it
    void PopulateItemList(ItemInstance item, GameObject parentPanel)
    {
        GameObject itemInvBar;
        TextMeshProUGUI itemInvNameText;
        Button itemButton;

        itemInvBar = Instantiate(itemBar, parentPanel.transform); //Make new item bar as child of weaponsPanel
        itemInvNameText = itemInvBar.GetComponentInChildren<TextMeshProUGUI>(); //Get item name component
        itemInvNameText.text = item.itemType.itemName; //Display item.name
        
        itemButton = itemInvBar.GetComponentInChildren<Button>();
    
        itemButton.onClick.AddListener(() => item.itemType.Use(item, inventoryManager)); //Tell each button instance to call the associated Use() method for each item

        itemList.Add(itemInvBar); //Add instance to itemlist
    }

    //Removes all items in list
    void DepopulateItemList()
    {
        foreach(GameObject item in itemList)
        {
            Destroy(item);
        }

        itemList.Clear();
    }

    // ------------ Floor & Health Update Methods ------------
    //Updates floor counter text
    public void UpdateFloorCounterText(int floor)
    {
        floorCounterText.text = "Floor: " + floor;
    }

    public void UpdateHealthText(int health)
    {
        healthText.text = "Health: " + health;
    }

    //------------ Interact Text Update Methods ------------
    public void UpdateInteractText(string text)
    {
        interactText.text = text;
    }

    public void ClearInteractText()
    {
        interactText.text = "";
    }


    //------------ Setup/Loading Screen, Esc Menu, & Death Screen UI ------------
    //Start & Restart game UI setup
    public void SetPlayerUIActive()
    {
        playerUI.SetActive(true);
    }

    //Toggle loading screen UI
    public void SetLoadingScreenActive()
    {
        loadingScreen.SetActive(true);
    }

    public void SetLoadingScreenInactive()
    {
        loadingScreen.SetActive(false);
    }

    //Toggle escape menu UI
    public void ToggleEscapeMenuUI()
    {
        if(!escapeMenuOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            escapeMenu.SetActive(true);
            escapeMenuOpen = true;
        } else if (escapeMenuOpen)
        {
            escapeMenuOpen = false;
            escapeMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    
    //Deathscreen UI
    public void SetGameOverUI()
    {
        playerUI.SetActive(false);
        deathScreen.SetActive(true);
    }
}
