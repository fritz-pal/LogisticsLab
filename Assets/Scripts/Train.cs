using UnityEngine;

public class Train : MonoBehaviour
{

    public NodeGroup nodeGroup;
    public float speed = 1.0f;
    private bool isMoving = false;


    void Update()
    {
        if (!isMoving)
        {
            if (nodeGroup == null) return;
            Vector2Int position2D = nodeGroup.GetPosition();
            gameObject.transform.position = new Vector3(position2D.x, position2D.y, 0);
            gameObject.transform.rotation = Quaternion.Euler(0, 0, (int)nodeGroup.GetAlignment() * 45);
        }
    }
}
