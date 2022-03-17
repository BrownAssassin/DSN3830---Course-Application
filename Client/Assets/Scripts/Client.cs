using UnityEngine;

// Non-Unity Libraries
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class Client : MonoBehaviour
{
    public Transform myCube;
    private static byte[] outBuffer = new byte[12];
    private static IPEndPoint remoteEP;
    private static Socket clientSoc;
    private float[] position;
    private float[] prevPosition;
    private float elapsedTime;

    public static void RunClient()
    {
        try
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            remoteEP = new IPEndPoint(ip, 11111);

            clientSoc = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        }
        catch (Exception e)
        {
            Debug.Log("Exception: " + e.ToString());
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        myCube = GameObject.Find("Cube").transform;

        RunClient();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            outBuffer = Encoding.UTF8.GetBytes("shutdown");
            clientSoc.SendTo(outBuffer, remoteEP);

            clientSoc.Shutdown(SocketShutdown.Both);
            clientSoc.Close();

            Application.Quit();
        }
        else
        {
            elapsedTime += Time.deltaTime;

            position = new float[] {myCube.position.x, myCube.position.y, myCube.position.z};

            // Interval set at 25ms & check for movement
            if ((elapsedTime > 0.025f) && (position != prevPosition))
            {
                Buffer.BlockCopy(position, 0, outBuffer, 0, outBuffer.Length);
                clientSoc.SendTo(outBuffer, remoteEP);

                prevPosition = position;
                elapsedTime = 0f;
            }
        }
    }
}
