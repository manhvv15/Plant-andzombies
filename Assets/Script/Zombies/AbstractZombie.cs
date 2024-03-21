using Assets.Script.Zombies.Accessories;
using UnityEngine;

namespace Assets.Script.Zombies
{
    public abstract class AbstractZombie : MonoBehaviour
    {
        protected float speed;
        protected int health;
        protected int damage;
        protected float range;
        public LayerMask plantMask;
        protected float eatCooldown;
        protected bool canEat = true;
        public Plant targetPlant;
        public ZombieType type;

        protected static readonly Color chilledColor = new Color(0.22f, 0.42f, 0.95f);

        //Unique boolean for freezing/chilling
        protected bool isChilled = false;
        //Unique boolean for angered zombie
        protected bool isAngry = false;
        protected bool isPreAngry = false;
        //For helmets, doors, newspaper,...
        public Accessory accessory;

        protected float GetFinalSpeed()
        {
            float finalSpeed = speed;
            if (isChilled)
            {
                finalSpeed /= 2;
            }
            if (isPreAngry)
            {
                finalSpeed = 0;
            }
            else if (isAngry)
            {
                finalSpeed *= 2;
            }
            return finalSpeed;
        }

        protected float GetFinalEatCooldown()
        {
            float finalEatCooldown = eatCooldown;
            if (isChilled)
            {
                finalEatCooldown *= 2;
            }
            if (isAngry)
            {
                finalEatCooldown /= 2;
            }
            return finalEatCooldown;
        }

        public void Chill()
        {
            CancelInvoke(nameof(Unchill));
            if (!isChilled)
            {
                isChilled = true;
                var a = GetComponent<SpriteRenderer>().color.a;
                var color = chilledColor;
                color.a = a;
                GetComponent<SpriteRenderer>().color = color;
            }
            Invoke(nameof(Unchill), 10);
        }

        public void Unchill()
        {
            if (isChilled)
            {
                isChilled = false;
                var a = GetComponent<SpriteRenderer>().color.a;
                var color = Color.white;
                color.a = a;
                GetComponent<SpriteRenderer>().color = color;
            }
        }

        public void TriggerAnger()
        {
            isPreAngry = true;
            Invoke(nameof(Anger),2);
        }

        private void Anger()
        {
            isPreAngry = false;
            isAngry = true;
        }

        public void SetHealth(int health)
        {
            this.health = health;
        }
    }
}
