using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Networking.Transport;
using Unity.Collections;

/// <summary>
/// ���� ����� � ����� ������� ��� ������� ��� ������� � �������.
/// </summary>
public class NetworkScript : MonoBehaviour
{
    /// <summary>
    /// ������� �������.
    /// </summary>
    public NetworkDriver Driver;

    public NetworkRole Role;
    /// <summary>
    /// �������������� ������ � ���� �������.
    /// </summary>
    /// <param name="connections">������ ������ �����������.</param>
    /// <exception cref="UnityException">��������� � ������ �������������� ���������� ������� (������� ���� <see cref="NetworkRole.Client"/>).</exception>
    protected void CreateServer(out NativeList<NetworkConnection> connections)
    {
        if (Role == NetworkRole.Client)
        {
            throw new UnityException("This script cannot be initialized as server because it already has Client type.");
        }
        Role = NetworkRole.Server;
        Driver = NetworkDriver.Create();//������ ����� ������� ��� ��������.
        var endpoint = NetworkEndPoint.AnyIpv4;//��� � �����, ��� ����� ��� �����������.
        endpoint.Port = 9000;//������������ �������� ����.
        if (Driver.Bind(endpoint) != 0)//���� �� ���������� ������ ���� ����, ����������� ������.
            Debug.Log("Failed to bind to port 9000");
        else
            Driver.Listen();//���� �� ����, �� �������� ������������ ����.

        connections = new NativeList<NetworkConnection>(16, Allocator.Persistent);//������ ������ �����������.
    }
    /// <summary>
    /// �������������� ������ ��� �������.
    /// </summary>
    /// <param name="connection">������ ���� ����������� � �������.</param>
    protected void CreateClient(NetworkEndPoint endpoint, out NetworkConnection connection)
    {
        Driver = NetworkDriver.Create();//�������������� �������.

        
        connection = Driver.Connect(endpoint);//������ ����������� � ���������� ������� �� ���������� �����
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
    /// ���� ������� ������� � ������� ������-������.
    /// </summary>
    public enum NetworkRole
    {
        None,

        Server,

        Client
    }
}

