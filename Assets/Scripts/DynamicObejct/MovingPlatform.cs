using System.Collections;
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
    [SerializeField] private Transform boxPivot;
    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private Lever lever;
    public Lever Lever => lever;


    private Vector3 boxSize = new Vector3(5f, 2f, 5f);

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
        lever.InitLeverRotation();
    }

    private IEnumerator MoveToTargetPos()
    {
        Vector3 target = Vector3.Distance(transform.position, startPos) < Vector3.Distance(transform.position, endPos) ? endPos : startPos;

        float moveSpeed = 5f;

        Player.Instance.transform.SetParent(transform);

        while (Vector3.Distance(transform.position, target) >= 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * moveSpeed);

            if (!IsPlayerOnPlatform())
                Player.Instance.transform.SetParent(null);

            else
                Player.Instance.transform.SetParent(transform);


            yield return null;
        }

        transform.position = target;

        Player.Instance.transform.SetParent(null);

        EndLeverAction();
    }

    private bool IsPlayerOnPlatform()
    {
        Collider[] colliders = Physics.OverlapBox(boxPivot.position, boxSize, Quaternion.identity, playerLayer);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
                return true;
        }

        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxPivot.position, boxSize);
    }
}
