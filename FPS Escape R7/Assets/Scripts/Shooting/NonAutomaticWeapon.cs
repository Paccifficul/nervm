using UnityEngine;

namespace Shooting
{
    public class NonAutomaticWeapon : RangedWeapon
    {
        private const int Clip = 1;

        private void Start()
        {
            Cam = GetComponentInParent<Camera>();
            CurrentAmmo = Clip;
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKey(KeyCode.Mouse0) && CurrentAmmo > 0)
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
            if (CurrentAmmo == Clip) return;
            if (magazine == 0) return;
            CurrentAmmo = Clip;
            magazine -= Clip;
        }
    }
}
