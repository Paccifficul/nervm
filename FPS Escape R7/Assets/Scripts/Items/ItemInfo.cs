using UnityEngine;

namespace Items
{
    public class ItemInfo : ScriptableObject
    {
        [SerializeField] private string itemName;

        public string ItemName => itemName;
    }
}