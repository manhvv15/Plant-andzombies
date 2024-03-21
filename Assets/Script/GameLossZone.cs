using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Script
{
    public class GameLossZone : MonoBehaviour
    {
        [SerializeField] private GameManage manager;
        private bool lost = false;
        public void OnTriggerEnter2D(Collider2D other)
        {
            if (lost)
            {
                return;
            }
            Debug.Log("Something entered this zone.");
            if (other.TryGetComponent<Zombie>(out Zombie zombie))
            {
                lost = true;
                Debug.Log("You lose!");
                manager.Lose();
            }
        }

    }
}
