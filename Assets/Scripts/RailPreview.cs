using UnityEngine;
using UnityEngine.InputSystem;

public class RailPreview : MonoBehaviour
{
    public bool show = true;
    public Direction rotation = Direction.NORTH;
    public GridManager gridManager;
    public LineRenderer line;
    private Vector2Int? firstPosition = null;
    void Start()
    {   
        gameObject.SetActive(show);
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.enabled = false;
    }

    void Update()
    {
        if (show) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(Mathf.Round(mousePos.x), Mathf.Round(mousePos.y), 0);
            transform.rotation = Quaternion.Euler(0, 0, (int)rotation * 45);
        }
        gameObject.SetActive(show);

        if (firstPosition != null)
        {
            line.SetPosition(1, transform.position);
        } 
        else
        {
            line.enabled = false;
        }
    }

    public void HandleRightClick(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0)
        {
            rotation = (Direction)(((int)rotation - 1) % 8);
        }
    }

    public void HandleLeftClick(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0)
        {
            if (firstPosition == null)
            {
                firstPosition = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
                line.SetPosition(0, transform.position);
                line.enabled = true;
            }
            else
            {
                Vector2Int secondPosition = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
                Track track = new Track(firstPosition.Value, secondPosition, rotation);
                gridManager.addTrack(track);
                firstPosition = null;
                line.enabled = false;
            }
        }
    }
}
