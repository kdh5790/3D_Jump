using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CostumeWrapper // 코스튬 정보를 담아둘 클래스
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

        // 변경할 코스튬 정보로 costumeList에서 일치하는 코스튬을 찾아 변경
        GameObject costume = costumeList.Find(x => x.costumeInfo == info).costumeObj;
        if(costume != null)
            costume.SetActive(true);

        // 코스튬에 맞는 스탯 설정
        Player.Instance.controller.equipmentSpeed = info.Speed;
        Player.Instance.controller.jumpPower = info.JumpPower * 10;
        Player.Instance.stats.SetEquipmentStatus(info.Health, info.Stamina);

        currentCostume = info;
    }

    private IEnumerator InitCostume()
    {
        // 게임 시작 시 초기 코스튬으로 적용
        yield return new WaitForSeconds(0.3f);
        ChangeCostume(costumeList[0].costumeInfo);
    }
}
