using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public Slider batterySlider;

    public GameObject keysPanel;

    public Text batteryText;

    public Text promptText;

    [SerializeField] private LidarScanner scannerScript;

    [SerializeField] private PlayerMovement playerScript;

    private List<int> keys;

    public bool canPickKey;
    public bool canOpenChest;

    private float maxValue;

    public void SetBattery(float battery)
    {
        batterySlider.value = maxValue - battery;
        batteryText.text = (maxValue - battery).ToString();
    }

    void updateKeySlots()
    {
        for (int i=3; i<=5; i++){
            if (keys.Contains(i)){
                keysPanel.transform.GetChild(i-3).gameObject.SetActive(true);
            } else {
                keysPanel.transform.GetChild(i-3).gameObject.SetActive(false);
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        batterySlider.maxValue = scannerScript.maxPoints;
        batterySlider.value = scannerScript.maxPoints;
        maxValue = scannerScript.maxPoints;

    }

    // Update is called once per frame
    void Update()
    {
        keys = playerScript.keys;
        canPickKey = playerScript.canPickKey;
        canOpenChest = playerScript.canOpenChest;
        SetBattery(scannerScript.pointCount);
        updateKeySlots();
        if (canPickKey)
        {
            promptText.text = "Press E to pick up key";
        }
        else if (canOpenChest){
            promptText.text = "Press E to open";
        } else {
            promptText.text = "";
        }
    }
}
