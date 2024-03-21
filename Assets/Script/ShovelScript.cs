using UnityEngine;
using UnityEngine.UI;

namespace Assets.Script
{
    public class ShovelScript : MonoBehaviour
    {
        private GameManage gms;
        private bool isUsingShovel = false;
        private Vector3 initialShovelLocation;
        [SerializeField] private GameObject shovel;
        // Start is called before the first frame update
        void Start()
        {
            initialShovelLocation = shovel.transform.position;
            gms = GameObject.Find("GameManage").GetComponent<GameManage>();
            GetComponent<Button>().onClick.AddListener(GetShovel);
        }

        // Update is called once per frame
        void Update()
        {
            if (gms.isUsingShovel)
            {
                shovel.transform.position = Input.mousePosition;
            }
            else
            {
                shovel.transform.position = initialShovelLocation;
            }
        }

        private void GetShovel()
        {
            gms.GetShovel();
        }
    }
}
