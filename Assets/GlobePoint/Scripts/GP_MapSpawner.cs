using UnityEngine;

namespace Globepoint.TaekwonSlime
{
    public class GP_MapSpawner : MonoBehaviour
    {
        [Header("<color=yellow>Ground</color>")]
        [SerializeField] private GameObject groundField;
        [SerializeField] private float spawnCooltime = 0.5f;

        [Header("<color=yellow>Gap</color>")]
        [SerializeField] private float gapCooltime = 1f;
        [SerializeField] private float gapPercent = 0.1f;
        [SerializeField] private float gapLenMin = 0.2f;
        [SerializeField] private float gapLenMax = 2f;

        private float gapLen = 0f;
        private float gapPos = 0f;

        private void Start()
        {
            InvokeRepeating(nameof(SpawnGround), 0f, spawnCooltime);
            InvokeRepeating(nameof(ChangeGap), 0f, gapCooltime);
        }

        private void SpawnGround()
        {
            var field = Instantiate(groundField, transform);
            var fieldController = field.GetComponent<GP_GroundField>();
            fieldController.SetGap(gapLen, gapPos);
        }

        private void ChangeGap()
        {
            float len = Random.Range(gapLenMin, gapLenMax);
            Debug.Log(len);
            gapLen = Random.value < gapPercent ? len : 0f;

            float halfGap = gapLen * 0.5f;
            float fieldWidth = groundField.transform.localScale.x;

            float minCenter = -fieldWidth * 0.5f + halfGap;
            float maxCenter = fieldWidth * 0.5f - halfGap;

            gapPos = Random.Range(minCenter, maxCenter);
        }

    }
}