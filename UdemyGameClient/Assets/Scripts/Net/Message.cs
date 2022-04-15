using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using System.Linq;
using Common;



public class Message : MonoBehaviour
{

    private byte[] data = new byte[1024];
    private int startIndex = 0;

    public byte[] Data
    {
        get { return data; }
    }
    public int StartIndex
    {
        get { return startIndex; }
    }
    public int RemainSize
    {
        get
        {
            return data.Length - startIndex;
        }
    }


    public void ReadMessage(int newDataAmount, Action<ActionCode, string> processDataCallback)
    {
        startIndex += newDataAmount;
        while (true)
        {
            if (startIndex <= 4) return;
            int count = BitConverter.ToInt32(data, 0);
            if (startIndex - 4 >= count)
            {
                ActionCode actionCode = (ActionCode)BitConverter.ToInt32(data, 4);

                string s = Encoding.UTF8.GetString(data, 8, count - 4);
                processDataCallback(actionCode, s);
                Array.Copy(data, count + 4, data, 0, startIndex - 4 - count);
                startIndex -= (count + 4);

            }
            else { break; }
        }



    }

    public static byte[] PackData(RequestCode requestData, string data)
    {
        byte[] requestCodeBytes = BitConverter.GetBytes((int)requestData);
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);
        int DataAmount = requestCodeBytes.Length + dataBytes.Length;
        byte[] DataAmountBytes = BitConverter.GetBytes(DataAmount);
        return DataAmountBytes.Concat(requestCodeBytes).ToArray().Concat(dataBytes).ToArray();
    }
    public static byte[] PackData(RequestCode requestData, ActionCode actionCode, string data)
    {
        byte[] requestCodeBytes = BitConverter.GetBytes((int)requestData);
        byte[] actionCodeBytes = BitConverter.GetBytes((int)actionCode);
        byte[] dataBytes = Encoding.UTF8.GetBytes(data);
        int DataAmount = requestCodeBytes.Length + dataBytes.Length + actionCodeBytes.Length;
        byte[] DataAmountBytes = BitConverter.GetBytes(DataAmount);
        byte[] newBytes = DataAmountBytes.Concat(requestCodeBytes).ToArray().Concat(actionCodeBytes).ToArray();
        return newBytes.Concat(dataBytes).ToArray();
    }

}


