using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

//Handles user input
public class PlayerController : MonoBehaviour 
{
    //------------ References ------------
    //Other GameObjects
    public GameUIManager gameUIManager;
    //public GameObject cameraPivotPoint;

    //Self
    public HealthManager healthManager;
    public CombatController combatController;
    public GameObject playerCamera;
    public Rigidbody playerRb;

    //------------ Private Variables ------------
    //Movement variables
    private float moveSpeed = 3f; //Player movement speed
    private float lookSensitivity = 100f;
    private float cameraRotation = 0f;
    private float interactionDistance = 5f;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!healthManager.isDead) //If player is not dead
        {
            HandleInput(); //Player can make inputs
        }
    }

    void HandleInput()
    {   
        //Gets mouse movement input
        float mouseX = Input.GetAxis("Mouse X") * lookSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * lookSensitivity * Time.deltaTime;

        //Gets movement input
        float moveHorizontal = Input.GetAxis("Horizontal"); //(AD)
        float moveVertical = Input.GetAxis("Vertical"); //(WS)

        LookController(mouseX, mouseY); 
        MovementController(moveHorizontal, moveVertical);

        RaycastHit hit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, interactionDistance))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            // Check if the hit object has the IInteractable interface
            if (interactable != null)
            {
                gameUIManager.UpdateInteractText(interactable.GetInteractPrompt());
                if(Input.GetButtonDown("Interact1")) //Interact 
                {
                // Interact with the object
                interactable.Interact();
                }
            } else
            {
                gameUIManager.ClearInteractText();
            }
        }

        if(Input.GetButtonDown("Inventory")) //If inventory button pressed
        {
            gameUIManager.ToggleInventoryUI(); //Toggle inventory
        }
        if(Input.GetButtonDown("Attack1")) //If attack button pressed
        {
            combatController.Attack(); //Attack
        }
        if(Input.GetButtonDown("Cancel")) //If esc button pressed
        {
            gameUIManager.ToggleEscapeMenuUI(); //Open escape menu
        }
    }

    //Handles camera movement and player rotation
    void LookController(float mouseX, float mouseY)
    {
        //Rotates camera vertically
        cameraRotation -= mouseY;
        cameraRotation = Mathf.Clamp(cameraRotation, -90f, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(cameraRotation, 0f, 0f);

        //Rotates player horizontally
        Quaternion playerRotation = Quaternion.Euler(0f, mouseX, 0f);
        playerRb.MoveRotation(playerRb.rotation * playerRotation);
    }

    //Handles player movement
    void MovementController(float moveHorizontal, float moveVertical)
    {
        //Moves the player
        Vector3 movementVector = transform.forward * moveVertical + transform.right * moveHorizontal;
        movementVector = Vector3.ClampMagnitude(movementVector, 1) * Time.fixedDeltaTime * moveSpeed; //Limit diagonal movement vector (makes diagonal speed same as regular speed)
        playerRb.MovePosition(playerRb.position + movementVector);
    }

    //Moves player to starting area (used on new dungeon level generation)
    public void MovePlayerToStart()
    {
        playerRb.position = new Vector3(0, 0.7f, 0);
    }
}
