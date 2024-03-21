using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Script.Zombies.Accessories
{
    public class SunHatAccessory : Accessory
    {
        [SerializeField] private Sun sun;

        protected override void OnAccessoryRemoved(Zombie owner)
        {
            var destroyLocation = transform.position;
            var firstSpawn = destroyLocation + new Vector3(1, 0, 0);
            if (firstSpawn.x > 8)
            {
                firstSpawn.x = 8;
            }
            var secondSpawn = destroyLocation + new Vector3(-1, 0, 0);
            if (secondSpawn.x > 8) { secondSpawn.x = 8; }
            var sun1 = Instantiate(sun, firstSpawn, Quaternion.identity);
            sun1.dropTpYPos = sun1.transform.position.y - 1;
            var sun2 = Instantiate(sun, secondSpawn, Quaternion.identity);
            sun2.dropTpYPos = sun2.transform.position.y - 1;
        }
    }
}
