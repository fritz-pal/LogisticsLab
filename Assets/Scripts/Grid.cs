using UnityEngine;

public class Grid : MonoBehaviour
{

    public GameObject gridPrefab;
    public int width, height;
    void Start()
    {
        for (int x = -width/2; x < width/2; x++)
        {
            for (int y = -height/2; y < height/2; y++)
            {
                GameObject grid = Instantiate(gridPrefab, new Vector3(x, y, 0), Quaternion.identity);
                grid.transform.parent = transform;
            }
        }
    }
}
