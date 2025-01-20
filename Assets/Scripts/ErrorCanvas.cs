using UnityEngine;

public class ErrorCanvas : MonoBehaviour
{
    public new Camera camera;

    void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        transform.LookAt(transform.position + camera.transform.rotation * Vector3.back, camera.transform.rotation * Vector3.up);
        transform.Rotate(0, 180, 0);
    }
}
