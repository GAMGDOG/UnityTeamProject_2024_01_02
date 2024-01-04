using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    [SerializeField] private Collider myCollider;

    private int damage;
    private float knockback;

    private List<Collider> myColliders = new List<Collider>();

    private void OnEnable()
    {
        myColliders.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == myCollider) return;
        if(myColliders.Contains(other)) return;

        myColliders.Add(other);

        if(other.TryGetComponent(out PlayerHealth health))
        {
            
        }
    }
}
