using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public enum ActionCode
    {
        None,
        Login,
        Register,
        CreateRoom,
        ListRoom,
        JoinRoom,
        UpdateRoom,
        StartGame,
        StartTimer,
        StartPlay,
        Move,
        Shoot,
        Attack,
        GameOver,
          QuitRoom
    }
}
