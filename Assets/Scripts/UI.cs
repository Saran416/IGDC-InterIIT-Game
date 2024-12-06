using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Battery : MonoBehaviour
{
    public Slider batterySlider;

    public GameObject keysPanel;

    public Text batteryText;

    public Text keyText;

    [SerializeField] private LidarScanner scannerScript;

    [SerializeField] private PlayerMovement playerScript;

    private List<int> keys;

    public bool canPickKey;

    private float maxValue;

    public void SetBattery(float battery)
    {
        batterySlider.value = maxValue - battery;
        batteryText.text = (maxValue - battery).ToString();
    }

    void updateKeySlots()
    {
        for (int i = 0; i < keys.Count; i++)
        {
            int index = keys[i];
            if (index == 3 || index == 4 || index == 5)
            {
                keysPanel.transform.GetChild(keys[i] - 3).gameObject.SetActive(true);
            }
            else
            {
                keysPanel.transform.GetChild(keys[i] - 3).gameObject.SetActive(false);
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
        SetBattery(scannerScript.pointCount);
        updateKeySlots();
        if (canPickKey)
        {
            keyText.text = "Press E to pick up key";
        }
        else
        {
            keyText.text = "";
        }
    }
}
