using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class Movement : MonoBehaviour
{
    public GameObject gridManager;
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
        railPreview.SetActive(true);
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
            railPreview.GetComponent<RailPreview>().enabled = true;
        }
    }

    public void HandleStationSelect(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0)
        {
            railPreview.SetActive(false);
            stationPreview.SetActive(true);
            stationPreview.GetComponent<StationPreview>().enabled = true;
        }
    }

    public void HandleMouseModeSelect(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0)
        {
            railPreview.SetActive(false);
            stationPreview.SetActive(false);
        }
    }

    public void HandleLeftClickInMouseMode(InputAction.CallbackContext context)
    {
        if (!railPreview.activeSelf && !stationPreview.activeSelf)
        {
            gridManager.GetComponent<GridManager>().GetMousePosition(true);
        }
    }
}