using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;

public class ClientScript : NetworkScript
{
    /// <summary>
    /// Подключение, на этот раз для клиента.
    /// </summary>
    public NetworkConnection Connection;
    /// <summary>
    /// Флаг, указывающий, что с подключением всё нормально.
    /// </summary>
    public bool Done;

    void Start()
    {
        var endpoint = NetworkEndPoint.LoopbackIpv4;//Эта штука по умолчанию делает локалхост.
        endpoint.Port = 9000;
        CreateClient(endpoint, out Connection);
    }

    public void OnDestroy()
    {
        Driver.Dispose();//Нужно освободить драйвер при отключении.
    }

    void Update()
    {
        Driver.ScheduleUpdate().Complete();//Подготоваливаем драйвер.

        if (!Connection.IsCreated)//Если нет подключения
        {
            if (!Done)//И есть ошибки с подключением
                Debug.Log("Something went wrong during connect");//Отправляем сообщение в дебаг
            return;//Завершаем процедуру обновления.
        }

        DataStreamReader stream;//Поток для чтения данных
        NetworkEvent.Type cmd;//Тип команды, поступающей из потока.

        while ((cmd = Connection.PopEvent(Driver, out stream)) != NetworkEvent.Type.Empty)//Пока есть данные в потоке
        {
            if (cmd == NetworkEvent.Type.Connect)//Если это событие на подключение
            {
                Debug.Log("We are now connected to the server");//Пишем, что успешно подключились.

                uint value = 1;//Делаем что-то после подключения.
                Driver.BeginSend(Connection, out var writer);
                writer.WriteUInt(value);
                Driver.EndSend(writer);
            }
            else if (cmd == NetworkEvent.Type.Data)//Если пришли данные
            {
                uint value = stream.ReadUInt();//Что-то делаем с ними
                Debug.Log("Got the value = " + value + " back from the server");
                Done = true;
                Connection.Disconnect(Driver);
                Connection = default(NetworkConnection);//Эта штука должны отключить нас от сервера.
            }
            else if (cmd == NetworkEvent.Type.Disconnect)//Если это отключение от сервера
            {
                Debug.Log("Client got disconnected from server");
                Connection = default(NetworkConnection);
            }
        }
    }
}
