using System;
using TMPro;
using UnityEngine;

namespace Assets
{
    public class WarningText : MonoBehaviour
    {
        public const string HUGE_WAVE = "A huge wave of zombie is approaching";
        public const string FINAL_WAVE = "Final Wave";

        private TextMeshProUGUI textMesh;
        private RectTransform rectTransform;
        private bool isAnimating = false;
        private readonly Vector3 speed = new Vector3(0.02f, 0.02f, 0f);
    

        // Start is called before the first frame update
        void Start()
        {
            textMesh = GetComponent<TextMeshProUGUI>();
            rectTransform = GetComponent<RectTransform>();
        }

        // Update is called once per frame
        void Update()
        {
            if (isAnimating)
            {
                var scale = rectTransform.localScale;
                scale -= speed;
                rectTransform.localScale = scale;
                if (scale.x <= 1)
                {
                    isAnimating = false;
                    rectTransform.localScale = new Vector3(1f, 1f, 1f);
                }
            }
        }

        public void ShowText(string text, float t = 2)
        {
            textMesh.text = text;
            rectTransform.localScale = new Vector3(2,2,1);
            isAnimating = true;
            Invoke(nameof(HideText),t);
        }

        private void HideText()
        {
            textMesh.text = String.Empty;
        }
    }
}
