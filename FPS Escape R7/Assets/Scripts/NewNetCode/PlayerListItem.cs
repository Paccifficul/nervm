using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace NewNetCode
{
    public class PlayerListItem : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMP_Text text;
        private Player _player;
        
        public void SetUp(Player player)
        {
            _player = player;
            text.text = player.NickName;
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if (Equals(_player, otherPlayer))
            {
                Destroy(gameObject);
            }
        }

        public override void OnLeftRoom()
        {
            Destroy(gameObject);
        }
    }
}
