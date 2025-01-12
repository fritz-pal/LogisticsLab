using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

public class RailPreview : MonoBehaviour
{
    private bool show = true;
    public Direction rotation = Direction.NORTH;
    public float maxCurveAngle = 15f;
    public float maxCurveDistance = 4f;
    public AudioClip placeSound;
    public AudioClip errorSound;
    public GridManager gridManager;
    public GameObject previewSpline;
    private Spline spline = new();
    private Vector2Int? firstPosition = null;
    private Direction? firstDirection = null;

    void Start()
    {
        previewSpline.GetComponent<SplineInstantiate>().enabled = false;
        previewSpline.GetComponent<SplineContainer>().AddSpline(spline);
    }

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2Int gridPos = new Vector2Int(Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y));

        if (firstPosition != null)
        {
            previewSpline.GetComponent<SplineInstantiate>().enabled = true;
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
                
                BezierKnot knot = spline.ToArray()[1];
                knot.Position = new Vector3(gridPos.x, gridPos.y, 0);
                Vector2Int tangentVector = GridManager.VectorFromDirection(rotation);
                Vector3 tangent = new Vector3(tangentVector.x, 0, tangentVector.y) * -1;
                knot.TangentIn = tangent;
                knot.TangentOut = tangent;
                spline.SetKnot(1, knot);
                previewSpline.GetComponent<SplineInstantiate>().UpdateInstances();
            }
            else
            {   
                //calculate nearest possible position
                //firstDirection = (Direction)(((int)firstDirection + 4) % 8); <--- das funktioniert nicht, weil es manchmal random falsche richtungen gibt
              
            }
        } 
        else
        {
            previewSpline.GetComponent<SplineInstantiate>().enabled = false;
            spline.Clear();
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
                if (gridManager.CheckTrack(firstPosition.Value, secondPosition, firstDirection.Value, rotation))
                {
                    HandleError();
                    return;
                }
                Track track = new Track(firstPosition.Value, secondPosition, firstDirection.Value, rotation);
                gridManager.addTrack(track);
                AudioSource.PlayClipAtPoint(placeSound, Camera.main.transform.position);

                firstDirection = rotation;
            }
            else
            {
                if (gridManager.GetNodeGroup(secondPosition) != null)
                {
                    firstDirection = gridManager.GetNodeGroup(secondPosition).GetAlignment();
                }
                else
                {
                    firstDirection = rotation;
                }
            }
            Debug.Log("Left click at " + secondPosition + " with rotation " + rotation);
            firstPosition = secondPosition;
            Vector2Int tangentVector = GridManager.VectorFromDirection(firstDirection.Value);
            Vector3 tangent = new Vector3(tangentVector.x, 0, tangentVector.y);
            spline.Clear();
            spline.Add(new BezierKnot(new Vector3(secondPosition.x, secondPosition.y, 0), tangent, tangent, Quaternion.LookRotation(Vector3.up)));
            spline.Add(new BezierKnot(new Vector3(secondPosition.x, secondPosition.y, 0), -tangent, -tangent, Quaternion.LookRotation(Vector3.up)));
        }
    }

    public void HandleError()
    {
        AudioSource.PlayClipAtPoint(errorSound, Camera.main.transform.position);
    }

    public void HandleCancel(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0)
        {
            if (firstPosition != null)
            {
                firstPosition = null;
                firstDirection = null;
                spline.Clear();
                previewSpline.GetComponent<SplineInstantiate>().enabled = false;
            }
        }
    }
}
