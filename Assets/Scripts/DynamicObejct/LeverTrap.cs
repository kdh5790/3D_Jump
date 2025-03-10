using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class LeverTrap : MonoBehaviour, ILeverActionable
{
    [SerializeField] private Transform trapTransform;
    [SerializeField] private Lever lever;
    public Lever Lever => lever;

    private BoxCollider boxCollider;
    private NavMeshObstacle navMeshObstacle;


    private void Start()
    {
        navMeshObstacle = GetComponent<NavMeshObstacle>();
        boxCollider = GetComponent<BoxCollider>();

        boxCollider.enabled = false;
        navMeshObstacle.carving = false;
        navMeshObstacle.enabled = false;
    }

    public void EndLeverAction()
    {
        lever.InitLeverRotation();
    }

    public void StartLeverAction()
    {
        StartCoroutine(ActiveTrap());
    }

    private IEnumerator ActiveTrap()
    {
        boxCollider.enabled = true;
        Vector3 startScale = trapTransform.localScale;
        Vector3 targetScale = new Vector3(1, 1, 1);
        float duration = 0.5f;
        float elapsedTime = 0f;

        navMeshObstacle.enabled = true;
        navMeshObstacle.carving = true;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            trapTransform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        yield return new WaitForSeconds(3f);

        elapsedTime = 0f;

        boxCollider.enabled = false;
        navMeshObstacle.carving = false;
        navMeshObstacle.enabled = false;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            trapTransform.localScale = Vector3.Lerp(targetScale, startScale, t);
            yield return null;
        }


        EndLeverAction();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            IDamageable damageable = other.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.OnDamaged(20);
            }
            else
            {
                Debug.LogError("IDamageable 인터페이스를 찾지 못했습니다.");
            }
        }
    }
}
