using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using Common;
using MySql.Data.MySqlClient;
using GameServer001.Tool;
using GameServer001.Model;

namespace GameServer001.Servers
{
    class Client
    {
        private Socket clientSocket;
        private Server server;
        private Message msg = new Message();
        private User user;
        private Result result;
        public int HP { get; set; }
        public Client() { }
        private Room room1;
        public Room room
        {
            set { room1 = value; }
            get { return room1; }
        }
        private MySqlConnection mysqlConn;
        public MySqlConnection MysqlConn
        {
            get { return mysqlConn; }
        }
        public Client(Socket clientSocket,Server server)
        {
            this.clientSocket = clientSocket;
            this.server = server;
            mysqlConn = ConnHelper.Connect();
        }
        public void Start()
        {
            if (clientSocket == null || clientSocket.Connected == false) return;
            clientSocket.BeginReceive(msg.Data, msg.StartIndex, msg.RemainSize, SocketFlags.None, ReceiveCallBack, null);
        }
        public void Send(ActionCode actionCode, string data)
        {
            
                byte[] bytes = Message.PackData(actionCode, data);
            if(clientSocket!=null&&bytes!=null)
                clientSocket.Send(bytes);
           
        }
        private void ReceiveCallBack(IAsyncResult ar)
        {
            
                int count = clientSocket.EndReceive(ar);
                if (count == 0)
                {
                    Close();
                }
                msg.ReadMessage(count, OnProcessMessage);
                Start();
           
        }
        private void Close()
        {
            ConnHelper.CloseConnection(mysqlConn);
            if (clientSocket != null)
            {
                clientSocket.Close();
            }
            server.RemoveClient(this);
            if (room1 != null)
            {
                room1.QuitRoom(this);
            }
        }
        private void OnProcessMessage(RequestCode requestCode,ActionCode actionCode,string data)
        {
            server.HandleRequest(requestCode, actionCode, data, this);
        }
       public void SetUserData(User user, Result result)
        {
            this.user = user;
            this.result = result;
        }
        public string GetUserData()
        {
            return user.Id+","+user.Username +","+ result.TotalCount +","+ result.WinCount;
        }
        public int GetUserId()
        {
            return user.Id;
        }
       public bool IsHouseOwner()
        {
            return room1.IsHouseOwner(this);
        }
        public bool TakeDamage(int damage)
        {
            HP -= damage;
            HP = Math.Max(HP, 0);
            if (HP <= 0) return true;
            else return false;
        }
        public bool isDead()
        {
            return HP <= 0;
        }
    }
}
