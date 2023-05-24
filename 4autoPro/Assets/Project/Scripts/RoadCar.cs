using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadCar : MonoBehaviour
{
    public ChunkManager spawner;
    [SerializeField] private int damage = 1;
    [SerializeField] private bool damageTriggered = false;

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

    public void ResetCar()
    {
        damageTriggered = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponentInParent<PlayerStats>() && !damageTriggered)
        {
            GameManager.instance.OnGetDamage?.Invoke(damage);
            damageTriggered = true;
        }
    }

}
