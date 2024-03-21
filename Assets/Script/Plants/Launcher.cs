using Assets.Script.Sound;
using UnityEngine;

namespace Assets.Script.Plants
{
    public class Launcher : MonoBehaviour
    {
        public GameObject bullet;
        public Transform shootOrigin;
        public float cooldown;
        private bool canShoot;
        public float range;
        public LayerMask shootMask;
        private GameObject target;

    

        [SerializeField] private AudioClip _shootClip;

        private void Update()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, range, shootMask);
            if (hit.collider)
            {
                target = hit.collider.gameObject;
                Shoot();
            }
        }

        private void Start()
        {
            Invoke(nameof(ResetCooldown),cooldown);
        }

        private void Shoot()
        {
            if (!canShoot || target == null)
            {
                return;
            }
            canShoot = false;
            Invoke(nameof(ResetCooldown), cooldown);
            GameObject myBullet = Instantiate(bullet, shootOrigin.position, Quaternion.identity);
            myBullet.GetComponent<LobbedProjectile>().target = target;
            myBullet.GetComponent<LobbedProjectile>().initialLocation = shootOrigin.position;
            SoundManager.Instance.PlaySound(_shootClip);
        }

        private void ResetCooldown()
        {
            canShoot = true;
        }
    }
}
