using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Text;
using Common;


namespace GameServer001.Servers
{
    class Room
    {
        enum RoomState
        {
            WaitingJoin,
            WaitinBattle,
            Battle,
            End
        }
        private List<Client> clientRoom = new List<Client>();
        private const int maxHp = 200;
        private RoomState state = RoomState.WaitingJoin;
        private Server server;
        public Room(Server server)
        {
            this.server = server;
        }
       
        public void AddClient(Client client)
        {
            clientRoom.Add(client);
            client.room = this;
            client.HP = maxHp;
            if (clientRoom.Count >= 2)
            {
                state = RoomState.WaitinBattle;
            }
        }
        public void QuitRoom(Client client)
        {
            server.RemoveRoom(this);
            if (client == clientRoom[0])
            {
                Close();
            }
            else
            {
                clientRoom.Remove(client);
            }
        }
        public void Close()
        {
           
        }
        public bool isWaitingJoin()
        {
            return state == RoomState.WaitingJoin;
        }
        public string GetHouseOwnerData()
        {
            return clientRoom[0].GetUserData();
        }
        public int GetId()
        {
            if (clientRoom.Count > 0)
            {
                return clientRoom[0].GetUserId();
            }
            return -1;
        }
        public string GetRoomData()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Client client in clientRoom)
            {
                sb.Append(client.GetUserData() + "|");
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            
            return sb.ToString();
        }
        public void BroadCastMessage(Client excludeClient,ActionCode actionCode,string data)
        {
            foreach(Client client in clientRoom)
            {
                if (excludeClient != client)
                {
                    server.SendResponse(client, actionCode, data);
                }
            }
        }
        
       
       public bool IsHouseOwner(Client client)
        {
            return client == clientRoom[0];
        }

        public void StartTimer()
        {
            new Thread(RunTimer).Start();
        }
        private void RunTimer()
        {
            Thread.Sleep(1000);
            for (int i = 3; i > 0; i--)
            {
                BroadCastMessage(null, ActionCode.StartTimer, i.ToString());
                Thread.Sleep(1000);
            }
            BroadCastMessage(null, ActionCode.StartPlay, "r");

        }
        public void TakeDamage(int damage,Client excludeClient)
        {
            bool isDead = false;
            foreach(Client client in clientRoom)
            {
                if (client != excludeClient)
                {
                    if (client.TakeDamage(damage))
                    {
                        isDead = true;
                    }
                }
            }
            if (isDead == false) return;
            else
            {
                foreach(Client client in clientRoom)
                {
                    if (client.isDead())
                    {
                        client.Send(ActionCode.GameOver, ((int)ReturnCode.Fail).ToString());
                    }
                    else
                    {
                        client.Send(ActionCode.GameOver, ((int)ReturnCode.Success).ToString());
                    }
                }
            }
        }
      
       
        
    }
}
