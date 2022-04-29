using System;
using System.Collections.Generic;
using System.Linq;
using InGameScripts;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Menu
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMP_InputField roomNameInputField;
        [SerializeField] private TMP_Text roomName;
        [SerializeField] private TMP_Text errorMessageText;
        [SerializeField] private TMP_Text errorMessageCode;
        [SerializeField] private TMP_Text playerNickname;
        [SerializeField] private Transform roomListContent;
        [SerializeField] private Transform playerListContent;
        [SerializeField] private GameObject roomListItemPrefab;
        [SerializeField] private GameObject playerListItemPrefab;
        [SerializeField] private GameObject errorMessage;
        [SerializeField] private GameObject startGameButton;

        public static Launcher Instance;

        private void Awake()
        {
            Instance = this;
        }

        // Start is called before the first frame update
        private void Start()
        {
            var nick =
                GenerateNickName.NickNames[Random.Range(
                    0,
                    GenerateNickName.NickNames.Count - Random.Range(
                        1,
                        GenerateNickName.NickNames.Count % Random.Range(
                            2,
                            6
                        )
                    )
                )];
            
            PhotonNetwork.ConnectUsingSettings();
            playerNickname.text += nick;
            PhotonNetwork.NickName = nick;
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        public override void OnJoinedLobby()
        {
            MenuManager.Instance.OpenMenu("Bar", "PlayWindow");
        }

        public override void OnJoinedRoom()
        {
            MenuManager.Instance.OpenMenu("Room");
            roomName.text = PhotonNetwork.CurrentRoom.Name;
            
            var players = PhotonNetwork.PlayerList;

            foreach (Transform child in playerListContent)
            {
                Destroy(child.gameObject);
            }

            foreach (var player in players)
            {
                Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(player);
            }
            
            startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            var animErrorMessage = errorMessage.GetComponent<Animator>();
            animErrorMessage.Play("OpenError");
            errorMessageCode.text = returnCode.ToString();
            errorMessageText.text = message;
        }


        public override void OnLeftRoom()
        {
            MenuManager.Instance.OpenMenu("Loading");
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            foreach (Transform trans in roomListContent)
            {
                Destroy(trans);
            }

            foreach (var room in roomList.Where(room => !room.RemovedFromList))
            {
                Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(room);
            }
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer); 
        }

        public void StartGame()
        {
            PhotonNetwork.LoadLevel(2);
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public static void JoinRoom(RoomInfo info)
        {
            PhotonNetwork.JoinRoom(info.Name);
            MenuManager.Instance.OpenMenu("Loading");
        }
        
        public void CreateRoom()
        {
            if (string.IsNullOrEmpty(roomNameInputField.text)) return;

            MenuManager.Instance.OpenMenu("Loading");

            PhotonNetwork.CreateRoom(roomNameInputField.text);
        }
    }
}
