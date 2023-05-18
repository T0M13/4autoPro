using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Vehicle Color", menuName = "Vehicle/Vehicle Color")]
public class VehicleColor : ScriptableObject
{
    public Material[] materials;
    public Material currentMaterial;
}
