using System.Collections;
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

    // 오브젝트 감지 코루틴
    private IEnumerator ObjectCheckCoroutine()
    {
        // 플레이어 사망 판정 시 멈추도록 나중에 변경
        while (true)
        {
            // 화면에 중앙 부분에서 ray 발사 (크로스헤어)
            ray = rayCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

            if (Physics.Raycast(ray, out hit, distance, layer))
            {
                // 충돌한 오브젝트의 인터페이스를 검사해 존재한다면 정보를 가져와 UI에 띄워줌
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

            // ray 검사는 0.5초마다 실행되도록 함
            yield return new WaitForSeconds(0.5f);
        }
    }
}
