using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectInteractable : MonoBehaviour
{
    public Transform cameraTransform;
    private readonly float distance = 3f;
    public InventoryManager invManager;
    public PopupText popupText;

    // Layer Information
    public LayerMask layerMaskInteract;
    public string _excludeLayer = null;

    // Saved Colour Information
    private Color originColour;
    private MeshRenderer lastChanged;

    private bool eventTwoBedTriggered = false;
    private bool eventTwoBookTriggered = false;
    public bool eventTwoComplete = false;
    private bool eventThreeTriggered = false;
    public bool eventFourComplete = false;
    private bool eventFiveTriggered = false;

    public GameObject inventory;
    public GameObject flashlight;
    private bool note;
    private bool flashlightBool;
    private bool briefcaseBool;

    public SafePuzzle safePuzzle;

    // Update is called once per frame
    void Update()
    {
        int mask = 1 << LayerMask.NameToLayer(_excludeLayer) | layerMaskInteract.value;

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward * distance, out RaycastHit hit, distance, mask))
        {
            CheckChange();

            // If object is interactable then highlight it
            if (hit.collider.tag == "Interactable")
            {
                HighlightObject(hit);
                // If object is interactable and interact button is pressed, add the item to the player's inventory
                if (Input.GetKeyDown(KeyCode.E))
                {
                    switch (hit.collider.name)
                    {
                        case "Bed":
                            if (!eventTwoBedTriggered)
                            {
                                EventManager.current.SceneTwoStart();
                                eventTwoBedTriggered = true;
                            }
                            else if (!eventThreeTriggered && eventTwoComplete)
                            {
                                EventManager.current.SceneThree();
                                eventThreeTriggered = true;
                            }
                            break;
                        case "FallenBook":
                            if (!eventTwoBookTriggered)
                            {
                                EventManager.current.SceneOneTransitionToTwo();
                                eventTwoBookTriggered = true;
                            }
                            else if (!eventFiveTriggered && eventFourComplete)
                            {
                                EventManager.current.SceneFive();
                                eventFiveTriggered = true;
                            }
                            break;
                        case "Key":
                            EventManager.current.SceneEightBegin();
                            break;
                        case "FinalDoor":
                            EventManager.current.FinalScene();
                            break;
                        case "RippedPage":
                            EventManager.current.SceneTwo();
                            break;
                        case "SecondApartmentNote":
                            Destroy(hit.collider.gameObject);
                            StartCoroutine(popupText.PopupLong("If you are reading this, then I didn't escape. It got me. Take my stuff and get out of here... while you still can."));
                            StartCoroutine(NoteWait());
                            break;
                        case "Briefcase":
                            if (note)
                            {
                                briefcaseBool = true;
                                Destroy(hit.collider.gameObject);
                                inventory.SetActive(true);
                                StartCoroutine(popupText.Popup("Press I to open inventory."));
                                if (flashlightBool && briefcaseBool)
                                    StartCoroutine(NoKey());
                            }
                            break;
                        case "Flashlight":
                            if (note)
                            {
                                flashlightBool = true;
                                Destroy(hit.collider.transform.parent.gameObject);
                                flashlight.SetActive(true);
                                StartCoroutine(popupText.Popup("Press F to toggle flashlight."));
                                if (flashlightBool && briefcaseBool)
                                    StartCoroutine(NoKey());
                            }
                            break;
                        case "Button":
                            hit.collider.gameObject.GetComponent<SafeButton>().PressButton();
                            break;
                        default:
                            break;
                    }
                }
            }
            if (hit.collider.tag == "Collectible")
            {
                HighlightObject(hit);
                // If object is collectable and interact button is pressed, add the item to the player's inventory
                if (Input.GetKeyDown(KeyCode.E))
                {
                    // Display some pickup text
                    StartCoroutine(popupText.Popup("Picked up " + hit.collider.name));
                    // Add it to the player's inventory
                    invManager.PickUpItem(hit.collider.name);
                    // Destroy object
                    Destroy(hit.collider.gameObject);
                }
            }
            if (hit.collider.tag == "Door")
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    LockedDoor lockedDoor = hit.collider.gameObject.GetComponent<LockedDoor>();
                    lockedDoor.interacted = true;
                }
            }
        }
        else
        {
            CheckChange();
        }
    }
    private void CheckChange()
    {
        // If nothing is being looked at/something else is being looked at, an object's colour has been changed and that object's colour is not its original colour, set it back to its original colour
        if (lastChanged != null && lastChanged.material.color != originColour)
        {
            lastChanged.material.color = originColour;
        }
    }

    private void HighlightObject(RaycastHit hit)
    {
        // Highlight in world
        MeshRenderer renderer = hit.collider.GetComponent<MeshRenderer>();
        originColour = renderer.material.color;
        lastChanged = renderer;
        renderer.material.color = new Color(0.6f, 1.5f, 2f);
    }
    private IEnumerator NoteWait()
    {
        yield return new WaitForSeconds(6f);
        note = true;
    }
    private IEnumerator NoKey()
    {
        eventFourComplete = true;
        yield return new WaitForSeconds(3f);
        NoKeyPopup();
    }
    private void NoKeyPopup()
    {
        StartCoroutine(popupText.PopupLong("Well the key isn't here. Maybe I should look for that fairy-tale book instead. It might explain what's going on here."));
    }
}
