using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private float maxStamina = 100;
    private int currentHealth;

    public int Health
    {
        get { return currentHealth; }
        private set
        {
            currentHealth = Mathf.Clamp(value, 0, maxHealth);
            if (currentHealth <= 0)
            {
                // »ç¸Á
            }
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if(Player.Instance.controller.isSprint && Player.Instance.controller.moveState == MoveState.Move)
        {
            maxStamina -= Time.deltaTime;
        }
    }
}
