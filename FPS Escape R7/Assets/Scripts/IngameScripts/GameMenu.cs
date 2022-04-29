using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace InGameScripts
{
    public class GameMenu : MonoBehaviour
    {
        public bool IsGameMenuOpened { get; private set; }

        public static GameMenu Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            IsGameMenuOpened = false;
            transform.position += new Vector3(2000f, 0f, 0f);
        }

        private void Update()
        {
            if (SceneManager.GetActiveScene().name != "MapPrototype") return;

            if (Input.GetKeyDown(KeyCode.Escape) && ! IsGameMenuOpened)
            {
                transform.position -= new Vector3(2000f, 0f, 0f);
                IsGameMenuOpened = true;
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && IsGameMenuOpened)
            {
                transform.position += new Vector3(2000f, 0f, 0f);
                IsGameMenuOpened = false;
            }
        }

        public void ExitLobby()
        {
            SceneManager.LoadScene("NewMainMenu");
            PhotonNetwork.LeaveRoom();
        }

        public void ContinueGame()
        {
            transform.position += new Vector3(2000f, 0f, 0f);
            IsGameMenuOpened = false;
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}