using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Deactivater : MonoBehaviour
{
    private BoxCollider boxColl;
    [SerializeField] private Vector3 boxColliderSize = Vector3.one;

    private void OnValidate()
    {
        GetBoxCollider();
        SetBoxColliderSize();
    }

    private void Awake()
    {
        GetBoxCollider();
        SetBoxColliderSize();
    }

    private void GetBoxCollider()
    {
        boxColl = GetComponent<BoxCollider>();
    }

    private void SetBoxColliderSize()
    {
        if (boxColl != null)
            boxColl.size = boxColliderSize;
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Vector3.zero, boxColl ? boxColl.size : Vector3.one);
    }
}
