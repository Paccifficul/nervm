using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using Unity.Collections;

/// <summary>
/// Этот класс я решил сделать как базовый для сервера и клиента.
/// </summary>
public class NetworkScript : MonoBehaviour
{
    /// <summary>
    /// Сетевой драйвер.
    /// </summary>
    public NetworkDriver Driver;

    public NetworkRole Role;
    /// <summary>
    /// Инициализирует скрипт в роли сервера.
    /// </summary>
    /// <param name="connections">Создаёт список подключений.</param>
    /// <exception cref="UnityException">Возникает в случае нессответствия назначения скрипта (Указана роль <see cref="NetworkRole.Client"/>).</exception>
    protected void CreateServer(out NativeList<NetworkConnection> connections)
    {
        if (Role == NetworkRole.Client)
        {
            throw new UnityException("This script cannot be initialized as server because it already has Client type.");
        }
        Role = NetworkRole.Server;
        Driver = NetworkDriver.Create();//Создаём новый драйвер без настроек.
        var endpoint = NetworkEndPoint.AnyIpv4;//Как я понял, это адрес для подключения.
        endpoint.Port = 9000;//Используемый сервером порт.
        if (Driver.Bind(endpoint) != 0)//Если не получилось занять этот порт, выбрасываем ошибку.
            Debug.Log("Failed to bind to port 9000");
        else
            Driver.Listen();//Если всё норм, то начинаем прослушивать порт.

        connections = new NativeList<NetworkConnection>(16, Allocator.Persistent);//Создаём список подключений.
    }
    /// <summary>
    /// Инициализирует скрипт как клиента.
    /// </summary>
    /// <param name="connection">Создаёт одно подключение к серверу.</param>
    protected void CreateClient(NetworkEndPoint endpoint, out NetworkConnection connection)
    {
        Driver = NetworkDriver.Create();//Инициализируем драйвер.

        
        connection = Driver.Connect(endpoint);//Создаём подключение к указанному серверу по указанному порту
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Роль данного скрипта в системе клиент-сервер.
    /// </summary>
    public enum NetworkRole
    {
        None,

        Server,

        Client
    }
}

