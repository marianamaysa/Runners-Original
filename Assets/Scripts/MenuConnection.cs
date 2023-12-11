using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MenuConnection : MonoBehaviour
{
    [SerializeField] public GameObject bgMenu;
    public void ClientConnect()
    {
        NetworkManager.Singleton.StartClient();
        bgMenu.SetActive(false);
    }

    public void HostConnect()
    {
        NetworkManager.Singleton.StartHost();
        bgMenu.SetActive(false);
    }
}
