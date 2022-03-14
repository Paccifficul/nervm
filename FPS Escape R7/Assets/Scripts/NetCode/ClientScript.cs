using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;

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

        while ((cmd = Connection.PopEvent(Driver, out stream)) != NetworkEvent.Type.Empty)//���� ���� ������ � ������
        {
            if (cmd == NetworkEvent.Type.Connect)//���� ��� ������� �� �����������
            {
                Debug.Log("We are now connected to the server");//�����, ��� ������� ������������.

                uint value = 1;//������ ���-�� ����� �����������.
                Driver.BeginSend(Connection, out var writer);
                writer.WriteUInt(value);
                Driver.EndSend(writer);
            }
            else if (cmd == NetworkEvent.Type.Data)//���� ������ ������
            {
                uint value = stream.ReadUInt();//���-�� ������ � ����
                Debug.Log("Got the value = " + value + " back from the server");
                Done = true;
                Connection.Disconnect(Driver);
                Connection = default(NetworkConnection);//��� ����� ������ ��������� ��� �� �������.
            }
            else if (cmd == NetworkEvent.Type.Disconnect)//���� ��� ���������� �� �������
            {
                Debug.Log("Client got disconnected from server");
                Connection = default(NetworkConnection);
            }
        }
    }
}
