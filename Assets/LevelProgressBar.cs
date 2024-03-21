using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgressBar : MonoBehaviour
{
    private Slider slider;

    private int value;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        if (slider.value != value)
        {
            var x = slider.value < value ? 1 : -1;
            slider.value += x;
        }
    }

    public void SetMax(int max)
    {
        slider.maxValue = max * 100;
    }

    public void SetValue(int value)
    {
        this.value = value * 100;
    }
}
