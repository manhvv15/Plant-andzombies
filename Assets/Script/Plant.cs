using Assets.Script.Sound;
using UnityEngine;

namespace Assets.Script
{
    public class Plant : MonoBehaviour
    {
        public int health;
        [SerializeField] protected AudioClip eatenAudioClip;
        public virtual void Start()
        {
            gameObject.layer = 9;
        }
        public virtual void Hit(int damage)
        {
            health -= damage;
            if (health <= 0)
            {
                SoundManager.Instance.PlaySound(eatenAudioClip);
                Destroy(gameObject);
            }
        }
    }
}
