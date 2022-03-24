using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace NewNetCode
{
    public class RoomListItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        private RoomInfo _info;
    
        public void SetUp(RoomInfo info)
        {
            _info = info;
            text.text = info.Name;
        }

        public void OnClick()
        {
            Launcher.JoinRoom(_info);
        }
    }
}
