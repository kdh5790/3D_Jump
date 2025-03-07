using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILeverActionable
{
    void StartLeverAction();
    void EndLeverAction();

    Lever Lever { get; }
}

public class MovingPlatform : MonoBehaviour, ILeverActionable
{
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 endPos;

    [SerializeField] private Lever lever;
    public Lever Lever => lever;

    private void Start()
    {
        startPos = transform.position;
    }

    public void StartLeverAction()
    {
        StartCoroutine(MoveToTargetPos());
    }

    public void EndLeverAction()
    {
        StartCoroutine(lever.InitInteractState());
    }

    private IEnumerator MoveToTargetPos()
    {
        Vector3 target = Vector3.Distance(transform.position, startPos) < Vector3.Distance(transform.position, endPos) ? endPos : startPos;

        Player.Instance.transform.SetParent(transform);

        while (Vector3.Distance(transform.position, target) >= 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * 5f);

            yield return null;
        }

        transform.position = target;

        Player.Instance.transform.SetParent(null);

        EndLeverAction();
    }
}
