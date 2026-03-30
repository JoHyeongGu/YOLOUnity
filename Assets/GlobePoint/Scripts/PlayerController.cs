using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float smooth = 5f;
    public float xScale = 3f;

    private Rigidbody rb;
    private Vector3 targetPosition;
    private Vector3 velocity;
    private Vector3 origin;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        origin = transform.position;
        targetPosition = transform.position;
    }

    public void SetTargetPosition(Vector3 pos)
    {
        Vector3 offset = pos - origin;

        offset.x = -offset.x * xScale;

        targetPosition = origin + offset;
    }

    void FixedUpdate()
    {
        Vector3 pos = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 1f / smooth);
        rb.MovePosition(pos);
    }
}