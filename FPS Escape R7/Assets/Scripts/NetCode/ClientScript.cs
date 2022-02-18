using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using Assets.Scripts.NetCode;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Scripts.Data;
using System.Runtime.InteropServices;
using System;
using static Assets.Scripts.NetCode.NetRequestToken;

public class ClientScript : NetworkScript
{
    /// <summary>
    /// �����������, �� ���� ��� ��� �������.
    /// </summary>
    public NetworkConnection Connection;
    /// <summary>
    /// ����, �����������, ��� � ������������ �� ���������.
    /// </summary>
    public bool Done;
    /// <summary>
    /// ������� ������� �� �������� ������. ������������ ������� ����� ����� �����������.
    /// </summary>
    private Queue<NetRequestToken> tokens = new Queue<NetRequestToken>();
    private ulong tokenId;
    private Dictionary<ulong, INetRequestHolder> requests = new Dictionary<ulong, INetRequestHolder>();

    public int MaxRequestsPerFrame = 1000;

    void Start()
    {
        var endpoint = NetworkEndPoint.LoopbackIpv4;//��� ����� �� ��������� ������ ���������.
        endpoint.Port = 9000;
        CreateClient(endpoint, out Connection);
    }

    public void OnDestroy()
    {
        Driver.Dispose();//����� ���������� ������� ��� ����������.
    }

    void Update()
    {
        Driver.ScheduleUpdate().Complete();//��������������� �������.

        if (!Connection.IsCreated)//���� ��� �����������
        {
            if (!Done)//� ���� ������ � ������������
                Debug.Log("Something went wrong during connect");//���������� ��������� � �����
            return;//��������� ��������� ����������.
        }

        DataStreamReader stream;//����� ��� ������ ������
        NetworkEvent.Type cmd;//��� �������, ����������� �� ������.
        Driver.BeginSend(Connection, out var writer);
        for (int i = 0; tokens.Count > 0; i++, tokenId++)
        {
            if (i > MaxRequestsPerFrame) break;//��������� ����� ������ ��� ���������� ����� �������� � �������.
            var token = tokens.Dequeue();
            requests.Add(tokenId, token.TokenHolder);
            writer.WriteULong(tokenId);
            writer.WriteInt((int)token.Type);
            switch (token.Type)
            {
                case RequestType.GetPlayerData:
                    {
                        writer.WriteULong((ulong)token.SendData);
                        break;
                    }
                case RequestType.SetPlayerData:
                    {
                        PlayerData data = (PlayerData)token.SendData;
                        writer.WriteULong(data.ID);
                        writer.WriteFixedString32(new Unity.Collections.FixedString32Bytes(data.Nickname));
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
                case RequestType.SyncronizePosition:
                    {
                        Transform transform = (Transform)token.SendData;
                        Quaternion rot = transform.localRotation;
                        Vector3 pos = transform.localPosition;
                        Vector3 size = transform.localScale;
                        writer.WriteFloat(rot.x);
                        writer.WriteFloat(rot.y);
                        writer.WriteFloat(rot.z);
                        writer.WriteFloat(rot.w);
                        writer.WriteFloat(pos.x);
                        writer.WriteFloat(pos.y);
                        writer.WriteFloat(pos.z);
                        writer.WriteFloat(size.x);
                        writer.WriteFloat(size.y);
                        writer.WriteFloat(size.z);
                        break;
                    }
                case RequestType.SendInput:
                    {
                        KeyCode code = (KeyCode)token.SendData;
                        writer.WriteInt((int)code);
                        break;
                    }
                case RequestType.Disconnect:
                    {
                        Done = true;
                        Connection.Disconnect(Driver);
                        Connection = default(NetworkConnection);//��� ����� ������ ��������� ��� �� �������.
                        break;
                    }
            }
        }

        while ((cmd = Connection.PopEvent(Driver, out stream)) != NetworkEvent.Type.Empty)//���� ���� ������ � ������
        {
            if (cmd == NetworkEvent.Type.Connect)//���� ��� ������� �� �����������
            {
                Debug.Log("We are now connected to the server");//�����, ��� ������� ������������.
            }
            else if (cmd == NetworkEvent.Type.Data)//���� ������ ������
            {
                ulong id = stream.ReadULong();
                RequestType reqType = (RequestType)stream.ReadInt();
                NetResponse resp = new NetResponse();
                switch (reqType)
                {
                    case RequestType.GetPlayerData:
                        {
                            PlayerData data = new PlayerData()
                            {
                                ID = stream.ReadULong(),
                                Nickname = stream.ReadFixedString32().ToString(),
                                Rank = (PlayerData.PlayerRank)stream.ReadInt(),
                                TotalKills = stream.ReadULong(),
                                TotalHeadshots = stream.ReadULong(),
                                TotalDeaths = stream.ReadULong(),
                                TotalAssists = stream.ReadULong(),
                                TotalDamage = stream.ReadULong(),
                                TotalDamageReceived = stream.ReadULong(),
                                TotalPlayTime = DateTime.FromBinary(stream.ReadLong()) - DateTime.MinValue,
                                TotalMatches = stream.ReadUInt(),
                                PlayerRegion = (GameRegion)stream.ReadInt(),
                            };
                            resp.Response = data;
                            break;
                        }
                    case RequestType.SyncronizePosition:
                        {
                            Quaternion rot = new Quaternion()
                            {
                                x = stream.ReadFloat(),
                                y = stream.ReadFloat(),
                                z = stream.ReadFloat(),
                                w = stream.ReadFloat()
                            };
                            Vector3 pos = new Vector3()
                            {
                                x = stream.ReadFloat(),
                                y = stream.ReadFloat(),
                                z = stream.ReadFloat(),
                            };
                            Vector3 size = new Vector3()
                            {
                                x = stream.ReadFloat(),
                                y = stream.ReadFloat(),
                                z = stream.ReadFloat(),
                            };
                            resp.Response = (rot, pos, size);
                            break;
                        }
                    default:
                        {
                            if (stream.ReadByte() != SuccessServerResponse) Debug.Log("Wrong Server response!");
                            resp.Response = null;
                            break;
                        }
                }
                requests[id].Response = resp;
            }
            else if (cmd == NetworkEvent.Type.Disconnect)//���� ��� ���������� �� �������
            {
                Debug.Log("Client got disconnected from server");
                Connection = default(NetworkConnection);
            }
        }
    }
}
