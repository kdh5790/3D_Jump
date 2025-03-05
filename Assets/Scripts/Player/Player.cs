using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player instance;
    public static Player Instance { get { return instance; } }

    public PlayerController controller;
    public PlayerStats stats;

    void Awake()
    {
        if (instance == null)
            instance = this;

        controller = GetComponent<PlayerController>();
        stats = GetComponent<PlayerStats>();
    }
}
