using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

using Unity.Collections;
using Unity.Networking.Transport;

public class ServerScript : NetworkScript
{
    /// <summary>
    /// ������ ������������ ��������.
    /// </summary>
    private NativeList<NetworkConnection> Connections;

    void Start()
    {
        CreateServer(out Connections);
    }

    void OnDestroy()
    {
        //��� ������������� �������, ������� �� ���� ����������� ��� ���������� �������.
        Driver.Dispose();
        Connections.Dispose();
    }

    void Update()
    {
        Driver.ScheduleUpdate().Complete();//��������� ��������, ��� �� ����� ��������� �������.
        // CleanUpConnections
        for (int i = 0; i < Connections.Length; i++)//� ������ ���� ���� ������� �� ������ ������������� �������������.
        {
            if (!Connections[i].IsCreated)
            {
                Connections.RemoveAtSwapBack(i);
                --i;
            }
        }
        // AcceptNewConnections
        NetworkConnection c;
        while ((c = Driver.Accept()) != default(NetworkConnection))//� ���� ��������� �����.
        {
            Connections.Add(c);
            Debug.Log("Accepted a connection");
        }

        DataStreamReader stream;
        for (int i = 0; i < Connections.Length; i++)//��� ������� �����������
        {
            Assert.IsTrue(Connections[i].IsCreated);

            NetworkEvent.Type cmd;
            while ((cmd = Driver.PopEventForConnection(Connections[i], out stream)) != NetworkEvent.Type.Empty)//�� ��� ���, ���� � ������ ������ ���� �������
            {
                if (cmd == NetworkEvent.Type.Data)//��� ��� ��������� �����-���� ����������� ������.
                {
                    uint number = stream.ReadUInt();//�� ����, ��� ������ ������, ���� ����� ������� ��� ��������������� �����-�� �������� �����.

                    Debug.Log("Got " + number + " from the Client adding + 2 to it.");
                    number +=2;

                    Driver.BeginSend(NetworkPipeline.Null, Connections[i], out var writer);//��� �� �������������� ����� ��� �������� ������.
                    writer.WriteUInt(number);//���������� �����-���� ������.
                    Driver.EndSend(writer);//��������� ��������.
                }
                else if (cmd == NetworkEvent.Type.Disconnect)//���� ������������ ��� ������� �� ����������, ��������� ���.
                {
                    Debug.Log("Client disconnected from server");
                    Connections[i] = default(NetworkConnection);
                }
            }
        }
    }
}
