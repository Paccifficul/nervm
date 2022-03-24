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
            Menu menu1 = null;
            Menu menu2 = null;

            foreach (var t in menus)
            {
                if (t.MenuName == menu1Name)
                {
                    menu1 = t;
                }

                if (t.MenuName == menu2Name)
                {
                    menu2 = t;
                }
            }

            OpenMenu(menu1, menu2);
        }

        private void OpenMenu(Menu menu1, Menu menu2)
        {
            foreach (var m in menus)
            {
                if (m.IsOpen)
                {
                    CloseMenu(m);
                }
            }

            menu1.Open();
            if (menu2 != null)
            {
                menu2.Open();
            }
        }

        public void CloseMenu(string menuName)
        {
            foreach (var m in menus)
            {
                if (m.MenuName != menuName) continue;
                CloseMenu(m);
                return;
            }
        }

        private static void CloseMenu(Menu menu)
        {
            menu.Close();
        }
    }
}
