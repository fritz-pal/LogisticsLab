using System.Collections.Generic;
using System.Data;
using Unity.Collections;
using UnityEngine;

public class GridManager : MonoBehaviour
{

    public GameObject gridPrefab;
    public int width, height;
    public GameObject railPrefab;
    public static GridManager Instance { get; private set; }

    private Dictionary<Vector2Int, NodeGroup> nodeGroups = new Dictionary<Vector2Int, NodeGroup>();
    private List<Track> tracks = new List<Track>();

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        for (int x = -width/2; x < width/2; x++)
        {
            for (int y = -height/2; y < height/2; y++)
            {
                GameObject grid = Instantiate(gridPrefab, new Vector3(x, y, 0), Quaternion.identity);
                grid.transform.parent = transform;
            }
        }
    }

    public void addTrack(Track track)
    {
        tracks.Add(track);
    }

    public bool CreateNodeGroup(Vector2Int position, Direction alignment, Node[] nodes)
    {
        if (nodeGroups.ContainsKey(position))
        {
            if (nodeGroups[position].GetAlignment() == alignment)
            {
                foreach (Node node in nodes)
                {
                    nodeGroups[position].AddNode(node);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        NodeGroup nodeGroup = new NodeGroup(alignment, position);
        nodeGroups.Add(position, nodeGroup);
        foreach (Node node in nodes)
        {
            nodeGroup.AddNode(node);
        }
        Instantiate(railPrefab, new Vector3(position.x, position.y, 0), Quaternion.Euler(0, 0, (int)alignment * 45));
        return true;
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DebugGrid();
        }

        // draw line for all tracks
        foreach (Track track in tracks)
        {
            track.DrawTrack();
        }
    }

    void DebugGrid(){
        foreach (KeyValuePair<Vector2Int, NodeGroup> nodeGroup in nodeGroups)
        {
            Debug.Log("NodeGroup at " + nodeGroup.Key + " with alignment " + nodeGroup.Value.GetAlignment() + " has " + nodeGroup.Value.GetNodes());
        }

        foreach (Track track in tracks)
        {
            track.DebugTrack();
        }
    }
}
