using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewNetCode
{
    public class SpawnPointManager : MonoBehaviour
    {
        public static SpawnPointManager Instance;

        private SpawnPoint[] _spawnPoints; 

        private void Awake()
        {
            Instance = this;
            _spawnPoints = GetComponentsInChildren<SpawnPoint>();
        }
        
        public Transform GetSpawnPoint()
        {
            return _spawnPoints[Random.Range(0, _spawnPoints.Length)].transform;
        }
    }
}