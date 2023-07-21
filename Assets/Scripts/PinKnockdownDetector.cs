using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinKnockdownDetector : MonoBehaviour
{
    public int pinIndex;
    public Vector3 startPosition;
    public Quaternion startRotation;
    private bool isInitialized = false;

    private void Awake()
    {
        pinIndex = transform.GetSiblingIndex();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!isInitialized)
        {
            startPosition = transform.GetChild(0).localPosition;
            startRotation = Quaternion.Euler(0, 0, 0);
            isInitialized = true;
        }
    }

    public bool IsUpright()
    {
        // Calculate the angle between the pin's up direction and the vertical (upright) direction
        float angle = Quaternion.Angle(transform.GetChild(0).rotation, startRotation);
        Debug.Log("index :" + pinIndex +" angle :"+ angle);

        return angle < 0.85f;
    }
}
