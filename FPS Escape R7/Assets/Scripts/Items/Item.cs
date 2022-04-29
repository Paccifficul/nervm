using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private ItemInfo itemInfo;
        [SerializeField] private GameObject infoGameObject;

        public ItemInfo ItemInfoProp
        {
            get => itemInfo;
            set => itemInfo = value;
        }

        public GameObject ItemGameObjectProp
        { 
            get => infoGameObject;
            set => infoGameObject = value;
        }
        
        
    }    
}