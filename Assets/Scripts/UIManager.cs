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

    public void ShowContextMenu(Vector3 position)
    {
        IsUIOpen = true;
        ContextMenuPanel.transform.position = position;
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
        var text = GenerateRoomIdText.text;
        if (text.Length != 2 || !char.IsDigit(text[0]))
        {
            GenerateRoomIdText.text = "";
            return;
        }

        GameManager.InstantiateRoom(GenerateRoomIdText.text);
        HideUI();
        GenerateRoomIdText.text = "";
    }
}
