using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Globepoint.TaekwonSlime
{
    public class GP_YoloManager : MonoBehaviour
    {
        [Header("<color=yellow>YOLO Detector</color>")]
        [SerializeField] private DetectorPose detector;
        [SerializeField] private FileLoader fileLoader;
        [SerializeField] private float confidence = 0.5f;
        [SerializeField] private float iou = 0.5f;
        [SerializeField] private GP_PlayerController player;

        [Header("<color=yellow>UI</color>")]
        [SerializeField] private Button detectButton;
        [SerializeField] private RawImage monitor;

        private float mapWidth = 8f;
        private float mapHeight = 8f;

        private float cameraWidth = 640f;
        private float cameraHeight = 480f;

        private void Start()
        {
            detectButton.onClick.AddListener(DetectButtonClick);
            monitor.gameObject.SetActive(false);
        }

        private void DetectButtonClick()
        {
            fileLoader.SetDefaultFilter(SourceType.CameraSource);
            detector.StartDetection(confidence, iou, monitor);
            monitor.gameObject.SetActive(true);
        }

        public void UpdatePlayerPosition(Rect bbox)
        {
            float centerX = bbox.x + bbox.width * 0.5f;
            float centerY = bbox.y + bbox.height * 0.5f;

            float nx = centerX / cameraWidth;
            float ny = centerY / cameraHeight;

            float worldX = (nx - 0.5f) * mapWidth;
            float worldZ = (ny - 0.5f) * mapHeight;

            Vector3 target = new Vector3(worldX, 0.5f, worldZ);

            player.SetTargetPosition(target, bbox.width);
        }
    }
}