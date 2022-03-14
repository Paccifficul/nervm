using Assets.Scripts.NetCode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkSyncronizer : MonoBehaviour, INetRequestHolder
{
    public static ClientScript clientScript;
    //Пусть каждый элемент будет иметь возможность отправлять только один тип запросов и самостоятельно их обрабатывать.
    public NetResponse Response { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
