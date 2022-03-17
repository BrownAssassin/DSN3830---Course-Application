using UnityEngine;

// Non-Unity Libraries
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;


public class Server : MonoBehaviour
{
    private Transform myCube;
    private static byte[] inBuffer = new byte[12];
    private static Socket server;
    private static EndPoint remoteClient;
    private static int rec = 0;
    private float[] position = new float[12];

    public static void RunServer()
    {
        try
        {
            IPHostEntry hostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            Debug.Log($"Server name: {hostInfo.HostName} | IP: {ip}");

            IPEndPoint localEP = new IPEndPoint(ip, 11111);

            server = new Socket(ip.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

            // Create an EP to capture the info from the sending client
            IPEndPoint client = new IPEndPoint(IPAddress.Any, 0);
            remoteClient = (EndPoint)client;

            server.Bind(localEP);
            Debug.Log("Waiting for data...");
        }
        catch (Exception e)
        {
            Debug.Log("Exception: " + e.ToString());
        }
    }

    public void UpdateServer()
    {
        try
        {
            rec = server.ReceiveFrom(inBuffer, ref remoteClient);
        }
        catch (SocketException e)
        {
            Debug.Log("Exception: " + e.ToString());
        }

        if (Encoding.UTF8.GetString(inBuffer, 0, rec) == "shutdown")
        {
            server.Shutdown(SocketShutdown.Both);
            server.Close();

            Application.Quit();
        }
        else
        {
            Buffer.BlockCopy(inBuffer, 0, position, 0, rec);

            if (rec > 0)
            {
                myCube.position = new Vector3(position[0], position[1], position[2]);
            }
            
            Debug.Log($"Received from: {remoteClient.ToString()} | Data: ({position[0]}, {position[1]}, {position[2]})");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        myCube = GameObject.Find("Cube").transform;

        RunServer();

        // Non-Blocking Mode
        server.Blocking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            server.Shutdown(SocketShutdown.Both);
            server.Close();

            Application.Quit();
        }

        UpdateServer();
    }
}
