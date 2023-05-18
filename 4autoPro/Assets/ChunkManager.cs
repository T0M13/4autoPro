using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    [SerializeField] private GameObject[] chunks;
    [SerializeField] private List<GameObject> chunkList;
    [SerializeField] private int chunkAmount;
    [SerializeField] private ChunkDirection chunkDirection;
    [SerializeField] private int chunkMoveSpeed = 2;
    [SerializeField] private Vector3 chunkMoveDirection = new Vector3(0, 0, 1);
    [SerializeField] private float chunkSize = 10;
    [SerializeField] private float destoryZone = 15;
    [Header("Gizmos")]
    [SerializeField] private bool showGizmos;
    [SerializeField] private Vector3 gizmosScale = Vector3.one;

    private GameObject lastChunk;

    public int ChunkMoveSpeed { get => chunkMoveSpeed; set => chunkMoveSpeed = value; }
    public Vector3 ChunkMoveDirection { get => chunkMoveDirection; set => chunkMoveDirection = value; }
    public ChunkDirection ChunkDirectionO { get => chunkDirection; set => chunkDirection = value; }
    public float DestoryZone { get => destoryZone; set => destoryZone = value; }

    private void Awake()
    {
        InitializeObjects();
    }

    private void OnValidate()
    {
        for (int i = 0; i < chunkList.Count; i++)
        {
            GameObject chunk = chunkList[i];

            switch (ChunkDirectionO)
            {
                case ChunkDirection.XPositive:
                    chunk.transform.localPosition = new Vector3(-i * chunkSize, 0, transform.position.z);
                    ChunkMoveDirection = new Vector3(1, 0, 0);
                    break;

                case ChunkDirection.XNegative:
                    chunk.transform.localPosition = new Vector3(i * chunkSize, 0, transform.position.z);
                    ChunkMoveDirection = new Vector3(-1, 0, 0);
                    break;

                case ChunkDirection.ZPositive:
                    chunk.transform.localPosition = new Vector3(transform.position.x, 0, -i * chunkSize);
                    ChunkMoveDirection = new Vector3(0, 0, 1);
                    break;

                case ChunkDirection.ZNegative:
                    chunk.transform.localPosition = new Vector3(transform.position.x, 0, i * chunkSize);
                    ChunkMoveDirection = new Vector3(0, 0, -1);
                    break;
            }
        }
    }

    private void InitializeObjects()
    {
        int chunkIndex = 0;
        for (int i = 0; i < chunkAmount; i++)
        {
            GameObject chunk = (GameObject)Instantiate(chunks[chunkIndex]);
            chunkList.Add(chunk);
            chunk.transform.SetParent(transform, false);
            chunk.SetActive(true);

            chunk.GetComponent<RoadChunk>().spawner = this;

            switch (ChunkDirectionO)
            {
                case ChunkDirection.XPositive:
                    chunk.transform.localPosition = new Vector3(-i * chunkSize, 0, transform.position.z);
                    ChunkMoveDirection = new Vector3(1, 0, 0);
                    break;

                case ChunkDirection.XNegative:
                    chunk.transform.localPosition = new Vector3(i * chunkSize, 0, transform.position.z);
                    ChunkMoveDirection = new Vector3(-1, 0, 0);
                    break;

                case ChunkDirection.ZPositive:
                    chunk.transform.localPosition = new Vector3(transform.position.x, 0, -i * chunkSize);
                    ChunkMoveDirection = new Vector3(0, 0, 1);
                    break;

                case ChunkDirection.ZNegative:
                    chunk.transform.localPosition = new Vector3(transform.position.x, 0, i * chunkSize);
                    ChunkMoveDirection = new Vector3(0, 0, -1);
                    break;
            }


            lastChunk = chunk;

            if (++chunkIndex >= chunks.Length)
                chunkIndex = 0;
        }

    }

    public void RespawnChunk(RoadChunk thisChunk)
    {
        Vector3 newPos = lastChunk.transform.position;
        switch (chunkDirection)
        {
            case ChunkDirection.XPositive:
                newPos.x -= chunkSize;
                break;

            case ChunkDirection.XNegative:
                newPos.x += chunkSize;
                break;

            case ChunkDirection.ZPositive:
                newPos.z -= chunkSize;
                break;

            case ChunkDirection.ZNegative:
                newPos.z += chunkSize;
                break;
        }

        lastChunk = thisChunk.gameObject;
        lastChunk.transform.position = newPos;
    }


    private void OnDrawGizmos()
    {
        if (!showGizmos) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, gizmosScale);

        Gizmos.color = Color.yellow;
        switch (chunkDirection)
        {
            case ChunkDirection.XPositive:
                Gizmos.DrawWireCube(transform.position + new Vector3(-chunkSize, 0, 0) * chunkAmount, gizmosScale);
                break;

            case ChunkDirection.XNegative:
                Gizmos.DrawWireCube(transform.position + new Vector3(chunkSize, 0, 0) * chunkAmount, gizmosScale);
                break;

            case ChunkDirection.ZPositive:
                Gizmos.DrawWireCube(transform.position + new Vector3(0, 0, -chunkSize) * chunkAmount, gizmosScale);
                break;

            case ChunkDirection.ZNegative:
                Gizmos.DrawWireCube(transform.position + new Vector3(0, 0, chunkSize) * chunkAmount, gizmosScale);
                break;
        }

        Gizmos.color = Color.red;
        switch (chunkDirection)
        {
            case ChunkDirection.XPositive:
                Gizmos.DrawWireCube(transform.position + new Vector3(destoryZone, 0, 0), gizmosScale);
                break;

            case ChunkDirection.XNegative:
                Gizmos.DrawWireCube(transform.position + new Vector3(-destoryZone, 0, 0), gizmosScale);
                break;

            case ChunkDirection.ZPositive:
                Gizmos.DrawWireCube(transform.position + new Vector3(0, 0, destoryZone), gizmosScale);
                break;

            case ChunkDirection.ZNegative:
                Gizmos.DrawWireCube(transform.position + new Vector3(0, 0, -destoryZone), gizmosScale);
                break;
        }
    }

    [System.Serializable]
    public enum ChunkDirection
    {
        XPositive,
        XNegative,
        ZPositive,
        ZNegative
    }
}



