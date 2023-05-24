using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RoadChunk : MonoBehaviour
{
    public ChunkManager spawner;
    [Header("Collider")]
    [SerializeField] private BoxCollider boxColl;
    [SerializeField] private Vector3 boxScale = Vector3.one;
    [SerializeField] private bool showGizmos;


    private void Update()
    {
        transform.Translate(spawner.ChunkMoveDirection * spawner.ChunkMoveSpeed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        switch (spawner.ChunkDirection)
        {
            case ChunkManager.ChunkDirectionType.XPositive:
                if (transform.position.x > spawner.DestoryZone)
                    spawner.RespawnChunk(this);
                break;

            case ChunkManager.ChunkDirectionType.XNegative:
                if (transform.position.x < -spawner.DestoryZone)
                    spawner.RespawnChunk(this);
                break;

            case ChunkManager.ChunkDirectionType.ZPositive:
                if (transform.position.z > spawner.DestoryZone)
                    spawner.RespawnChunk(this);
                break;

            case ChunkManager.ChunkDirectionType.ZNegative:
                if (transform.position.z < -spawner.DestoryZone)
                    spawner.RespawnChunk(this);
                break;
        }

    }

    private void OnValidate()
    {
        GetBoxCollider();
        if (boxColl != null)
            SetBoxCollider();
    }

    private void Awake()
    {
        GetBoxCollider();
    }

    private void GetBoxCollider()
    {
        boxColl = GetComponent<BoxCollider>();
    }

    private void SetBoxCollider()
    {
        boxColl.size = boxScale;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody == null) return;
        collision.rigidbody.AddForce(spawner.ChunkMoveDirection * spawner.ChunkMoveSpeed, ForceMode.Force);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.rigidbody == null) return;
        collision.rigidbody.AddForce(spawner.ChunkMoveDirection * spawner.ChunkMoveSpeed, ForceMode.Force);
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, boxScale);
    }

}
