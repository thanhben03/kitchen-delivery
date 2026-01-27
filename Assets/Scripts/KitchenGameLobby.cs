using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KitchenGameLobby : MonoBehaviour
{
    public static KitchenGameLobby Instance { get; private set; }

    public event Action OnCreateLobbyStarted;
    public event Action OnCreateLobbyFailed;
    public event Action OnJoinStarted;
    public event Action OnQuickJoinFailed;
    public event Action OnJoinFailed;
    public event Action<List<Lobby>> OnLobbyListChanged;

    private Lobby joinedLobby;
    private float heartBeatTimer;
    private float listLobbiesTimer;

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);

        InitializeUnityAuthentication();

    }

    private void Update()
    {
        HandleHeartBeat();
        HandlePeriodicListLobbies();
    }

    private void HandlePeriodicListLobbies()
    {
        if (joinedLobby == null &&
            UnityServices.State == ServicesInitializationState.Initialized &&
            AuthenticationService.Instance.IsSignedIn &&
            SceneManager.GetActiveScene().name == Loader.Scene.LobbyScene.ToString())
        {

            listLobbiesTimer -= Time.deltaTime;
            if (listLobbiesTimer <= 0f)
            {
                float listLobbiesTimerMax = 3f;
                listLobbiesTimer = listLobbiesTimerMax;
                ListLobbies();
            }
        }
    }


    private void HandleHeartBeat()
    {
        if (IsLobbyHost())
        {
            heartBeatTimer -= Time.deltaTime;
            if (heartBeatTimer < 0f)
            {
                float heartBeatTimerMax = 15f;
                heartBeatTimer = heartBeatTimerMax;
                LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
            }
        }
    }

    private bool IsLobbyHost()
    {
        return joinedLobby != null && AuthenticationService.Instance.PlayerId == joinedLobby.HostId;
    }

    private async void InitializeUnityAuthentication()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            InitializationOptions initializationOptions = new InitializationOptions();
            //initializationOptions.SetProfile(Random.Range(0, 100000).ToString());

            await UnityServices.InitializeAsync(initializationOptions);
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

        }

    }

    private async void ListLobbies()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Filters = new List<QueryFilter> {
                  new QueryFilter(QueryFilter.FieldOptions.AvailableSlots, "0", QueryFilter.OpOptions.GT)
             }
            };
            QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync(queryLobbiesOptions);

            OnLobbyListChanged?.Invoke(queryResponse.Results);

        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }

    }

    public async void CreateLobby(string lobbyName, bool isPrivate)
    {
        OnCreateLobbyStarted?.Invoke();
        try
        {
            joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, KitchenGameMultiplayer.MAX_PLAYER_AMOUNT, new CreateLobbyOptions
            {
                IsPrivate = isPrivate
            });

            KitchenGameMultiplayer.Instance.StartHost();
            Loader.LoadNetwork(Loader.Scene.CharacterSelectScene);
        }
        catch (LobbyServiceException e)
        {
            OnCreateLobbyFailed?.Invoke();
            Debug.Log(e);
        }
    }

    public async void QuickJoin()
    {
        OnJoinStarted?.Invoke();
        try
        {
            joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();
            KitchenGameMultiplayer.Instance.StartClient();


        }
        catch (LobbyServiceException e)
        {
            OnQuickJoinFailed?.Invoke();
            Debug.Log(e);
        }
    }

    public async void JoinWithId(string lobbyId)
    {
        OnJoinStarted?.Invoke();
        try
        {
            joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
            KitchenGameMultiplayer.Instance.StartClient();

        }
        catch (LobbyServiceException e)
        {
            OnJoinFailed?.Invoke();
            Debug.Log(e);
        }
    }

    public async void JoinWithCode(string lobbyCode)
    {
        OnJoinStarted?.Invoke();
        try
        {
            joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);
            KitchenGameMultiplayer.Instance.StartClient();

        }
        catch (LobbyServiceException e)
        {
            OnJoinFailed?.Invoke();
            Debug.Log(e);
        }
    }

    public async void DeleteLobby()
    {
        if (joinedLobby != null)
        {
            try
            {

                await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);

                joinedLobby = null;

            }
            catch (LobbyServiceException e)
            {

                Debug.Log(e);
            }
        }
        
    }

    public async void LeaveLobby()
    {
        if (joinedLobby != null)
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);

                joinedLobby = null;
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }

    public async void KickPlayer(string playerId)
    {
        if (IsLobbyHost())
        {
            try
            {
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, playerId);
            }
            catch (LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
    }

    public Lobby GetLobby()
    {
        return joinedLobby;
    }

}
