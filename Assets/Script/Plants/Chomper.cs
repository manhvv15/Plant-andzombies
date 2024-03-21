using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Assets.Script.Plants
{
    public class Chomper : MonoBehaviour
    {
        public float cooldown;
        private bool canEat = true;
        public float range;
        public LayerMask eatMask;
        private GameObject target;

        private Sprite standbySprite;
        [SerializeField] private Sprite chewingSprite;

        private void Start()
        {
            standbySprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        }

        private void Update()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, range, eatMask);
            if (hit.collider)
            {
                target = hit.collider.gameObject;
                TryEat();
            }
        }

        private void TryEat()
        {
            if (!canEat)
            {
                return;
            }
            //TODO Make sure some zombies (ie giant one) can't be eaten
            if (true)
            {
                //TODO fix this line
                canEat = false;
                Invoke(nameof(Eat),1);
            }
        }

        private void Eat()
        {
            Destroy(target);
            DenyEating();
            Invoke(nameof(AllowEating),cooldown);
        }

        private void DenyEating()
        {
            canEat = false;
            gameObject.GetComponent<SpriteRenderer>().sprite = chewingSprite;
        }
        private void AllowEating()
        {
            canEat = true;
            gameObject.GetComponent<SpriteRenderer>().sprite = standbySprite;
        }
    }
}
