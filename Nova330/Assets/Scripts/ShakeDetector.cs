using UnityEngine;

public class ShakeDetector : MonoBehaviour
{
    public float shakeThreshold = 2.0f;
    private Vector3 _lastAcceleration;

    private void Start()
    {
        _lastAcceleration = Input.acceleration;
    }

    private void Update()
    {
        var accelerationDelta = Input.acceleration - _lastAcceleration;
        _lastAcceleration = Input.acceleration;
        
        var deltaMagnitude = accelerationDelta.magnitude;
        if (deltaMagnitude > shakeThreshold)
        {
            Debug.Log("Device shaken");
        }
    }
}