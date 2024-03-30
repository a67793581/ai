using UnityEngine;
using OpenCvSharp;
using UnityEngine.UI;

public class FaceDetection : MonoBehaviour
{
    public RawImage rawImage;
    public Transform emoji;
    private WebCamTexture _webcamTexture;
    private CascadeClassifier _faceDetector;

    private void Start()
    {
        // if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        // {
        //     Debug.LogError("***************************");
        //     _webcamTexture = new WebCamTexture("Camera 0");
        //     _faceDetector = new CascadeClassifier("Assets/OpenCV+Unity/Demo/Face_Detector/haarcascade_frontalface_default.xml");
        //     _webcamTexture.Play();
        //     rawImage.texture = _webcamTexture;
        // }
    }

    // private void OnApplicationFocus(bool hasFocus)
    // {
    //     if (hasFocus)
    //     {
    //         if (_webcamTexture.isPlaying == false)
    //         {
    //             _webcamTexture.Play();
    //         }
    //     }
    // }

    private void Update()
    {
        if (_webcamTexture.isPlaying == false)
        {
            _webcamTexture.Play();
        }

        // return;
        var frame = OpenCvSharp.Unity.TextureToMat(_webcamTexture);

        switch (Screen.orientation)
        {
            case ScreenOrientation.LandscapeLeft:
                Cv2.Rotate(frame, frame, RotateFlags.Rotate90Clockwise);
                break;
            case ScreenOrientation.LandscapeRight:
                Cv2.Rotate(frame, frame, RotateFlags.Rotate90CounterClockwise);
                break;
            // 添加其他方向的处理逻辑
        }

        var faces = _faceDetector.DetectMultiScale(frame);
        foreach (var face in faces)
        {
            Debug.LogError(face);
            var rotationX = Mathf.Lerp(-2, 2, face.X * 0.5f);
            var rotationY = Mathf.Lerp(-2, 2, face.Y * 0.5f);
            emoji.localPosition = new Vector2(rotationX, rotationY);
        }
        // OpenCvSharp.Unity.MatToTexture(frame);
    }
}