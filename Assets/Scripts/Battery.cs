using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battery : MonoBehaviour
{
    public Slider batterySlider;

    [SerializeField] private LidarScanner scannerScript;

    private float maxValue;

    public void SetBattery(float battery)
    {
        batterySlider.value = maxValue - battery;
    }

    // Start is called before the first frame update
    void Start()
    {
        batterySlider.maxValue = scannerScript.maxPoints;
        maxValue = scannerScript.maxPoints;
    }

    // Update is called once per frame
    void Update()
    {
        SetBattery(scannerScript.pointCount);
    }
}
