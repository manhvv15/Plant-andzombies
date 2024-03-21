using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Script.Constants;
using Assets.Script.Sound;
using UnityEngine;

namespace Assets.Script.Plants
{
    public class CherryBombPlant : InstantPlant
    {
        public int explosionDamage = 1800;
        [SerializeField] private AudioClip explosion;
        protected override void Effect()
        {
            var vectorA = new Vector3(1.6f, 1.6f, 0);
            var vectorB = new Vector3(-1.6f, -1.6f, 0);
            var position = gameObject.transform.position;
            var overlapArea = Physics2D.OverlapAreaAll(position + vectorA, position + vectorB);
            Debug.Log($"Cherry Bomb spotted {overlapArea.Length} objects.");
            foreach (var x in overlapArea)
            {
                if (x.TryGetComponent<Zombie>(out var zombie))
                {
                    zombie.Hit(explosionDamage,DamageType.EXPLOSION);
                }
            }
            SoundManager.Instance.PlaySound(explosion);
        }
    }
}
