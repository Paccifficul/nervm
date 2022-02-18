using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

using Unity.Collections;
using Unity.Networking.Transport;
using Assets.Scripts.NetCode;
using static Assets.Scripts.NetCode.NetRequestToken;
using Assets.Scripts.Data;
using System;

public class ServerScript : NetworkScript
{
    /// <summary>
    /// Список подключенных клиентов.
    /// </summary>
    private NativeList<NetworkConnection> Connections;

    void Start()
    {
        CreateServer(out Connections);
    }

    void OnDestroy()
    {
        //Это неуправляемые ресурсы, поэтому их надо освобождать при отключении сервера.
        Driver.Dispose();
        Connections.Dispose();
    }

    void Update()
    {
        Driver.ScheduleUpdate().Complete();//Указываем драйверу, что он может принимать запросы.
        // CleanUpConnections
        for (int i = 0; i < Connections.Length; i++)//В теории этот цикл удаляет из списка отключившихся пользователей.
        {
            if (!Connections[i].IsCreated)
            {
                Connections.RemoveAtSwapBack(i);
                --i;
            }
        }
        // AcceptNewConnections
        NetworkConnection c;
        while ((c = Driver.Accept()) != default)//А этот добавляет новых.
        {
            Connections.Add(c);
            Debug.Log("Accepted a connection");
        }

        for (int i = 0; i < Connections.Length; i++)//Для каждого подключения
        {
            Assert.IsTrue(Connections[i].IsCreated);

            NetworkEvent.Type cmd;
            while ((cmd = Driver.PopEventForConnection(Connections[i], out DataStreamReader stream)) != NetworkEvent.Type.Empty)//До тех пор, пока в потоке данных есть события
            {
                if (cmd == NetworkEvent.Type.Data)//Тут идёт обработка каких-либо поступающих данных.
                {
                    ulong reqId = stream.ReadULong();
                    RequestType type = (RequestType)stream.ReadInt();
                    HandleRequest(stream, type, reqId, i);
                }
                else if (cmd == NetworkEvent.Type.Disconnect)//Если пользователь даёт команду на отключение, отключаем его.
                {
                    Debug.Log("Client disconnected from server");
                    Connections[i] = default;
                }
            }
        }
    }

    /// <summary>
    /// По идее, тут нужно как-то обрабатывать поступающие запросы. Надо будет что-нибудь потом с этим сделать.
    /// </summary>
    /// <param name="reader"></param>
    /// <param name="type"></param>
    /// <param name="id"></param>
    private void HandleRequest(in DataStreamReader reader, RequestType type, ulong id, int connectionId)
    {
        Driver.BeginSend(Connections[connectionId], out var writer);
        switch (type)
        {
            case RequestType.GetPlayerData:
                {
                    PlayerData data = DatabaseHandler.GetData(reader.ReadULong());
                    writer.WriteULong(data.ID);
                    writer.WriteFixedString32(new FixedString32Bytes(data.Nickname));
                    writer.WriteInt((int)data.Rank);
                    writer.WriteULong(data.TotalKills);
                    writer.WriteULong(data.TotalHeadshots);
                    writer.WriteULong(data.TotalDeaths);
                    writer.WriteULong(data.TotalAssists);
                    writer.WriteULong(data.TotalDamage);
                    writer.WriteULong(data.TotalDamageReceived);
                    writer.WriteLong((DateTime.MinValue + data.TotalPlayTime).ToBinary());
                    writer.WriteUInt(data.TotalMatches);
                    writer.WriteInt((int)data.PlayerRegion);
                    break;
                }
        }
    }
}
