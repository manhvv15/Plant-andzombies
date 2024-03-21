using Assets.Script.Constants;
using Assets.Script.Interfaces;
using Assets.Script.Sound;
using UnityEngine;

namespace Assets.Script
{
    public class LobbedProjectile : MonoBehaviour, IProjectile
    {
        public int damage;
        public int framesUntilHit = 300;
        public GameObject target;
        public Vector3 initialLocation;
        [SerializeField] private AudioClip _onHitClip;
        private Vector3 targetPosition;

        private int frame = 0;

        private void Update()
        {
            Move();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Zombie>(out Zombie zombie))
            {
                zombie.Hit(damage, DamageType.LOB);
                SoundManager.Instance.PlaySound(_onHitClip);
                Destroy(gameObject);
            }
        }

        protected bool TryHitTarget()
        {
            bool hit = false;
            if (target == null)
            {
            
            }
            else
            {
                hit = true;
                var zombie = target.GetComponent<Zombie>();
                zombie.Hit(damage);
                SoundManager.Instance.PlaySound(_onHitClip);
            }
            Destroy(gameObject);
            return hit;
        }

        public void Move()
        {
            frame++;
            if (frame > framesUntilHit)
            {
                TryHitTarget();
                return;
            }
            if (target != null)
            {
                targetPosition = target.transform.position;
            }
            var distanceToTarget = (targetPosition.x - initialLocation.x);

            var xValue = distanceToTarget / 2;
            var aValue = 3 / (xValue * xValue);

            var xTraveled = distanceToTarget / framesUntilHit * frame - distanceToTarget / 2;
            var yTraveled = -aValue * xTraveled * xTraveled;
            var newLocation = new Vector3(initialLocation.x + xTraveled + xValue, initialLocation.y + yTraveled + 3, 0);
            gameObject.transform.position = newLocation;
        }
    }
}
