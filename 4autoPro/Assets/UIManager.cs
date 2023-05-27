using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreUI;
    public TextMeshProUGUI TimeScoreUI { get => scoreUI; set => scoreUI = value; }
}
