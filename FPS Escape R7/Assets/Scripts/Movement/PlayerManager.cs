using System.IO;
using System.Collections;
using NewNetCode;
using UnityEngine;
using Photon.Pun;

namespace Movement
{
    public class PlayerManager : MonoBehaviour
    {
        private static PhotonView _pv;

        private static GameObject _controller;

        private void Awake()
        {
            _pv = GetComponent<PhotonView>();
        }

        private void Start()
        {
            if (_pv.IsMine) CreateController();
        }

        private static void CreateController()
        {
            var sp = SpawnPointManager.Instance.GetSpawnPoint();
            
            PhotonNetwork.Instantiate(
                Path.Combine("PhotonPrefabs", "PlayerController"),
                sp.position,
                sp.rotation,
                0,
                new object[]
                {
                    _pv.ViewID
                }
            );
        }

        public static void Die()
        {
            if (!_pv.IsMine) return;
         
            PhotonNetwork.Destroy(_controller);   
            PlayerController.Dead = 0;
            CreateController();
        }
    }
}
