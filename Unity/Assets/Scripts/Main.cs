using UnityEngine;
using System.Collections;
using TCWO_CSLibrary.Network;

public class Main 
{
    public static WorldServerConnection ServerConnection;

    public static bool SubsystemInitialized = false;

    public static void InitializeSubSystem()
    {
        ServerConnection = WorldServerConnection.Create();
        SubsystemInitialized = true;
    }

}
