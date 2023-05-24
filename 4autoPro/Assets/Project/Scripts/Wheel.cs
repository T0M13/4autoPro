using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    [Header("Rotation Speed")]
    [SerializeField] private float rotationSpeed = 1500;
    [Header("Turn Settings")]
    [SerializeField] private float turnAngle = 80f;
    [SerializeField] private float turnSpeed = 5f;

    private void Update()
    {
        transform.Rotate(Vector3.right, rotationSpeed * Time.deltaTime);
    }

    public void TurnWheelLeft()
    {
        StartCoroutine(TurnWheelCoroutine(Quaternion.Euler(0, -turnAngle, 0)));
    }

    public void TurnWheelRight()
    {
        StartCoroutine(TurnWheelCoroutine(Quaternion.Euler(0, turnAngle, 0)));
    }

    private IEnumerator TurnWheelCoroutine(Quaternion targetRotation)
    {
        Quaternion initialRotation = transform.localRotation;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * turnSpeed;
            transform.localRotation = Quaternion.Slerp(initialRotation, targetRotation, t);
            yield return null;
        }

        transform.localRotation = targetRotation;
        StartCoroutine(TurnWheelBackCoroutine(initialRotation));
    }

    private IEnumerator TurnWheelBackCoroutine(Quaternion initialRotation)
    {
        Quaternion targetRotation = Quaternion.identity;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * turnSpeed;
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, t);
            yield return null;
        }

        transform.localRotation = targetRotation;
    }
}
