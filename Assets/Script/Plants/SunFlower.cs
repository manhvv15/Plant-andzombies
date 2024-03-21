using UnityEngine;

namespace Assets.Script.Plants
{
    public class SunFlower : MonoBehaviour
    {
        public GameObject sunObject;
        public float initialCooldown;
        public float cooldown;

        protected virtual void Start()
        {
            Invoke(nameof(SpawnSun), initialCooldown);
            InvokeRepeating(nameof(SpawnSun), initialCooldown + cooldown, cooldown);
        }

        protected virtual void SpawnSun()
        {
            GameObject mySun =  Instantiate(sunObject, new Vector3(transform.position.x, transform.position.y, 0 ), Quaternion.identity);
            mySun.GetComponent<Sun>().dropTpYPos = transform.position.y  - 1 ;
        }
    }
}
