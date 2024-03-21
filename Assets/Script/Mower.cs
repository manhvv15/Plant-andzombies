using Assets.Script.Sound;
using UnityEngine;

namespace Assets.Script
{
    public class Mower : MonoBehaviour
    {
        public float speed = .4f;
        private bool activated = false;
        [SerializeField] private AudioClip mowing;

        private void Update()
        {
            if (activated)
            {
                Move();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.TryGetComponent<Zombie>(out Zombie zombie)){
                Destroy(zombie.gameObject);
                if (!activated)
                {
                    SoundManager.Instance.PlaySound(mowing);
                    activated = true;
                    Invoke(nameof(Remove),8);
                }
            }    
        }

        private void Remove()
        {
            Destroy(gameObject);
        }

        public void Move()
        {
            transform.position += new Vector3(speed * Time.fixedDeltaTime, 0, 0);
        }
    }
}
