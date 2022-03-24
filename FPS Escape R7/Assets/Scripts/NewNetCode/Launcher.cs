using System;
using System.Collections;
using System.Collections.Generic;
using InGameScripts;
using Menu;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using Random = System.Random;

namespace NewNetCode
{
    public class Launcher : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMP_InputField roomNameInputField;
        [SerializeField] private TMP_Text roomName;
        [SerializeField] private GameObject errorMessage;
        [SerializeField] private TMP_Text errorMessageText;
        [SerializeField] private TMP_Text errorMessageCode;
        [SerializeField] private Transform roomListContent;
        [SerializeField] private GameObject roomListItemPrefab;
        [SerializeField] private Transform playerListContent;
        [SerializeField] private GameObject playerListItemPrefab;

        // Start is called before the first frame update
        private void Start()
        {
            StartCoroutine(ConnectToServers());
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        public override void OnJoinedLobby()
        {
            MenuManager.Instance.OpenMenu("Bar", "PlayWindow");
            PhotonNetwork.NickName =
                GenerateNickName.NickNames[UnityEngine.Random.Range(0, GenerateNickName.NickNames.Count - 1)];
        }

        private static IEnumerator ConnectToServers()
        {
            yield return new WaitForSeconds(1f);
            PhotonNetwork.ConnectUsingSettings();
        }

        public void CreateRoom()
        {
            if (string.IsNullOrEmpty(roomNameInputField.text)) return;

            MenuManager.Instance.OpenMenu("Loading");

            PhotonNetwork.CreateRoom(roomNameInputField.text);
        }

        public override void OnJoinedRoom()
        {
            MenuManager.Instance.OpenMenu("Room");
            roomName.text = PhotonNetwork.CurrentRoom.Name;
            
            var players = PhotonNetwork.PlayerList;

            foreach (var player in players)
            {
                Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(player);
            }
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            var animErrorMessage = errorMessage.GetComponent<Animator>();
            animErrorMessage.Play("OpenError");
            errorMessageCode.text = returnCode.ToString();
            errorMessageText.text = message;
        }

        public void StartGame()
        {
            PhotonNetwork.LoadLevel(2);
        }

        public override void OnLeftRoom()
        {
            MenuManager.Instance.OpenMenu("Bar", "PlayWindow");
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            foreach (Transform trans in roomListContent)
            {
                Destroy(trans.gameObject);
            }
            
            foreach (var room in roomList)
            {
                Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(room); 
            }   
        }
 
        public static void JoinRoom(RoomInfo info)
        {
            PhotonNetwork.JoinRoom(info.Name);
            MenuManager.Instance.OpenMenu("Loading");
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer); 
        }
    }
}
