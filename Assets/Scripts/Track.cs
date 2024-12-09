using UnityEngine;

public class Track : MonoBehaviour
{
    public bool hasSignal = false;
    public Direction direction;

    public Track(Direction direction)
    {
        this.direction = direction;
    }
}

public enum Direction
{
    NORTH,
    NORTHWEST,
    WEST,
    SOUTHWEST,
    SOUTH,
    SOUTHEAST,
    EAST,
    NORTHEAST
}