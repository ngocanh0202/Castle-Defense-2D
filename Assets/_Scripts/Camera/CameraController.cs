using UnityEngine;
using Common2D;

public class CameraController : Singleton<CameraController>
{
    [Header("Camera Settings")]
    [SerializeField] private float panSpeed = 0.5f;
    [SerializeField] private float zoomSpeed = 0.5f;
    [SerializeField] private float smoothTime = 0.1f;

    [Header("Zoom Limits")]
    [SerializeField] private float minZoom = 3f;
    [SerializeField] private float maxZoom = 15f;

    [Header("Map Boundaries")]
    [SerializeField] private Vector2 minBounds;
    [SerializeField] private Vector2 maxBounds;

    [Header("Current State")]
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private float targetZoom;

    private Vector3 velocity;
    private float zoomVelocity;
    private bool isDragging;
    private Vector3 lastTouchPosition;
    private float lastTouchDistance;

    protected override void Awake()
    {
        base.Awake();
        targetPosition = transform.position;
        Camera cam = Camera.main;
        if (cam != null)
        {
            targetZoom = cam.orthographicSize;
        }
        else
        {
            targetZoom = 5f;
        }
    }

    void Start()
    {
        InitializeBounds();
    }

    void Update()
    {
        HandleTouchInput();
        MoveCamera();
    }

    private void InitializeBounds()
    {
        if (minBounds == Vector2.zero && maxBounds == Vector2.zero)
        {
            GameObject terrain = GameObject.Find("Terrain");
            if (terrain != null)
            {
                SpriteRenderer sr = terrain.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    Bounds bounds = sr.bounds;
                    minBounds = new Vector2(bounds.min.x, bounds.min.y);
                    maxBounds = new Vector2(bounds.max.x, bounds.max.y);
                }
            }
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    isDragging = true;
                    lastTouchPosition = touch.position;
                    break;

                case TouchPhase.Moved:
                    if (isDragging)
                    {
                        Camera cam = Camera.main;
                        if (cam != null)
                        {
                            Vector2 delta = touch.position - lastTouchPosition.ChangeToVector2();
                            float zoomFactor = cam.orthographicSize / 10f;
                            Vector3 move = new Vector3(-delta.x, -delta.y, 0) * panSpeed * zoomFactor * Time.deltaTime;
                            targetPosition += move;
                            lastTouchPosition = touch.position;
                        }
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    isDragging = false;
                    break;
            }
        }
        else if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            float currentDistance = Vector2.Distance(touch1.position, touch2.position);

            if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
            {
                float zoomDelta = (lastTouchDistance - currentDistance) * zoomSpeed * Time.deltaTime;
                targetZoom = Mathf.Clamp(targetZoom + zoomDelta, minZoom, maxZoom);
            }

            lastTouchDistance = currentDistance;
        }
    }

    private void MoveCamera()
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        Vector3 clampedPosition = new Vector3(
            Mathf.Clamp(targetPosition.x, minBounds.x, maxBounds.x),
            Mathf.Clamp(targetPosition.y, minBounds.y, maxBounds.y),
            targetPosition.z
        );

        float clampedZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);

        transform.position = Vector3.SmoothDamp(transform.position, clampedPosition, ref velocity, smoothTime);

        if (cam != null)
        {
            cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, clampedZoom, ref zoomVelocity, smoothTime);
        }
    }

    public void SetBounds(Vector2 min, Vector2 max)
    {
        minBounds = min;
        maxBounds = max;
    }

    public void FocusOnPosition(Vector3 position)
    {
        targetPosition = new Vector3(position.x, position.y, transform.position.z);
    }
}
