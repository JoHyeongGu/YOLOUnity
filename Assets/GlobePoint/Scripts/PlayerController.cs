using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float smooth = 5f;
    public float xScale = 3f;
    public float widthSensitivity = 3f;

    private Rigidbody rb;
    private Vector3 targetPosition;
    private Vector3 velocity;
    private Vector3 origin;

    private float baseWidth;
    private float baseScaleX;
    private bool initialized;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        origin = transform.position;
        targetPosition = transform.position;
        baseScaleX = transform.localScale.x;
    }

    public void SetTargetPosition(Vector3 pos, float width)
    {
        Vector3 offset = pos - origin;
        offset.x = -offset.x * xScale;
        targetPosition = origin + offset;

        if (!initialized)
        {
            baseWidth = width;
            initialized = true;
        }

        float ratio = (width / baseWidth - 1f) * widthSensitivity + 1f;

        Vector3 scale = transform.localScale;
        scale.x = baseScaleX * ratio;
        transform.localScale = scale;
    }

    void FixedUpdate()
    {
        Vector3 pos = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, 1f / smooth);
        rb.MovePosition(pos);
    }
}