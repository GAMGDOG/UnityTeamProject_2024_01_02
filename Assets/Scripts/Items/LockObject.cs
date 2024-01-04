using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockObject : MonoBehaviour, IInteractable
{
    public LockedDoorData lockedDoor;

    [SerializeField] private GameObject KeyDoor;

    private BoxCollider _doorCollider;
    private Animator _doorAnimator;

    //case blue, red, green 일때 따로따로 배치하는 방법이 필요하다.


    private void Start()
    {
        _doorCollider = KeyDoor.GetComponentInChildren<BoxCollider>();
        _doorAnimator = KeyDoor.GetComponentInChildren<Animator>();
    }


    public string GetInteractPrompt()
    {
        switch (lockedDoor.DisPlayName)
        {
            case "Blue":
                if (Main.Player.KeyCheck.Blue == true)
                    return string.Format("Open {0} Door", lockedDoor.DisPlayName);
                else
                    return string.Format("Need {0} Key", lockedDoor.DisPlayName);

            case "Red":
                if (Main.Player.KeyCheck.Red == true)
                    return string.Format("Open {0} Door", lockedDoor.DisPlayName);
                else
                    return string.Format("Need {0} Key", lockedDoor.DisPlayName);

            case "Green":
                if (Main.Player.KeyCheck.Green == true)
                    return string.Format("Open {0} Door", lockedDoor.DisPlayName);
                else
                    return string.Format("Need {0} Key", lockedDoor.DisPlayName);
        }
        //return string.Format("Open {0} Door", lockedDoor.DisPlayName);
        return string.Format("Need {0} Key", lockedDoor.DisPlayName);
    }

    public void OnInteract()
    {
        switch(lockedDoor.DisPlayName)
        {
            case "Blue":
                if(Main.Player.KeyCheck.Blue == true)
                    DoorInteraction();
                break;
            case "Red":
                if(Main.Player.KeyCheck.Red == true)
                    DoorInteraction();
                break;
            case "Green":
                if(Main.Player.KeyCheck.Green == true)
                    DoorInteraction();
                break;
        }
    }

    public void DoorInteraction()
    {
        _doorAnimator.SetTrigger("OpenDoor");
        Destroy(_doorCollider);
    }
}
