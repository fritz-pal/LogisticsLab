using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

public class RailPreview : MonoBehaviour
{
    private bool show = true;
    public Direction rotation = Direction.NORTH;
    public float maxCurveAngle = 15f;
    public float maxCurveDistance = 4f;
    public GridManager gridManager;
    public LineRenderer line;
    private Vector2Int? firstPosition = null;
    private Direction? firstDirection = null;

    void Start()
    {   
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.enabled = false;
    }

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int gridPos = new Vector2Int(Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y));

        if (firstPosition != null)
        {
            line.SetPosition(1, transform.position);
            (float, float) angles = Track.GetAngles(firstDirection.Value, firstPosition.Value, gridPos, maxCurveAngle);
            if (Mathf.Abs(angles.Item1) <= angles.Item2 && gridPos != firstPosition && Vector2Int.Distance(firstPosition.Value, gridPos) <= maxCurveDistance)
            {
                show = true;
                float nearestAngle = Mathf.Round(angles.Item1 / 45) * 45;
                if (nearestAngle == 0 && angles.Item1 != 0)
                {
                    if (angles.Item1 > 0)
                    {
                        nearestAngle = 45;
                    }
                    else
                    {
                        nearestAngle = -45;
                    }
                }
                Vector2Int vector = GridManager.AngleToVector(nearestAngle);
                rotation = (Direction)(((int)GridManager.DirectionFromVector(vector) + (int)firstDirection.Value + 2) % 8);

                transform.position = new Vector3(gridPos.x, gridPos.y, 0);
                transform.rotation = Quaternion.Euler(0, 0, (int)rotation * 45);
            }
            else
            {   
                //calculate nearest possible position
                firstDirection = (Direction)(((int)firstDirection + 4) % 8); // reverse direction
              
            }
        } 
        else
        {
            line.enabled = false;
            show = true;
            transform.position = new Vector3(gridPos.x, gridPos.y, 0);
            transform.rotation = Quaternion.Euler(0, 0, (int)rotation * 45);
        }
        gameObject.GetComponent<SpriteRenderer>().enabled = show;
    }

    public void HandleRightClick(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0 && firstPosition == null)
        {
            int rot = ((int)rotation - 1) % 8;
            if (rot < 0) rot = 7;
            rotation = (Direction) rot;
        }
    }

    public void HandleLeftClick(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0)
        {
            Vector2Int secondPosition = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
            if (firstPosition != null)
            {
                Track track = new Track(firstPosition.Value, secondPosition, firstDirection.Value, rotation);
                gridManager.addTrack(track);
            }
            Debug.Log("Left click at " + secondPosition + " with rotation " + rotation);
            firstPosition = secondPosition;
            firstDirection = rotation;
            line.SetPosition(0, transform.position);
            line.enabled = true;
        }
    }

    public void HandleCancel(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0)
        {
            if (firstPosition != null)
            {
                firstPosition = null;
                firstDirection = null;
                line.enabled = false;
            }
        }
    }
}
