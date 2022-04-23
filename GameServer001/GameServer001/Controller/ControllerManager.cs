using System;
using System.Collections.Generic;
using System.Text;
using Common;
using System.Reflection;
using GameServer001.Servers;

namespace GameServer001.Controller
{
    class ControllerManager
    {
        private Dictionary<RequestCode, BaseController> controllerDict = new Dictionary<RequestCode, BaseController>();
        private Server server;

        public ControllerManager(Server server)
        {
            
            this.server = server;
            Init();
        }
        private void Init()
        {
            DefaultController defaultController = new DefaultController();
            controllerDict.Add(defaultController.RequestCode, new DefaultController());
            controllerDict.Add(RequestCode.User, new UserController());
            controllerDict.Add(RequestCode.Room, new RoomController());
            controllerDict.Add(RequestCode.Game, new GameController());
        }
        public void HandleRequest(RequestCode requestCode,ActionCode actionCode,string data,Client client)
        {
            BaseController controller;
           bool isGet= controllerDict.TryGetValue(requestCode, out controller);
            if (isGet == false)
            {
                Console.WriteLine("Can't found controller for" + requestCode);
                return;
            }
           string methodName= Enum.GetName(typeof(ActionCode), actionCode);
            MethodInfo mi= controller.GetType().GetMethod(methodName);
            if (mi == null)
            {
                Console.WriteLine("there is no corresponding processing method" + methodName);
                return;
            }
            object[] parameters = new object[] { data,client,server };
           object o= mi.Invoke(controller, parameters);
            if (o == null) return;
            server.SendResponse(client, actionCode, o as string);
        }

    }
  //controllerManager=>Server=>Client
}
