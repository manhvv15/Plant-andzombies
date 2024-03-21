using System;
using System.Collections.Generic;
using Assets.Script.Zombies.Accessories;
using UnityEngine;

namespace Assets.Script.Zombies
{
    public class ZombieFactory : MonoBehaviour
    {
        [SerializeField] private Zombie basicZombie;
        [SerializeField] private Zombie impZombie;
        [SerializeField] private Accessory cone;
        [SerializeField] private Accessory bucket;
        [SerializeField] private Accessory screenDoor;
        [SerializeField] private Accessory newspaper;
        [SerializeField] private Accessory sunHat;

        public bool InvisiGhoulMode = false;
        private ZombieFactory(){}

        private Dictionary<string, Func<Vector3, Zombie>> functionDictionary =
            new();

        private void Start()
        {
            functionDictionary.Add("normal",InstantiateBasicZombie);
            functionDictionary.Add("conehead", InstantiateConeheadZombie);
            functionDictionary.Add("buckethead", InstantiateBucketheadZombie);
            functionDictionary.Add("door",InstantiateScreenDoorZombie);
            functionDictionary.Add("newspaper",InstantiateNewspaperZombie);
            functionDictionary.Add("sunhat", InstantiateSunHatZombie);
            functionDictionary.Add("imp", InstantiateImpZombie);
        }

        public static ZombieFactory Instance;
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public Zombie InstantiateBasicZombie(Vector3 location)
        {
            var z = Instantiate(basicZombie,location,Quaternion.identity);
            if (InvisiGhoulMode)
            {
                var spriteRenderer = z.GetComponent<SpriteRenderer>();
                var color = spriteRenderer.color;
                color.a = 0;
                spriteRenderer.color = color;
            }
            return z;
        }

        public Zombie InstantiateConeheadZombie(Vector3 location)
        {
            var zombie = InstantiateBasicZombie(location);
            zombie.SetAccessory(cone);
            return zombie;
        }

        public Zombie InstantiateBucketheadZombie(Vector3 location)
        {
            var zombie = InstantiateBasicZombie(location);
            zombie.SetAccessory(bucket);
            return zombie;
        }

        public Zombie InstantiateFromType(Vector3 location, string type)
        {
            if (functionDictionary.TryGetValue(type, out var func))
            {
                return func(location);
            }
            Debug.LogWarning($"Unknown zombie type: {type}. Spawned Basic.");
            return InstantiateBasicZombie(location);
        }

        public Zombie InstantiateScreenDoorZombie(Vector3 location)
        {
            var zombie = InstantiateBasicZombie(location);
            zombie.SetAccessory(screenDoor);
            return zombie;
        }

        public Zombie InstantiateNewspaperZombie(Vector3 location)
        {
            var zombie = InstantiateBasicZombie(location);
            zombie.SetHealth(270);
            zombie.SetAccessory(newspaper);
            return zombie;
        }

        public Zombie InstantiateSunHatZombie(Vector3 location)
        {
            var zombie = InstantiateBasicZombie(location);
            zombie.SetAccessory(sunHat);
            return zombie;
        }

        public Zombie InstantiateImpZombie(Vector3 location)
        {
            var z = Instantiate(impZombie, location, Quaternion.identity);
            if (InvisiGhoulMode)
            {
                var spriteRenderer = z.GetComponent<SpriteRenderer>();
                var color = spriteRenderer.color;
                color.a = 0;
                spriteRenderer.color = color;
            }
            return z;
        }
    }
}
