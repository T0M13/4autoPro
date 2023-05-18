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
        switch (spawner.ChunkDirectionO)
        {
            case ChunkManager.ChunkDirection.XPositive:
                if (transform.position.x > spawner.DestoryZone)
                    spawner.RespawnChunk(this);
                break;

            case ChunkManager.ChunkDirection.XNegative:
                if (transform.position.x < -spawner.DestoryZone)
                    spawner.RespawnChunk(this);
                break;

            case ChunkManager.ChunkDirection.ZPositive:
                if (transform.position.z > spawner.DestoryZone)
                    spawner.RespawnChunk(this);
                break;

            case ChunkManager.ChunkDirection.ZNegative:
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

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, boxScale);
    }
}
