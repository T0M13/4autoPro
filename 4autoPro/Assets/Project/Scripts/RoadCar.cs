using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadCar : MonoBehaviour
{
    public ChunkManager spawner;
    [SerializeField] private int carIndex;
    [SerializeField] private RoadVehicleStats vehicleStats;
    [SerializeField] private int damage = 1;
    [SerializeField] private bool damageTriggered = false;
    [Header("Explosion")]
    [SerializeField] private ForceMode forceMode;
    [SerializeField] private float explodeForce;
    [SerializeField] private bool exploded = false;
    [SerializeField] private GameObject[] deleteObjects;
    [SerializeField] private GameObject[] explodeObjects;
    [SerializeField] private GameObject explosionEffect;

    public bool Exploded { get => exploded; set => exploded = value; }
    public int CarIndex { get => carIndex; set => carIndex = value; }

    private void Awake()
    {
        GetStats();
    }

    private void Update()
    {
        transform.Translate(-spawner.ChunkMoveDirection * spawner.CarMoveSpeed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        switch (spawner.ChunkDirection)
        {
            case ChunkManager.ChunkDirectionType.XPositive:
                if (transform.position.x > spawner.DestoryZone)
                    spawner.DespawnCar(this);
                break;

            case ChunkManager.ChunkDirectionType.XNegative:
                if (transform.position.x < -spawner.DestoryZone)
                    spawner.DespawnCar(this);
                break;

            case ChunkManager.ChunkDirectionType.ZPositive:
                if (transform.position.z > spawner.DestoryZone)
                    spawner.DespawnCar(this);
                break;

            case ChunkManager.ChunkDirectionType.ZNegative:
                if (transform.position.z < -spawner.DestoryZone)
                    spawner.DespawnCar(this);
                break;
        }

    }

    private void GetStats()
    {
        if (vehicleStats == null) return;
        damage = vehicleStats.damage;
    }

    public void ResetCar()
    {
        damageTriggered = false;
    }

    private void Explode()
    {
        if (Exploded) return;
        explosionEffect.SetActive(true);
        explosionEffect.transform.SetParent(null, true);
        foreach (GameObject deleteObject in deleteObjects)
        {
            deleteObject.SetActive(false);
        }

        foreach (GameObject explodeObject in explodeObjects)
        {
            Vector3 randomExplodeDirection = new Vector3(UnityEngine.Random.Range(-1, 1) * explodeForce, UnityEngine.Random.Range(2, 2.5f) * explodeForce, UnityEngine.Random.Range(-1, 1) * explodeForce);

            if (explodeObject.GetComponent<Rigidbody>())
            {
                Rigidbody body = explodeObject.GetComponent<Rigidbody>();
                body.AddForce(randomExplodeDirection, forceMode);
            }
            else
            {
                Rigidbody body = explodeObject.AddComponent<Rigidbody>();
                body.AddForce(randomExplodeDirection, forceMode);
            }

            if (explodeObject.GetComponent<Collider>())
            {
                Collider col = explodeObject.GetComponent<Collider>();
                col.enabled = true;
            }
            else
            {
                MeshCollider col = explodeObject.AddComponent<MeshCollider>();
                col.convex = true;
            }
        }

        Exploded = true;

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<PlayerStats>() && !damageTriggered)
        {
            if (!GameManager.instance.GameOver)
                Explode();
            var player = collision.gameObject.GetComponentInParent<PlayerStats>();
            if (player.Invincible) return;
            GameManager.instance.OnGetDamage?.Invoke(damage);
            damageTriggered = true;
        }
    }

}
