using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData : MonoBehaviour {
    public int id { get; set; }
   public string username { set; get; }
    public int totalCount { set; get; }
    public int winCount { set; get; }

    public UserData(string userData)
    {
        string[] strs = userData.Split(',');
        this.id = int.Parse(strs[0]);
        this.username = strs[1];
        this.totalCount = int.Parse(strs[2]);
        this.winCount = int.Parse(strs[3]);
    }

    public UserData(int id,string username,int totalCount,int winCount)
    {
        this.id = id;
        this.username = username;
        this.totalCount = totalCount;
        this.winCount = winCount;
    }
    public UserData( string username, int totalCount, int winCount)
    {
        this.username = username;
        this.totalCount = totalCount;
        this.winCount = winCount;
    }


}
