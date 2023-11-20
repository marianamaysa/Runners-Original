using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MenuConnection : MonoBehaviour
{
    public void ClientConnect()
    {
        NetworkManager.Singleton.StartClient();
    }

    public void HostConnect()
    {
        NetworkManager.Singleton.StartHost();
    }
}
