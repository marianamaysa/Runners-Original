using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine.UI;
using UnityEngine;

public class SimpleMatchmaking : MonoBehaviour
{
    // Codigo tirado/adaptado do canal TARODEV no YouTube
    [SerializeField] public GameObject bgMenu;
    [SerializeField] public GameObject criarButton;
    [SerializeField] public GameObject buscarButton;
    [SerializeField] public GameObject codeButton;
    [SerializeField] private Text codeText;
    [SerializeField] private GameObject botoes;
    [SerializeField] public GameObject keyCodeText;

    private Lobby connectedLobby;
    private QueryResponse lobbies;
    private UnityTransport transport;
    private string playerId;
    private const string JoinCodeKey = "j";
    [SerializeField] private int playerToStart = 2;
    float timer;
    private bool sceneChanged = false;
    private bool stopScene = false; 

    private void Awake()
    {
        transport = FindObjectOfType<UnityTransport>();
    }

    public async void CreateOrJoinLobby()
    {
        bgMenu.SetActive(false);
        criarButton.SetActive(false);
        buscarButton.SetActive(false);
        keyCodeText.SetActive(true);
        codeButton.SetActive(false);
        await Authenticate();
        connectedLobby = await QuickJoinLobby() ?? await CreateLobby();
        if (connectedLobby != null) botoes.SetActive(false);
    }

    private void Update()
    {
        if (!stopScene)
        {
            timer += Time.deltaTime;
            {
                if (timer > 5 && !sceneChanged) // tenta mudar a cena a cada 5 segundos
                {
                    //ActivateCanvas();
                    ChangeScene();
                    //sceneChanged = true; 
                    timer = 0f;
                }
            }
        }

    }
    private async Task<Lobby> QuickJoinLobby()
    {
        try
        {
            var lobby = await Lobbies.Instance.QuickJoinLobbyAsync();

            var a = await RelayService.Instance.JoinAllocationAsync(lobby.Data[JoinCodeKey].Value);

            SetTransportAsClient(a);

            NetworkManager.Singleton.StartClient();
            return lobby;
        }
        catch (Exception e)
        {
            Debug.Log(e + "No lobbies");
            return null;
        }
    }

    private void SetTransportAsClient(JoinAllocation a)
    {
        transport.SetClientRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData, a.HostConnectionData);
    }

    private async Task<Lobby> CreateLobby()
    {
        try
        {
            const int maxPlayers = 2;

            var a = await RelayService.Instance.CreateAllocationAsync(maxPlayers);
            var joinCode = await RelayService.Instance.GetJoinCodeAsync(a.AllocationId);
            codeText.text = "Join code: " + joinCode;

            var options = new CreateLobbyOptions
            {
                Data = new Dictionary<string, DataObject> { { JoinCodeKey, new DataObject(DataObject.VisibilityOptions.Public, joinCode) } }
            };
            var lobby = await Lobbies.Instance.CreateLobbyAsync("runnersScene", maxPlayers, options);

            StartCoroutine(HeartbeatLobbyCoroutine(lobby.Id, 15));

            transport.SetHostRelayData(a.RelayServer.IpV4, (ushort)a.RelayServer.Port, a.AllocationIdBytes, a.Key, a.ConnectionData);

            NetworkManager.Singleton.StartHost();
            Debug.Log(joinCode);
            return lobby;
        }
        catch (Exception e)
        {
            Debug.Log($"Failed creating a lobby: {e}");
            return null;
        }
    }

    private static IEnumerator HeartbeatLobbyCoroutine(string lobbyId, float waitTimeSeconds)
    {
        var delay = new WaitForSecondsRealtime(waitTimeSeconds);
        while (true)
        {
            Lobbies.Instance.SendHeartbeatPingAsync(lobbyId);
            yield return delay;
        }
    }
    private async Task Authenticate()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
        playerId = AuthenticationService.Instance.PlayerId;
    }


    private async void ChangeScene()
    {
        if (connectedLobby != null && !sceneChanged)
        {
            Lobby lobby = await Lobbies.Instance.GetLobbyAsync(connectedLobby.Id);
            Debug.Log("PLAYERS: " + lobby.Players.Count);

            if (lobby.Players.Count == 2)
            {
                // Cena com nome gameplay -- mudar de acordo com seu jogo
                NetworkManager.Singleton.SceneManager.LoadScene("runnersScene", UnityEngine.SceneManagement.LoadSceneMode.Single);
                sceneChanged = true;

                var players = GameObject.FindGameObjectsWithTag("Player");
                foreach (var player in players)
                {
                    var playerMovement = player.GetComponent<PlayerMovement>();
                    if (playerMovement != null)
                    {
                        playerMovement.IsInLobby(false);
                    }
                }
            }
        }
    }


    private void OnDestroy()
    {
        try
        {
            StopAllCoroutines();
            if (connectedLobby != null)
            {
                if (connectedLobby.HostId == playerId) Lobbies.Instance.DeleteLobbyAsync(connectedLobby.Id);
                else Lobbies.Instance.RemovePlayerAsync(connectedLobby.Id, playerId);
            }
        }
        catch (Exception e)
        {
            Debug.Log($"error shutting down lobby {e}");
        }
    }
}
