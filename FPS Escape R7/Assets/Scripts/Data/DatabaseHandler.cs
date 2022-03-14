using Assets.Scripts.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting.Dependencies.Sqlite;

public class DatabaseHandler : MonoBehaviour
{

    private static TableQuery<PlayerData> playersData;

    // Start is called before the first frame update
    void Start()
    {
        playersData = DataBaseProcessor.GetTable<PlayerData>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static PlayerData GetData(ulong playerID)
    {
        return (from x in playersData where x.ID == playerID select x).First();
    }

    public static void SetData(PlayerData data)
    {
        playersData.Connection.InsertOrReplace(data);
    }
}
