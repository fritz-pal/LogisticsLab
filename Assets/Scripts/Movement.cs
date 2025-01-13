using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public float zoomSpeed = 0.2f;
    public int maxZoom = 10;
    public int minZoom = 1;
    public Camera mainCamera;
    public GameObject railPreview;
    public GameObject stationPreview;
    public float movementSpeed = 0.5f;
    private float zoomLevel = 5;
    private Vector2 moveDirection;

    void Start()
    {
        mainCamera.orthographicSize = zoomLevel;
        railPreview.SetActive(false);
        stationPreview.SetActive(false);
    }

    void Update()
    {
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, zoomLevel, Time.deltaTime * 10);
        Vector3 move = new Vector3(moveDirection.x, moveDirection.y, 0);
        transform.position += move * movementSpeed * Time.deltaTime * zoomLevel;
    }

    public void HandleScrollWheel(InputAction.CallbackContext context)
    {
        Vector2 scrollValue = context.ReadValue<Vector2>();
        zoomLevel -= (int)scrollValue.y * zoomSpeed * zoomLevel;
        zoomLevel = Mathf.Clamp(zoomLevel, minZoom, maxZoom);
    }

    public void HandleMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
        moveDirection = Quaternion.Euler(0, 0, -45) * moveDirection;
    }

    public void HandleRailSelect(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0)
        {
            railPreview.SetActive(true);
            stationPreview.SetActive(false);
        }
    }

    public void HandleStationSelect(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0)
        {
            railPreview.SetActive(false);
            stationPreview.SetActive(true);
        }
    }
}