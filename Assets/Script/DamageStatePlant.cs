using Assets.Script.Sound;
using UnityEngine;

namespace Assets.Script
{
    public class DamageStatePlant : Plant
    {
        [SerializeField] private Sprite firstDamage;
        [SerializeField] private Sprite secondDamage;
        [SerializeField] private int healthAtFirstDamage;
        [SerializeField] private int healthAtSecondDamage;

        public override void Hit(int damage)
        {
            health -= damage;
            if (health <= 0)
            {
                SoundManager.Instance.PlaySound(eatenAudioClip);
                Destroy(gameObject);
            }
            else
            {
                UpdateSprite();
            }
        }

        private void UpdateSprite()
        {
            if (health < healthAtSecondDamage)
            {
                GetComponent<SpriteRenderer>().sprite = secondDamage;
            } else if (health < healthAtFirstDamage)
            {
                GetComponent<SpriteRenderer>().sprite = firstDamage;
            }
        }
    }
}
