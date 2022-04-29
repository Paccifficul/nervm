using UnityEngine;

namespace Menu
{
    public class MenuManager : MonoBehaviour
    {
        public static MenuManager Instance;

        [SerializeField]
        private Menu[] menus;

        private void Awake()
        {
            Instance = this;
        }

        public void OpenMenu(string menu1Name, string menu2Name = null)
        {

            foreach (var menu in menus)
            {
                if (menu.MenuName == menu1Name)
                {
                    menu.Open();
                    continue;
                }

                if (menu.MenuName == menu2Name)
                {
                    menu.Open();
                    continue;
                }

                if (menu.IsOpen) menu.Close();
            }

        }

        public void CloseMenu(string menuName)
        {
            foreach (var m in menus)
            {
                if (m.MenuName != menuName) continue;
                m.Close();
                return;
            }
        }
    }
}
