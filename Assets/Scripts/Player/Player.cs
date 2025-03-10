using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player instance;
    public static Player Instance { get { return instance; } }

    public PlayerController controller;
    public PlayerStats stats;
    public Costume costume;

    void Awake()
    {
        if (instance == null)
            instance = this;

        controller = GetComponent<PlayerController>();
        stats = GetComponent<PlayerStats>();
        costume = GetComponent<Costume>();
    }
}
