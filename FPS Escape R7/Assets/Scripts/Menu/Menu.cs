using UnityEngine;

namespace Menu
{
    public class Menu : MonoBehaviour
    {
        [SerializeField]
        private string menuName;
        [SerializeField]
        private bool isOpen;

        public string MenuName
        {
            get => menuName;
            set => menuName = value;
        }

        public bool IsOpen
        {
            get => isOpen;
            set => isOpen = value;
        }

        public void Open()
        {
            IsOpen = true;
            gameObject.SetActive(true);
        }

        public void Close()
        {
            IsOpen = false;
            gameObject.SetActive(false);
        }
    }
}
