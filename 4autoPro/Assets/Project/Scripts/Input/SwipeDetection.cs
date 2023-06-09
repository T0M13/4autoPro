using System;
using System.Collections;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    private InputManager inputManager;

    [SerializeField] private float minimumDistance = .2f;
    [SerializeField] private float maximumTime = 1f;
    [SerializeField][Range(0f, 1f)] private float directionThreshold = .9f;
    [SerializeField] private GameObject trail;

    private Vector2 startPosition;
    private float startTime;
    private Vector2 endPosition;
    private float endTime;

    private Coroutine coroutine;

    public Action OnSwipeUp;
    public Action OnSwipeDown;
    public Action OnSwipeLeft;
    public Action OnSwipeRight;

    private void Awake()
    {
        inputManager = InputManager.instance;
    }

    private void OnEnable()
    {
        inputManager.OnStartTouch += SwipeStart;
        inputManager.OnEndTouch += SwipeEnd;

        inputManager.OnMovement += CheckDirection;

    }

    private void OnDisable()
    {
        inputManager.OnStartTouch -= SwipeStart;
        inputManager.OnEndTouch -= SwipeEnd;

        inputManager.OnMovement -= CheckDirection;

    }

    private void SwipeStart(Vector2 position, float time)
    {
        startPosition = position;
        startTime = time;

        if (trail == null) return;
        trail.SetActive(true);
        trail.transform.position = position;
        coroutine = StartCoroutine(Trail());
    }

    private IEnumerator Trail()
    {
        while (true)
        {
            trail.transform.position = inputManager.PrimaryPosition();
            yield return null;
        }
    }

    private void SwipeEnd(Vector2 position, float time)
    {
        if (trail != null)
        {
            trail.SetActive(false);
            StopCoroutine(coroutine);
        }
        endPosition = position;
        endTime = time;
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        if (Vector3.Distance(startPosition, endPosition) >= minimumDistance &&
            (endTime - startTime) <= maximumTime)
        {
            Debug.DrawLine(startPosition, endPosition, Color.red, 5f);
            Vector3 direction = endPosition - startPosition;
            Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;
            SwipeDirection(direction2D);
        }
    }

    private void SwipeDirection(Vector2 direction)
    {
        if (Vector2.Dot(Vector2.up, direction) > directionThreshold)
        {
            Debug.Log("Up");
            OnSwipeUp?.Invoke();
        }
        if (Vector2.Dot(Vector2.down, direction) > directionThreshold)
        {
            Debug.Log("Down");
            OnSwipeDown?.Invoke();
        }
        if (Vector2.Dot(Vector2.left, direction) > directionThreshold)
        {
            Debug.Log("Left");
            OnSwipeLeft?.Invoke();
        }
        if (Vector2.Dot(Vector2.right, direction) > directionThreshold)
        {
            Debug.Log("Right");
            OnSwipeRight?.Invoke();
        }
    }


    private void CheckDirection(Vector2 movement)
    {
        if (movement == Vector2.up)
        {
            Debug.Log("Up");
            OnSwipeUp?.Invoke();
        }
        if (movement == Vector2.down)
        {
            Debug.Log("Down");
            OnSwipeDown?.Invoke();
        }
        if (movement == Vector2.left)
        {
            Debug.Log("Left");
            OnSwipeLeft?.Invoke();
        }
        if (movement == Vector2.right)
        {
            Debug.Log("Right");
            OnSwipeRight?.Invoke();
        }
    }
}
