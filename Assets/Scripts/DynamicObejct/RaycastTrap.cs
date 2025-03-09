using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RaycastTrap : MonoBehaviour
{
    [SerializeField] private Transform rayPivot;
    [SerializeField] private GameObject trapObj;
    [SerializeField] private GameObject trapTarget;

    private BoxCollider boxCollider;
    private bool isActive;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;
    }

    private void Update()
    {
        if(Vector3.Distance(Player.Instance.transform.position, rayPivot.position) < 10f && !isActive && Physics.Raycast(rayPivot.position, transform.forward * 15f, out RaycastHit hitInfo))
        {
            isActive = true;
            StartCoroutine(TrapActive());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player") && collision.transform.TryGetComponent(out PlayerStats stats))
        {
            collision.rigidbody.AddForce(-collision.transform.forward * 120f, ForceMode.VelocityChange);
            stats.OnDamaged(50);
        }
    }

    private IEnumerator TrapActive()
    {
        float duration = 0.5f;
        float elapsedTime = 0f;

        Vector3 startPosition = trapObj.transform.position;

        yield return new WaitForSeconds(0.2f);

        boxCollider.enabled = true;

        while (elapsedTime < duration)
        {
            trapObj.transform.position = Vector3.Lerp(startPosition, trapTarget.transform.position, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        boxCollider.enabled = false;
        elapsedTime = 0f;
        duration = 1f;

        Vector3 tempPos = trapObj.transform.position;

        yield return new WaitForSeconds(1f);

        while (elapsedTime < duration)
        {
            while (elapsedTime < duration)
            {
                trapObj.transform.position = Vector3.Lerp(tempPos, startPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        isActive = false;
    }
}
