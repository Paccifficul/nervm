using UnityEngine;

namespace Movement
{
    public class PlayerGroundedCheck : MonoBehaviour
    {
        private PlayerController _playerController;

        private void Awake()
        {
            _playerController = GetComponentInParent<PlayerController>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == _playerController.gameObject) return;
                
            _playerController.SetGroundedState(true);
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject == _playerController.gameObject) return;

            _playerController.SetGroundedState(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == _playerController.gameObject) return;
            
            _playerController.SetGroundedState(false);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject == _playerController.gameObject) return;
                
            _playerController.SetGroundedState(true);
        }

        private void OnCollisionStay(Collision collisionInfo)
        {
            if (collisionInfo.gameObject == _playerController.gameObject) return;

            _playerController.SetGroundedState(true);
        }

        private void OnCollisionExit(Collision other)
        {
            if (other.gameObject == _playerController.gameObject) return;

            _playerController.SetGroundedState(false);
        }
    }
}