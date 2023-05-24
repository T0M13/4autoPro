using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    [Header("Chunk")]
    [SerializeField] private GameObject[] chunks;
    [SerializeField] private List<GameObject> chunkList;
    [SerializeField] private int chunkAmount;
    [SerializeField] private ChunkDirectionType chunkDirection;
    [SerializeField] private int chunkMoveSpeed = 2;
    [SerializeField] private Vector3 chunkMoveDirection = new Vector3(0, 0, 1);
    [SerializeField] private float chunkSize = 10;
    [SerializeField] private float chunkDestoryZone = 15;
    [Header("Chunk Gizmos")]
    [SerializeField] private bool showChunkGizmos;
    [SerializeField] private Vector3 gizmosScale = Vector3.one;
    [Header("Car")]
    [SerializeField] private GameObject[] cars;
    [SerializeField] private List<GameObject> carList;
    [SerializeField] private int carAmount;
    [SerializeField] private int carMoveSpeed = 2;
    [SerializeField] private float carSpawnRateMin;
    [SerializeField] private float carSpawnRateMax;
    [SerializeField] private float carSpawnTimer;
    [Header("Car Lane Settings")]
    [SerializeField] private float laneXDistance = 3.5f;
    [SerializeField] private int lanes = 3;
    [SerializeField] private Lane lastLane;
    [SerializeField] private Vector3 lanesOffset;
    [Header("Car Gizmos")]
    [SerializeField] private bool showCarGizmos;

    private GameObject lastChunk;

    public int ChunkMoveSpeed { get => chunkMoveSpeed; set => chunkMoveSpeed = value; }
    public Vector3 ChunkMoveDirection { get => chunkMoveDirection; set => chunkMoveDirection = value; }
    public ChunkDirectionType ChunkDirection { get => chunkDirection; set => chunkDirection = value; }
    public float DestoryZone { get => chunkDestoryZone; set => chunkDestoryZone = value; }
    public int CarMoveSpeed { get => carMoveSpeed; set => carMoveSpeed = value; }

    private void Awake()
    {
        InitializeChunks();
        InitializeCars();
    }

    private void OnValidate()
    {
        SwitchDirection();
    }

    private void Update()
    {
        carSpawnTimer += Time.deltaTime;

        if (carSpawnTimer > Random.Range(carSpawnRateMin, carSpawnRateMax))
        {
            carSpawnTimer = 0;
            SpawnCar();
        }
    }

    private void SwitchDirection()
    {
        for (int i = 0; i < chunkList.Count; i++)
        {
            GameObject chunk = chunkList[i];

            switch (ChunkDirection)
            {
                case ChunkDirectionType.XPositive:
                    chunk.transform.localPosition = new Vector3(-i * chunkSize, 0, transform.position.z);
                    ChunkMoveDirection = new Vector3(1, 0, 0);
                    break;

                case ChunkDirectionType.XNegative:
                    chunk.transform.localPosition = new Vector3(i * chunkSize, 0, transform.position.z);
                    ChunkMoveDirection = new Vector3(-1, 0, 0);
                    break;

                case ChunkDirectionType.ZPositive:
                    chunk.transform.localPosition = new Vector3(transform.position.x, 0, -i * chunkSize);
                    ChunkMoveDirection = new Vector3(0, 0, 1);
                    break;

                case ChunkDirectionType.ZNegative:
                    chunk.transform.localPosition = new Vector3(transform.position.x, 0, i * chunkSize);
                    ChunkMoveDirection = new Vector3(0, 0, -1);
                    break;
            }
        }
    }

    private void InitializeChunks()
    {
        int chunkIndex = 0;
        for (int i = 0; i < chunkAmount; i++)
        {
            GameObject chunk = (GameObject)Instantiate(chunks[chunkIndex]);
            chunkList.Add(chunk);
            chunk.transform.SetParent(transform, false);
            chunk.SetActive(true);

            chunk.GetComponent<RoadChunk>().spawner = this;

            switch (ChunkDirection)
            {
                case ChunkDirectionType.XPositive:
                    chunk.transform.localPosition = new Vector3(-i * chunkSize, 0, transform.position.z);
                    ChunkMoveDirection = new Vector3(1, 0, 0);
                    break;

                case ChunkDirectionType.XNegative:
                    chunk.transform.localPosition = new Vector3(i * chunkSize, 0, transform.position.z);
                    ChunkMoveDirection = new Vector3(-1, 0, 0);
                    break;

                case ChunkDirectionType.ZPositive:
                    chunk.transform.localPosition = new Vector3(transform.position.x, 0, -i * chunkSize);
                    ChunkMoveDirection = new Vector3(0, 0, 1);
                    break;

                case ChunkDirectionType.ZNegative:
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
            case ChunkDirectionType.XPositive:
                newPos.x -= chunkSize;
                break;

            case ChunkDirectionType.XNegative:
                newPos.x += chunkSize;
                break;

            case ChunkDirectionType.ZPositive:
                newPos.z -= chunkSize;
                break;

            case ChunkDirectionType.ZNegative:
                newPos.z += chunkSize;
                break;
        }

        lastChunk = thisChunk.gameObject;
        lastChunk.transform.position = newPos;
    }

    public void SpawnCar()
    {
        GameObject randomCar = RandomCar();

        if (randomCar == null) return;

        randomCar.transform.position = SetRandomCarLane();

        randomCar.SetActive(true);
    }

    private GameObject RandomCar()
    {
        int temp = Random.Range(0, carList.Count);
        if (!carList[temp].activeInHierarchy)
        {
            return carList[temp];
        }
        else
        {
            return RandomCar();
        }
    }

    private Vector3 SetRandomCarLane()
    {
        Vector3 spawnPos = Vector3.zero;
        Vector3 temp;
        int lane = Random.Range(0, lanes);

        switch (chunkDirection)
        {
            case ChunkDirectionType.XPositive:
                temp = transform.position + new Vector3(-chunkSize, 0, 0) * chunkAmount;
                spawnPos = temp + lanesOffset + (-laneXDistance + laneXDistance * lane) * new Vector3(0, 0, 1);
                break;
            case ChunkDirectionType.XNegative:
                temp = transform.position + new Vector3(chunkSize, 0, 0) * chunkAmount;
                spawnPos = temp + lanesOffset + (-laneXDistance + laneXDistance * lane) * new Vector3(0, 0, -1);
                break;
            case ChunkDirectionType.ZPositive:
                temp = transform.position + new Vector3(0, 0, -chunkSize) * chunkAmount;
                spawnPos = temp + lanesOffset + (-laneXDistance + laneXDistance * lane) * new Vector3(1, 0, 0);
                break;
            case ChunkDirectionType.ZNegative:
                temp = transform.position + new Vector3(0, 0, chunkSize) * chunkAmount;
                spawnPos = temp + lanesOffset + (-laneXDistance + laneXDistance * lane) * new Vector3(-1, 0, 0);
                break;
        }

        return spawnPos;
    }

    public void DespawnCar(RoadCar thisCar)
    {
        thisCar.gameObject.SetActive(false);
        thisCar.ResetCar();
    }

    private void InitializeCars()
    {
        int chunkIndex = 0;
        for (int i = 0; i < carAmount; i++)
        {
            GameObject car = (GameObject)Instantiate(cars[chunkIndex]);
            carList.Add(car);
            car.transform.Rotate(Vector3.up * 180);
            car.transform.SetParent(transform, false);
            car.SetActive(false);

            car.GetComponent<RoadCar>().spawner = this;

            switch (ChunkDirection)
            {
                case ChunkDirectionType.XPositive:
                    car.transform.localPosition = new Vector3(-i * chunkSize, 0, transform.position.z);
                    ChunkMoveDirection = new Vector3(1, 0, 0);
                    break;

                case ChunkDirectionType.XNegative:
                    car.transform.localPosition = new Vector3(i * chunkSize, 0, transform.position.z);
                    ChunkMoveDirection = new Vector3(-1, 0, 0);
                    break;

                case ChunkDirectionType.ZPositive:
                    car.transform.localPosition = new Vector3(transform.position.x, 0, -i * chunkSize);
                    ChunkMoveDirection = new Vector3(0, 0, 1);
                    break;

                case ChunkDirectionType.ZNegative:
                    car.transform.localPosition = new Vector3(transform.position.x, 0, i * chunkSize);
                    ChunkMoveDirection = new Vector3(0, 0, -1);
                    break;
            }

            if (++chunkIndex >= cars.Length)
                chunkIndex = 0;
        }
    }


    private void OnDrawGizmos()
    {
        ChunkGizmos();
        CarGizmos();
    }

    private void CarGizmos()
    {
        if (!showCarGizmos) return;
        Gizmos.color = Color.yellow;
        for (int i = 0; i < lanes; i++)
        {
            Vector3 pos;
            Vector3 spawnArea;
            switch (chunkDirection)
            {
                case ChunkDirectionType.XPositive:
                    spawnArea = transform.position + new Vector3(-chunkSize, 0, 0) * chunkAmount;
                    pos = spawnArea + lanesOffset + (-laneXDistance + laneXDistance * i) * new Vector3(0, 0, 1);
                    Gizmos.DrawSphere(pos, 0.2f);
                    break;
                case ChunkDirectionType.XNegative:
                    spawnArea = transform.position + new Vector3(chunkSize, 0, 0) * chunkAmount;
                    pos = spawnArea + lanesOffset + (-laneXDistance + laneXDistance * i) * new Vector3(0, 0, -1);
                    Gizmos.DrawSphere(pos, 0.2f);
                    break;
                case ChunkDirectionType.ZPositive:
                    spawnArea = transform.position + new Vector3(0, 0, -chunkSize) * chunkAmount;
                    pos = spawnArea + lanesOffset + (-laneXDistance + laneXDistance * i) * new Vector3(1, 0, 0);
                    Gizmos.DrawSphere(pos, 0.2f);
                    break;
                case ChunkDirectionType.ZNegative:
                    spawnArea = transform.position + new Vector3(0, 0, chunkSize) * chunkAmount;
                    pos = spawnArea + lanesOffset + (-laneXDistance + laneXDistance * i) * new Vector3(-1, 0, 0);
                    Gizmos.DrawSphere(pos, 0.2f);
                    break;
            }
        }
    }

    private void ChunkGizmos()
    {
        if (!showChunkGizmos) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, gizmosScale);

        Gizmos.color = Color.yellow;
        switch (chunkDirection)
        {
            case ChunkDirectionType.XPositive:
                Gizmos.DrawWireCube(transform.position + new Vector3(-chunkSize, 0, 0) * chunkAmount, gizmosScale);
                break;

            case ChunkDirectionType.XNegative:
                Gizmos.DrawWireCube(transform.position + new Vector3(chunkSize, 0, 0) * chunkAmount, gizmosScale);
                break;

            case ChunkDirectionType.ZPositive:
                Gizmos.DrawWireCube(transform.position + new Vector3(0, 0, -chunkSize) * chunkAmount, gizmosScale);
                break;

            case ChunkDirectionType.ZNegative:
                Gizmos.DrawWireCube(transform.position + new Vector3(0, 0, chunkSize) * chunkAmount, gizmosScale);
                break;
        }

        Gizmos.color = Color.red;
        switch (chunkDirection)
        {
            case ChunkDirectionType.XPositive:
                Gizmos.DrawWireCube(transform.position + new Vector3(chunkDestoryZone, 0, 0), gizmosScale);
                break;

            case ChunkDirectionType.XNegative:
                Gizmos.DrawWireCube(transform.position + new Vector3(-chunkDestoryZone, 0, 0), gizmosScale);
                break;

            case ChunkDirectionType.ZPositive:
                Gizmos.DrawWireCube(transform.position + new Vector3(0, 0, chunkDestoryZone), gizmosScale);
                break;

            case ChunkDirectionType.ZNegative:
                Gizmos.DrawWireCube(transform.position + new Vector3(0, 0, -chunkDestoryZone), gizmosScale);
                break;
        }
    }

    [System.Serializable]
    public enum ChunkDirectionType
    {
        XPositive,
        XNegative,
        ZPositive,
        ZNegative
    }
}



