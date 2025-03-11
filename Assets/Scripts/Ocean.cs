using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ocean : MonoBehaviour
{
    [SerializeField] private Transform startPos;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(other.TryGetComponent(out IDamageable player))
            {
                player.OnDamaged(15);
                other.transform.position = startPos.position;
            }
        }
    }
}
