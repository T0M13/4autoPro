using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    PlayerReferences playerReferences;
    [Header("Player Settings")]
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private float laneSwitchForce = 2.5f;
    [SerializeField] private float interpolationFactor = 8f;
    [SerializeField] private Lane currentLane = Lane.Middle;
    [Header("Lane Settings")]
    [SerializeField] private float laneXDistance = 4f;
    [SerializeField] private int lanes = 3;
    [Header("Lane Settings")]
    [SerializeField] private bool showGizmos = true;
    [SerializeField] private float laneSwitchRotation = 10f;
    [SerializeField] private float rotationSpeed = 5f;
    [Header("Variables")]
    private Vector3 pos;
    private Quaternion rotation;
    private Vector3 right;

    private Coroutine coroutine;

    [Header("Events")]
    public UnityEvent swipeRight;
    public UnityEvent swipeLeft;

    private void Awake()
    {
        playerReferences = GetComponent<PlayerReferences>();
        currentLane = Lane.Middle;
    }

    private void OnEnable()
    {
        InputManager.instance.swipeDetector.OnSwipeLeft += SwipeLeft;
        InputManager.instance.swipeDetector.OnSwipeRight += SwipeRight;

    }

    private void OnDisable()
    {
        InputManager.instance.swipeDetector.OnSwipeLeft -= SwipeLeft;
        InputManager.instance.swipeDetector.OnSwipeRight -= SwipeRight;


    }

    private void SwipeLeft()
    {
        if (playerReferences.PlayerStats.Exploded || !playerReferences.PlayerStats.CanUseLogic) return;
        if (currentLane == Lane.Middle)
        {
            currentLane = Lane.Left;
            swipeLeft?.Invoke();
            if (coroutine != null)
                StopCoroutine(coroutine);
            coroutine = StartCoroutine(RotateVehicle(-laneSwitchRotation));
        }
        else if (currentLane == Lane.Right)
        {
            currentLane = Lane.Middle;
            swipeLeft?.Invoke();
            if (coroutine != null)
                StopCoroutine(coroutine);
            coroutine = StartCoroutine(RotateVehicle(-laneSwitchRotation));
        }
    }

    private void SwipeRight()
    {
        if (playerReferences.PlayerStats.Exploded || !playerReferences.PlayerStats.CanUseLogic) return;

        if (currentLane == Lane.Middle)
        {
            currentLane = Lane.Right;
            swipeRight?.Invoke();
            if (coroutine != null)
                StopCoroutine(coroutine);
            coroutine = StartCoroutine(RotateVehicle(laneSwitchRotation));
        }
        else if (currentLane == Lane.Left)
        {
            currentLane = Lane.Middle;
            swipeRight?.Invoke();

            if (coroutine != null)
                StopCoroutine(coroutine);
            coroutine = StartCoroutine(RotateVehicle(laneSwitchRotation));
        }
    }

    private IEnumerator RotateVehicle(float targetAngle)
    {
        Quaternion initialRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, targetAngle, transform.rotation.eulerAngles.z);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * rotationSpeed;
            transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, t);
            yield return null;
        }

        transform.rotation = targetRotation;


        StartCoroutine(RotateVehicleBack(0));
    }

    private IEnumerator RotateVehicleBack(float targetAngle)
    {

        Quaternion initialRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, targetAngle, transform.rotation.eulerAngles.z);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * rotationSpeed;
            transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, t);
            yield return null;
        }

        transform.rotation = targetRotation;
    }


    private void Update()
    {
        if (playerReferences.PlayerStats.Exploded || !playerReferences.PlayerStats.CanUseLogic) return;

        pos = transform.position;
        rotation = transform.rotation;
        right = rotation * Vector3.right;

        pos = startPosition + (laneXDistance * (float)currentLane) * right;
        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * laneSwitchForce * interpolationFactor);
    }

    private void OnDrawGizmos()
    {
        if (!showGizmos) return;
        Gizmos.color = Color.green;

        if (!Application.isPlaying)
        {
            rotation = transform.rotation;
            right = rotation * Vector3.right;
        }

        for (int i = 0; i < lanes; i++)
        {
            Vector3 pos = startPosition + (-laneXDistance + laneXDistance * i) * right;
            Gizmos.DrawSphere(pos, 0.2f);
        }
    }
}
