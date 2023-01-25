using System;
using System.Collections.Generic;
using Alteruna;
using Alteruna.Trinity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Application = UnityEngine.Application;

public class UiManager : AttributesSync
{
    [SerializeField] private Button _startGameButton;
    [SerializeField] private TextMeshProUGUI _lobbyHeader;

    [SerializeField] private GameObject[] _playerPanels;
    
    private static UiManager _instance;
    
    public static UiManager Instance
    {
        get
        {
            if (_instance == null)
            {
                throw new NullReferenceException("_instance is null!");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    private void Start()
    {
        Multiplayer.RegisterRemoteProcedure("UpdateLobbyUiRemote", UpdateLobbyUiRemote);
    }
    

    public void ShowPanel(GameObject panel)
    {
        panel.SetActive(true);
    }

    public void HidePanel(GameObject panel)
    {
        panel.SetActive(false);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void SetLobbyName()
    {
        _lobbyHeader.text = Multiplayer.Instance.CurrentRoom.Name +"'s Room";
    }
    
    public void LeaveRoom()
    {
        Multiplayer.Instance.CurrentRoom.Leave();
    }

    public void CanStart(bool canStart)
    {
        _startGameButton.interactable = canStart;
    }

    public void UpdateLobbyUi()
    {
        Multiplayer.InvokeRemoteProcedure("UpdateLobbyUiRemote", UserId.AllInclusive);
    }
    
    public void UpdateLobbyUiRemote(ushort fromUser, ProcedureParameters parameters, uint callId, ITransportStreamReader processor)
    {
        GameManager.Instance.PrintDebug("Ui-Manager", "Updating ui, my user id: " + Multiplayer.Instance.Me.Index);
        foreach (var user in GameManager.Instance.GetUserListInRoom())
        {
            _playerPanels[user.Index].GetComponentInChildren<TextMeshProUGUI>().text = user.Name;
            _playerPanels[user.Index].SetActive(true);
        }
    }
}
