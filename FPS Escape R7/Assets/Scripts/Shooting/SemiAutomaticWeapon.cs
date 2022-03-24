using UnityEngine;

namespace Shooting
{
    public class SemiAutomaticWeapon : RangedWeapon
    {
        [SerializeField]
        protected int clip;
        [SerializeField]
        private float fireRate;

        private float _timeBetweenShots;

        private void Start()
        {
            Cam = GetComponentInParent<Camera>();
            CurrentAmmo = clip;
            _timeBetweenShots = fireRate;
        }

        // Update is called once per frame
        public new void Update()
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
            if (CurrentAmmo <= 0) return;
            _timeBetweenShots = 0;
            CurrentAmmo--;
            var camTransform = GetComponentInParent<Camera>().transform;
            if (!Physics.Raycast(camTransform.position, camTransform.forward, out var hit)) return;
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * force);
            }
        }

        private void Reload()
        {
            if (CurrentAmmo == clip) return;
            if (magazine >= clip)
            {
                CurrentAmmo = clip;
                magazine -= clip;
            }
            else if (magazine != 0)
            {
                CurrentAmmo = magazine;
                magazine = 0;
            }
        }
    }
}
