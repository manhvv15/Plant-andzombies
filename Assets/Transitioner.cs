using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transitioner : MonoBehaviour
{
    private Image image;
    [SerializeField] private Image loseImage;
    private RectTransform rectTransform;
    private float target = 0;

    private float imageTarget = 0;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        rectTransform = loseImage.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (image.color.a < target)
        {
            var color = image.color;
            color.a += 0.001f;
            image.color = color;
        }

        var x = rectTransform.localScale.x;
        if (x < imageTarget)
        {
            x += 0.005f;
            rectTransform.localScale = new Vector3(x, x, 1);
        }
    }

    public void FadeBlack(float delay)
    {
        var color = new Color(0, 0, 0, 0);
        image.color = color;
        Invoke(nameof(Transition),delay);
    }

    public void FadeWhite(float delay)
    { 
        var color = new Color(1,1,1,0);
        image.color = color;
        Invoke(nameof(Transition), delay);
    }

    private void Transition()
    {
        target = 1;
    }

    public void Reset()
    {
        target = 0;
        var color = image.color;
        color.a = 0;
        image.color = color;
    }

    public void ShowLoseImage(float delay)
    {
        Invoke(nameof(LoseImage),delay);
    }

    private void LoseImage()
    {
        imageTarget = 1;
    }
}
