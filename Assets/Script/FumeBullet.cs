using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Assets.Script.Constants;
using Assets.Script.Interfaces;
using Assets.Script.Sound;
using UnityEngine;

namespace Assets.Script
{
    public class FumeBullet : MonoBehaviour, IProjectile
    {
        public int damage;
        public float speed = 1f;
        private float xTarget;

        private readonly List<Zombie> zombieCollied = new();

        private void Start()
        {
            xTarget = transform.position.x + 5.5f;
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Zombie>(out var zombie))
            {
                if (zombieCollied.Contains(zombie)) return;
                zombieCollied.Add(zombie);
                zombie.Hit(damage,DamageType.LASER);
            }
        }
        public void Move()
        {
            if (transform.position.x < xTarget)
            {
                transform.position += new Vector3(speed * Time.fixedDeltaTime, 0, 0);
            }
            else
            {
                Kill();
            }
        }

        public void Kill()
        {
            Destroy(gameObject);
        }
    }
}
