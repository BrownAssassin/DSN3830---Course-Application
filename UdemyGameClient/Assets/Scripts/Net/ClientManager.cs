using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using Common;

public class ClientManager : BaseManager
{

    public ClientManager(GameFacade facade) : base(facade) { }

    public const string IP = "127.0.0.1";
    public const int PORT = 6688;
    Socket clientSocket;
    Message msg = new Message();
    public override void OnInit()
    {
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            clientSocket.Connect(IP, PORT);

            Start();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    private void Start()
    {
        clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallback, null);
    }

    private void ReceiveCallback(IAsyncResult ar)
    {
        if (clientSocket == null || clientSocket.Connected == false) return;
        int count = clientSocket.EndReceive(ar);
            msg.ReadMessage(count, OnProcessDataCallback);
            Start();
      
    }

    private void OnProcessDataCallback(ActionCode actionCode, string data)
    {
        facade.HandleResponse(actionCode, data);
    }
    public override void OnDestroy()
    {
        try
        {
            clientSocket.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
    public void SendRequest(RequestCode requestCode, ActionCode actionCode, string data)
    {
        byte[] bytes = Message.PackData(requestCode, actionCode, data);
        clientSocket.Send(bytes);
    }
}

