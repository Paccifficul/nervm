using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

using Unity.Collections;
using Unity.Networking.Transport;

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
        while ((c = Driver.Accept()) != default(NetworkConnection))//А этот добавляет новых.
        {
            Connections.Add(c);
            Debug.Log("Accepted a connection");
        }

        DataStreamReader stream;
        for (int i = 0; i < Connections.Length; i++)//Для каждого подключения
        {
            Assert.IsTrue(Connections[i].IsCreated);

            NetworkEvent.Type cmd;
            while ((cmd = Driver.PopEventForConnection(Connections[i], out stream)) != NetworkEvent.Type.Empty)//До тех пор, пока в потоке данных есть события
            {
                if (cmd == NetworkEvent.Type.Data)//Тут идёт обработка каких-либо поступающих данных.
                {
                    uint number = stream.ReadUInt();//По сути, это просто пример, надо будет сделать уже непосредственно какие-то действия здесь.

                    Debug.Log("Got " + number + " from the Client adding + 2 to it.");
                    number +=2;

                    Driver.BeginSend(NetworkPipeline.Null, Connections[i], out var writer);//Тут мы инициализируем поток для отправки ответа.
                    writer.WriteUInt(number);//Отправляем какие-либо данные.
                    Driver.EndSend(writer);//Завершаем отправку.
                }
                else if (cmd == NetworkEvent.Type.Disconnect)//Если пользователь даёт команду на отключение, отключаем его.
                {
                    Debug.Log("Client disconnected from server");
                    Connections[i] = default(NetworkConnection);
                }
            }
        }
    }
}
