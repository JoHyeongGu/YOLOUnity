using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Globepoint.TaekwonSlime;

public class DetectorPose : MonoBehaviour
{
    private const int TARGET_WIDTH = 640;
    private const int TARGET_HEIGHT = 640;

    public FileLoader fileLoader;

    // Object Detection
    private Unity.InferenceEngine.ModelAsset modelAsset;
    private Drawable screen;
    private Unity.InferenceEngine.Model runtimeModel;
    private Unity.InferenceEngine.Worker worker;
    private YoloPose yolo;
    private Source source = null;

    [SerializeField] private GP_YoloManager bridge;

    void Start()
    {
        // Initialise Classes
        yolo = new YoloPose();
        modelAsset = Resources.Load<Unity.InferenceEngine.ModelAsset>("Models/yolo11n-pose");
        runtimeModel = Unity.InferenceEngine.ModelLoader.Load(modelAsset);
        worker = new Unity.InferenceEngine.Worker(runtimeModel, Unity.InferenceEngine.BackendType.GPUCompute);

        fileLoader.OnSourceDetected += OnSourceChanged;
    }

    void Update()
    {
        if (source == null || source.IsProcessedOnce())
            return;

        if (source.IsFrameReady())
        {
            DetectFrame();
        }
    }

    private void OnDisable()
    {
        worker.Dispose();
    }

    public void StartDetection(float cTh, float iouTh, RawImage monitor)
    {
        yolo.IouThreshold = iouTh;
        yolo.ConfidenceThreshold = cTh;

        screen = new Drawable(monitor);

        if (source.IsProcessedOnce())
        {
            DetectFrame();
        }
        else
        {
            source.Play();
        }
    }
    void OnSourceChanged(SourceType sourceType, string path)
    {
        if (sourceType == SourceType.ImageSource)
        {
            source = new ImageSource(path);
        }
        else if (sourceType == SourceType.VideoSource)
        {
            source = new VideoSource(path);
        }
        else
        {
            source = new CameraSource();
        }
        Debug.Log($"Set Source!! {source}");
    }
    private void DetectFrame()
    {
        // Get the newly generated texture
        Texture texture = source.GetTexture();

        // Remove the old bounding boxes
        screen.ResetBoundingBoxes();

        // Display the texture
        screen.SetTexture(texture);

        // Prepare the input tensor
        Unity.InferenceEngine.Tensor<float> inputTensor = Unity.InferenceEngine.TextureConverter.ToTensor(texture, TARGET_WIDTH, TARGET_HEIGHT, 3);

        // Run the model on the input
        worker.Schedule(inputTensor);

        // Get output tensor
        Unity.InferenceEngine.Tensor<float> outputTensor = worker.PeekOutput() as Unity.InferenceEngine.Tensor<float>;

        // Process Model Output
        List<YoloPosePrediction> predictions = yolo.Predict(outputTensor, TARGET_WIDTH, TARGET_HEIGHT);

        if (predictions.Count > 0)
        {
            YoloPosePrediction firstPredict = predictions[0];
            bridge.UpdatePlayerPosition(firstPredict.BoundingBox);
        }

        // Draw the new bounding boxes 
        // screen.DrawBoundingBoxes(predictions);

        // if (predictions.Count != 0)
        // {
        //     Debug.Log("Landmark 1 x: " + predictions[0].Landmarks[0]);
        //     Debug.Log("Landmark 1 y: " + predictions[0].Landmarks[1]);
        //     Debug.Log("Landmark 1 score: " + predictions[0].Landmarks[2]);
        // }

        // Dispose tensors
        outputTensor.Dispose();
        inputTensor.Dispose();
    }
}


