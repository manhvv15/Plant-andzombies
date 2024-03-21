using Assets.Script.Sound;
using Assets.Script.Zombies.Accessories;
using UnityEngine;

namespace Assets.Script
{
    public class SnowPeaBullet : PeaBullet
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Zombie>(out Zombie zombie))
            {
                zombie.Hit(damage);
                if (zombie.accessory == null || zombie.accessory is not ShieldAccessory)
                {
                    zombie.Chill();
                }
                SoundManager.Instance.PlaySound(_onPeaHitClip);
                Destroy(gameObject);
            }
        }
    }
}