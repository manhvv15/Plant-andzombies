using System.Collections.Generic;
using Assets.Script;
using UnityEngine;

namespace Assets
{
    public class PlantSelectTab : MonoBehaviour
    {
        public List<PlantScriptSelect> Selects = new List<PlantScriptSelect>(9);

        [SerializeField] private GameObject[] slots;
        [SerializeField] private PlantSelectManager manager;

        public int plantLimit = 9;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public bool AppendPlant(PlantScriptSelect select)
        {
            if (Selects.Count >= plantLimit)
            {
                return false;
            }
            var package = Instantiate(select, this.transform);
            package.isAddMode = false;
            package.copy = select;
            select.copy = package;
            select.Disable();
            Selects.Add(package);
            UpdateTab();
            return true;
        }

        public void RemovePlant(PlantScriptSelect select)
        {
            if (Selects.Remove(select))
            {
                select.copy.Enable();
                select.copy.copy = null;
                Destroy(select.gameObject);
                UpdateTab();
            }
            else
            {
                Debug.LogWarning("Error");
            }
        }

        private void UpdateTab()
        {
            int i = 0;
            foreach (var select in Selects)
            {
                select.transform.position = slots[i].transform.position;
                select.transform.SetParent(slots[i].transform);
                i++;
            }

            if (Selects.Count == plantLimit)
            {
                manager.AllowBegin();
            }
            else
            {
                manager.DenyBegin();
            }
        }
    }
}
