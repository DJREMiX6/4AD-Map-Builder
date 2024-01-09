using System.Linq;
using Objects;
using Objects.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public GameObject ContextMenuPanel;
    public GameObject AddNewRoomPanel;
    public TMP_InputField GenerateRoomIdText;
    public GameManager GameManager;
    public EventSystem EventSystem;
    public UIMessage[] UIMessages;

    public bool IsUIOpen { get; private set; }

    public static UIManager Current => _instance;
    private static UIManager _instance;

    void Awake()
    {
        _instance = this;
    }

    public void ShowRoomNotFittingMessage()
    {
        IsUIOpen = true;
        var uiMessageItem = UIMessages.FirstOrDefault(uiMessage => uiMessage.MessageId == UIMessagesId.RoomDoesNotFitMessage);
        if (uiMessageItem == null)
        {
            Debug.LogError($"The UIMessage with Id: {UIMessagesId.RoomDoesNotFitMessage} is null.");
            IsUIOpen = false;
            return;
        }
        uiMessageItem.gameObject.SetActive(true);
    }

    public void HideMessages()
    {
        foreach(var uiMessage in UIMessages)
            uiMessage.gameObject.SetActive(false);
        IsUIOpen = false;
    }

    public void ShowContextMenu()
    {
        IsUIOpen = true;
        ContextMenuPanel.SetActive(true);
    }

    public void HideContextMenu()
    {
        IsUIOpen = false;
        ContextMenuPanel.SetActive(false);
    }

    public void ShowAddNewRoomPanel()
    {
        IsUIOpen = true;
        AddNewRoomPanel.SetActive(true); 
        if (EventSystem.currentSelectedGameObject != GenerateRoomIdText.gameObject)
            EventSystem.SetSelectedGameObject(GenerateRoomIdText.gameObject);
    }

    public void HideAddNewRoomPanel()
    {
        AddNewRoomPanel.SetActive(false);
        IsUIOpen = false;
    }

    public void HideUI()
    {
        HideContextMenu();
        HideAddNewRoomPanel();
        HideMessages();
    }

    public void OnAddRoomClicked()
    {
        HideContextMenu();
        ShowAddNewRoomPanel();
    }

    public void OnGenerateRandomRoomNumber()
    {
        var roomId = Room.GenerateRoomId();
        GenerateRoomIdText.text = roomId;
    }

    public void CreateRoom()
    {
        var roomId = GenerateRoomIdText.text;
        if (!Room.IsValidRoomId(roomId))
        {
            GenerateRoomIdText.text = string.Empty;
            if(EventSystem.currentSelectedGameObject != GenerateRoomIdText.gameObject)
                EventSystem.SetSelectedGameObject(GenerateRoomIdText.gameObject);
            return;
        }

        StartCoroutine(GameManager.InstantiateRoom(roomId));
        HideUI();
        GenerateRoomIdText.text = string.Empty;
    }
}
