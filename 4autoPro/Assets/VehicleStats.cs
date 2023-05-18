using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleStats : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private VehicleColor vehicleColor;
    [Header("Vehicle Preset")]
    [SerializeField] private MeshRenderer presetMeshRenderer;


    private void Start()
    {
        presetMeshRenderer.materials[1] = vehicleColor.currentMaterial;
    }

}
