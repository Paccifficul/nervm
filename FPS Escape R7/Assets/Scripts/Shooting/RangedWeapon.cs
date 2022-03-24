using UnityEngine;

namespace Shooting
{
    public class RangedWeapon : Weapon
    {
        [SerializeField]
        protected int magazine;
        [SerializeField]
        protected float reloadTime;
        [SerializeField]
        protected float RecoilValue = 1.0f;

        protected int CurrentAmmo;
        protected Camera Cam;

        private void Start()
        {
            Cam = GetComponentInParent<Camera>();
        }

        // Update is called once per frame
        public void Update()
        {
            print(Cam);
        }
    
        protected void Spread()
        {
            var vertAngle = Cam.transform.localEulerAngles.x + Random.Range(-RecoilValue, RecoilValue);
            if (vertAngle > 90 && vertAngle < 280 || vertAngle > 80 && vertAngle < 270) return;
            Cam.transform.localEulerAngles = new Vector3(vertAngle, Random.Range(-RecoilValue, RecoilValue), Cam.transform.localEulerAngles.z);
        }
    }
}
