using UnityEngine;

namespace Shooting
{
    public class ShootScript : MonoBehaviour
    {
        [SerializeField]
        private float damage;
        [SerializeField]
        private float force;
        [SerializeField]
        private int maxAmmo;
        [SerializeField]
        private int magazine;
        [SerializeField]
        private float fireRate;
        [SerializeField]
        private float reloadTime;

        private int _currentAmmo;
        private float _timeBetweenShots;

        private void Start()
        {
            _currentAmmo = maxAmmo;
            _timeBetweenShots = fireRate;
        }

        // Update is called once per frame
        private void Update()
        {
            if (_timeBetweenShots < fireRate)
            {
                _timeBetweenShots += Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.Mouse0) && _timeBetweenShots >= fireRate)
            {
                Shoot();
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                Invoke(nameof(Reload), reloadTime);
            }
        }

        private void Shoot()
        {
            if (_currentAmmo <= 0) return;
            _timeBetweenShots = 0;
            _currentAmmo--;
            var camTransform = GetComponentInParent<Camera>().transform;
            if (!Physics.Raycast(camTransform.position, camTransform.forward, out var hit)) return;
            if (hit.rigidbody == null) return;
            print("hi");
            hit.rigidbody.AddForce(-hit.normal * force);
        }

        private void Reload()
        {
            if (_currentAmmo != maxAmmo)
            {
                if (magazine >= maxAmmo)
                {
                    _currentAmmo = maxAmmo;
                    magazine -= maxAmmo;
                }
                else if (magazine != 0)
                {
                    _currentAmmo = magazine;
                    magazine = 0;
                }
            }
            print("Перезарядился");
        }
    }
}
