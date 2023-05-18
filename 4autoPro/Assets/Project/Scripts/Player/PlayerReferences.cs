using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReferences : MonoBehaviour
{

    private PlayerController playerController;

    public PlayerController PlayerController { get => playerController; set => playerController = value; }

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
    }
}