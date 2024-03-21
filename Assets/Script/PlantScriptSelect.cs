using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script
{
    public class PlantScriptSelect : AbstractPlantScript
    {
        [SerializeField] private PlantSelectTab plantSelect;
        [SerializeField] private TextMeshProUGUI priceText;
        [SerializeField] private int price;
        public GameObject seedPackage;
        public PlantScriptSelect copy;
        [SerializeField] private Image plantImage;
        [SerializeField] private Sprite plantSprite;
        public bool isSunProductionPlant = false;
        
        private bool isPermanentlyDisabled = false;

        public bool isAddMode = true;
        public override void Start()
        {
            base.Start();
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            if (isPermanentlyDisabled)
            {
                return;
            }
            if (isAddMode)
            {
                if (isEnabled)
                {
                    plantSelect.AppendPlant(this);
                }
                else
                {
                    plantSelect.RemovePlant(this.copy);
                }
            }
            else
            {
                plantSelect.RemovePlant(this);
            }
        }

        private void OnValidate()
        {
            priceText.text = $"{price}";
            plantImage.sprite = plantSprite;
        }

        public void PermanentlyDisable()
        {
            isPermanentlyDisabled = true;
            Disable();
        }
    }
}
