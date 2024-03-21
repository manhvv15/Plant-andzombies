using Assets.Script.Constants;
using Assets.Script.Sound;
using Assets.Script.Zombies;
using Assets.Script.Zombies.Accessories;
using UnityEngine;

namespace Assets.Script
{
    public class Zombie : AbstractZombie
    {
        [SerializeField] public Transform hatLocation;
        [SerializeField] private AudioClip eatAudioClip;
        private void Start()
        {
            health = type.health;
            damage = type.damage;
            range = type.range; 
            speed = type.speed;
            eatCooldown = type.eatCooldown;
        }
        private void Update()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, range, plantMask);

            if (hit.collider)
            {
                targetPlant = hit.collider.GetComponent<Plant>();
                Eat();
            }

            if(health == 2)
            {
                GetComponent<SpriteRenderer>().sprite = type.deathSprite;
            }
        }

        void Eat()
        {
            if(!canEat || !targetPlant)
            {
                return;
            }
            canEat = false;
            SoundManager.Instance.PlaySound(eatAudioClip);
            Invoke("ResetEatCooldown", GetFinalEatCooldown());

            targetPlant.Hit(damage);
        }

        private void ResetEatCooldown()
        {
            canEat = true;
        }

        private void FixedUpdate()
        {
            if (!targetPlant)
                transform.position -= new Vector3(GetFinalSpeed(), 0 ,0);
        }

        public void Hit(int damage, int damageType = DamageType.DIRECT)
        {
            if (accessory != null)
            {
                damage = accessory.Hit(damage,damageType);
                if (accessory.IsDead())
                {
                    accessory.RemoveAccessory(this);
                }
            }
            health -= damage;
            if (health <= 0 )
            {
                Destroy(gameObject);
            }
        }

        public void SetAccessory(Accessory accessory)
        {
            Debug.Log("Setting accessory");
            if (this.accessory != null)
            {
                Destroy(this.accessory);
            }
            this.accessory = Instantiate(accessory, hatLocation.position + accessory.dislodgeSprite, Quaternion.identity);
            this.accessory.transform.SetParent(this.transform);
        }

        
    }
}
