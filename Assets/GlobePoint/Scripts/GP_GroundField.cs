using UnityEngine;

namespace Globepoint.TaekwonSlime
{
    public class GP_GroundField : MonoBehaviour
    {
        public float speed = 10f;

        [SerializeField] private Transform leftGround;
        [SerializeField] private Transform rightGround;

        private readonly float deadline = -15f;
        
        public void SetGap(float gap, float gapCenter)
        {
            float parentWidth = transform.localScale.x;
            float halfGap = gap * 0.5f;

            float leftEdge = gapCenter - halfGap;
            float rightEdge = gapCenter + halfGap;

            float leftWidth = leftEdge + parentWidth * 0.5f;
            float rightWidth = parentWidth * 0.5f - rightEdge;

            leftGround.localScale = new Vector3(leftWidth, leftGround.localScale.y, leftGround.localScale.z);
            rightGround.localScale = new Vector3(rightWidth, rightGround.localScale.y, rightGround.localScale.z);

            leftGround.localPosition = new Vector3(-parentWidth * 0.5f + leftWidth * 0.5f, 0, 0);
            rightGround.localPosition = new Vector3(rightEdge + rightWidth * 0.5f, 0, 0);
        }

        private void Update()
        {
            transform.position += Vector3.back * speed * Time.deltaTime;

            if (transform.position.z < deadline)
                Destroy(gameObject);
        }
    }
}