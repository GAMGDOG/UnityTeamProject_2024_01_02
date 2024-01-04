using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockObject : MonoBehaviour, IInteractable
{
    //들어가야 할 것들

    //if(Main.Playter.KeyCheck.Blue == true)
    //else

    public LockedDoorData lockedDoor;

    // Door { Animator, Collider }
    [SerializeField] private GameObject KeyDoor;

    [SerializeField] private BoxCollider _doorCollider;
    [SerializeField] private Animator _doorAnimator;

    private bool _isHaveKey;
    // Key { Color, bool }

    private void Start()
    {
        _doorCollider = KeyDoor.GetComponentInChildren<BoxCollider>();
        _doorAnimator = KeyDoor.GetComponentInChildren<Animator>();
    }


    public string GetInteractPrompt()
    {
        if (Main.Player.KeyCheck.Blue == true)
        {
            return string.Format("Open {0} ", lockedDoor.DisPlayName);
        }
        else
            return string.Format("Need BlueKey");
    }

    public void OnInteract()
    {
        switch(lockedDoor.DisPlayName)
        {
            case "Blue":
                if(Main.Player.KeyCheck.Blue == true)
                {
                    _doorAnimator.SetTrigger("OpenDoor");
                    Destroy(_doorCollider);
                }
                break;
        }
    }
}
