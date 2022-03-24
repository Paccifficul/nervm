using System;
using TMPro;
using UnityEngine;

namespace InGameScripts 
{
    public class TeamChoose: MonoBehaviour
    {
        public static TeamChoose Instance;

        private void Awake()
        {
            Instance = this;
        }
    }
}