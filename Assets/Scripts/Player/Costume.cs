using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CostumeWrapper
{
    public GameObject costumeObj;
    public CostumeInfo costumeInfo;
}

public class Costume : MonoBehaviour
{
    [SerializeField] private List<CostumeWrapper> costumeList = new List<CostumeWrapper>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
