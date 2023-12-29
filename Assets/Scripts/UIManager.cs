using Objects;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject ContextMenuPanel;
    public GameObject AddNewRoomPanel;
    public TMP_InputField GenerateRoomIdText;
    public GameManager GameManager;

    public bool IsUIOpen { get; private set; }

    public static UIManager Current => _instance;
    private static UIManager _instance;

    void Awake()
    {
        _instance = this;
    }

    public void ShowContextMenu()
    {
        IsUIOpen = true;
        ContextMenuPanel.SetActive(true);
    }

    public void HideContextMenu()
    {
        ContextMenuPanel.SetActive(false);
        IsUIOpen = false;
    }

    public void ShowAddNewRoomPanel()
    {
        AddNewRoomPanel.SetActive(true);
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
        IsUIOpen = false;
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
            return;
        }

        GameManager.InstantiateRoom(roomId);
        HideUI();
        GenerateRoomIdText.text = string.Empty;
    }
}
