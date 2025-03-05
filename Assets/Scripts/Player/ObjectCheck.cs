using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectCheck : MonoBehaviour
{
    [SerializeField] private LayerMask layer;
    [SerializeField] private DescriptionUI descriptionUI;

    private RaycastHit hit;
    private Ray ray;

    private float distance = 7f;
    private Camera rayCamera;

    void Start()
    {
        rayCamera = Camera.main;
        descriptionUI = UIManager.Instance.descriptionUI;
        StartCoroutine(ObjectCheckCoroutine());
    }


    private IEnumerator ObjectCheckCoroutine()
    {
        // 플레이어 사망 판정 시 멈추도록 나중에 변경
        while (true)
        {
            ray = rayCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

            if (Physics.Raycast(ray, out hit, distance, layer))
            {
                if(hit.transform.TryGetComponent(out IInteractive obj))
                {
                    ObjectInfo info = obj.GetObjectInfo();
                    descriptionUI.SetInfoText(info);
                }
            }
            else
            {
                descriptionUI.SetInfoText();
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
}
