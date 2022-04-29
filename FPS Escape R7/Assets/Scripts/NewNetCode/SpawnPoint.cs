using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewNetCode
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField] private GameObject graphics;
        
        private void Awake()
        {
            graphics.SetActive(false);
        }
    }
}