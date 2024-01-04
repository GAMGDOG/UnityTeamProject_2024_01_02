using UnityEngine;

[CreateAssetMenu(fileName = "LockedDoor", menuName = "New LockedDoor")]
public class LockedDoorData : ScriptableObject
{
    [Header("Info")]
    [SerializeField] private string _displayName;

    [SerializeField] private bool _isHasKey;




    public string DisPlayName
    {
        get { return _displayName; }
        private set { _displayName = value; }
    }
}
