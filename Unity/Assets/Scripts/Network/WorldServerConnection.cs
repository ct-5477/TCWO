using UnityEngine;
using System.Collections;
using TCWO_CSLibrary.Network;

public class WorldServerConnection : Connection
{
    protected override void onConnect()
    {
        Debug.Log("Connected to server.");
    }

    public static WorldServerConnection Create()
    {
        WorldServerConnection con = new WorldServerConnection();
        Debug.Log("Connecting to server ...");
        con.Connect("127.0.0.1", 10501);
        return con;
    }

    protected override void log(string message)
    {
        Debug.Log(message);
    }
}
