using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReferences : MonoBehaviour
{
    private PlayerController playerController;
    private PlayerStats playerStats;

    public PlayerController PlayerController { get => playerController; set => playerController = value; }
    public PlayerStats PlayerStats { get => playerStats; set => playerStats = value; }

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
    }
}
