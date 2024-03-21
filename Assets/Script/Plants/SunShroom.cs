using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Script.Sound;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Script.Plants
{
    public class SunShroom : SunFlower
    {
        [SerializeField] private readonly int firstStageAmount = 15;
        [SerializeField] private readonly int secondStageAmount = 25;
        [SerializeField] private float timeTillGrowth = 120;
        [SerializeField] private Sprite secondSprite;
        [SerializeField] private AudioClip growClip;

        private int amount;

        protected override void Start()
        {
            amount = firstStageAmount;
            Invoke(nameof(Grow),timeTillGrowth);
            base.Start();
        }

        private void Grow()
        {
            amount = secondStageAmount;
            gameObject.GetComponent<SpriteRenderer>().sprite = secondSprite;
            SoundManager.Instance.PlaySound(growClip);
        }

        protected override void SpawnSun()
        {
            GameObject mySun = Instantiate(sunObject, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
            mySun.GetComponent<Sun>().SetAmount(amount);
            mySun.GetComponent<Sun>().dropTpYPos = transform.position.y - 1;
        }
    }
}
