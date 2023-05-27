using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private PlayerReferences playerReferences;
    [SerializeField] private bool invincible;
    [SerializeField] private bool canUseLogic = true;
    [Header("Explosion")]
    [SerializeField] private ForceMode forceMode;
    [SerializeField] private float explodeForce;
    [SerializeField] private bool exploded = false;
    [SerializeField] private GameObject[] deleteObjects;
    [SerializeField] private GameObject[] explodeObjects;
    [SerializeField] private GameObject explosionEffect;
    private ChunkManager spawner;

    public Action OnExplode;

    public bool Exploded { get => exploded; set => exploded = value; }
    public bool CanUseLogic { get => canUseLogic; set => canUseLogic = value; }

    private void Awake()
    {
        playerReferences = GetComponent<PlayerReferences>();
    }

    private void OnEnable()
    {
        OnExplode += Explode;
    }

    private void OnDisable()
    {
        OnExplode -= Explode;
    }


    private void Explode()
    {
        if (invincible) return;
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

        GameManager.instance.OnGameOver?.Invoke();

        Exploded = true;

    }

}
