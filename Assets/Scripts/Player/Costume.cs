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

    private CostumeInfo currentCostume;

    public List<CostumeInfo> GetCostumeInfo()
    {
        List<CostumeInfo> costumeInfos = new List<CostumeInfo>();

        foreach (var costume in costumeList)
        {
            costumeInfos.Add(costume.costumeInfo);
        }

        return costumeInfos;
    }

    public void ChangeCostume(CostumeInfo info)
    {
        foreach (var _costume in costumeList)
        {
            if (_costume.costumeObj != null)
                _costume.costumeObj.SetActive(false);
        }

        GameObject costume = costumeList.Find(x => x.costumeInfo == info).costumeObj;
        if(costume != null)
            costume.SetActive(true);
    }
}
