using ExitGames.Client.Photon;
using InGameScripts;
using Photon.Realtime;
using Items;
using Photon.Pun;
using UnityEngine;
using TMPro;

namespace Movement
{
    /// <summary>
    /// Movement Class
    /// </summary>
    public class PlayerController : MonoBehaviourPunCallbacks
    {
        [SerializeField] private float mouseSensitivity;
        [SerializeField] private float sprintSpeed;
        [SerializeField] private float walkSpeed;
        [SerializeField] private float jumpForce;
        [SerializeField] private float smoothTime;
        [SerializeField] private GameObject cameraHolder;
        [SerializeField] private Item[] items;
        [SerializeField] private int previousItemIndex = -1;

        private int _itemIndex;
        private float _verticalLocalRotation;
        private bool _isGrounded;
        public static int Dead { get; set; } = 0;
        private Rigidbody _rb;
        private PhotonView _pv;
        private Vector3 _smoothMoveVelocity;
        private Vector3 _moveAmount;
        public int Hitpoints { get; set; } = 100;
        private TextMeshProUGUI _hp;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _pv = GetComponent<PhotonView>();
        }

        private void Start()
        {
            if (_pv.IsMine)
            {
                EquipItem(0);
                _hp = GameObject.FindGameObjectWithTag("HPBar").GetComponent<TextMeshProUGUI>();
                Hitpoints = 100;
            }
            else
            {
                Destroy(GetComponentInChildren<Camera>().gameObject);
                Destroy(_rb);
            }
        }

        private void Update()
        {
            if (!_pv.IsMine || GameMenu.Instance.IsGameMenuOpened) return;

            Look();
            Move();
            Jump();
            
            _hp.text = $"{Hitpoints}";
            
            for (var i = 0; i < items.Length; ++i)
            {
                if (!Input.GetKeyDown((i + 1).ToString())) continue;
                EquipItem(i);
                break;
            }

            if (transform.position.y < -10f || Hitpoints <= 0)
            {
                Dead++;
            }
            
            if (Dead == 1) PlayerManager.Die();
        }

        private void FixedUpdate()
        {
            if (!_pv.IsMine) return;

            _rb.MovePosition(_rb.position + transform.TransformDirection(_moveAmount) * Time.fixedDeltaTime);
        }

        private void Look()
        {
            transform.Rotate(Vector3.up * (Input.GetAxisRaw("Mouse X") * mouseSensitivity));

            _verticalLocalRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
            _verticalLocalRotation = Mathf.Clamp(_verticalLocalRotation, -90f, 90f);

            cameraHolder.transform.localEulerAngles = Vector3.left * _verticalLocalRotation;
        }

        public void Rotate(float rotationValueX, float rotationValueY)
        {
            transform.Rotate(Vector3.up * rotationValueX);
            _verticalLocalRotation += rotationValueY;
            _verticalLocalRotation = Mathf.Clamp(_verticalLocalRotation, -90f, 90f);
            cameraHolder.transform.localEulerAngles = Vector3.left * _verticalLocalRotation;
        }

        private void Move()
        {
            var moveDir = new Vector3
            (
                Input.GetAxisRaw("Horizontal"),
                0,
                Input.GetAxisRaw("Vertical")
            ).normalized;

            _moveAmount = Vector3.SmoothDamp(
                _moveAmount,
                moveDir * (Input.GetButton("Sprint") ? sprintSpeed : walkSpeed),
                ref _smoothMoveVelocity,
                smoothTime
            );
        }

        private void Jump()
        {
            if (Input.GetButton("Jump") && _isGrounded)
            {
                _rb.AddForce(transform.up * jumpForce);
            }
        }

        public void SetGroundedState(bool grounded)
        {
            _isGrounded = grounded;
        }

        private void EquipItem(int index)
        {
            if (index == previousItemIndex) return;

            _itemIndex = index;

            items[_itemIndex].ItemGameObjectProp.SetActive(true);

            if (previousItemIndex != -1)
            {
                items[previousItemIndex].ItemGameObjectProp.SetActive(false);
            }

            previousItemIndex = _itemIndex;

            if (!_pv.IsMine) return;

            Hashtable hash = new()
            {
                {
                    "ItemIndex",
                    _itemIndex
                }
            };

            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (!_pv.IsMine && Equals(targetPlayer, _pv.Owner))
            {
                EquipItem(System.Convert.ToInt32(changedProps["ItemIndex"]));
            }
        }
    }
}