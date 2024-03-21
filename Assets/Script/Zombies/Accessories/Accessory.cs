using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Script.Constants;
using UnityEditor;
using UnityEngine;

namespace Assets.Script.Zombies.Accessories
{
    public class Accessory : MonoBehaviour
    {
        public int health;
        public bool isMetal = false;


        [SerializeField] private Sprite firstDamagedSprite;
        [SerializeField] private Sprite secondDamagedSprite;

        [SerializeField] private int healthUntilFirstDamaged;
        [SerializeField] private int healthUntilSecondDamaged;
        public Vector3 dislodgeSprite;


        private SpriteRenderer spriteRenderer;
        private Vector3 destination;
        private bool canMove = false;
        private readonly float speed = 0.02f;

        private void Start()
        {
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            if (canMove)
            {
                var gap = (destination - transform.position);
                if (gap.magnitude <= 0.5)
                {
                    transform.position = destination;
                    canMove = false;
                    return;
                }
                var move = gap.normalized * speed;
                transform.position += move;
            }
        }

        /// <summary>
        /// Inflict damage to the accessory.
        /// </summary>
        /// <param name="damage">Amount of damage</param>
        /// <returns>How much damage was not inflicted to the accessory.</returns>
        public virtual int Hit(int damage, int damageType = DamageType.DIRECT)
        {
            int remain = 0;
            if (health < damage)
            {
                remain = damage - health;
                health = 0;
            }
            else
            {
                health -= damage;
            }

            UpdateSprite();
            return remain;
        }

        private void UpdateSprite()
        {
            if (health <= healthUntilSecondDamaged)
            {
                spriteRenderer.sprite = secondDamagedSprite;
            } else if (health <= healthUntilFirstDamaged)
            {
                spriteRenderer.sprite = firstDamagedSprite;
            }
        }


        public void RemoveAccessory(Zombie owner, bool doNotDestroy = false)
        {
            if (this != owner.accessory)
            {
                Debug.LogError("A zombie attempted to remove an accessory of another zombie.");
                return;
            }
            owner.accessory = null;
            OnAccessoryRemoved(owner);
            if (!doNotDestroy)
            {
                Destroy(gameObject);
            }
        }

        protected virtual void OnAccessoryRemoved(Zombie owner)
        {

        }

        public bool IsDead()
        {
            return health <= 0;
        }

        public void MoveTowards(Vector3 location)
        {
            destination = location;
            canMove = true;
        }
    }
}
