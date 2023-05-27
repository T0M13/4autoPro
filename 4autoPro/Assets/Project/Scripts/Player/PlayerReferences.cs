using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReferences : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerStats playerStats;
    [SerializeField] private PlayerVehicleStat vehicleStats;
    public PlayerController PlayerController { get => playerController; set => playerController = value; }
    public PlayerStats PlayerStats { get => playerStats; set => playerStats = value; }
    public PlayerVehicleStat VehicleStats { get => vehicleStats; set => vehicleStats = value; }

    private void OnValidate()
    {
        GetReferences();
    }

    private void Awake()
    {
        GetReferences();
    }

    private void GetReferences()
    {
        playerController = GetComponent<PlayerController>();
        playerStats = GetComponent<PlayerStats>();

        if (VehicleStats == null)
            Debug.LogWarning("Vehicle Stats missing!");
    }
}
