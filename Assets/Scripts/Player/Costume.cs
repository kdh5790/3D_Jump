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

    private void Start()
    {
        StartCoroutine(InitCostume());
    }

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

        Player.Instance.controller.equipmentSpeed = info.Speed;
        Player.Instance.controller.jumpPower = info.JumpPower * 10;
        Player.Instance.stats.SetEquipmentStatus(info.Health, info.Stamina);

        currentCostume = info;
    }

    private IEnumerator InitCostume()
    {
        yield return new WaitForSeconds(0.3f);
        ChangeCostume(costumeList[0].costumeInfo);
    }
}
