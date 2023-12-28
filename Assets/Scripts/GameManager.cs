using System;
using Assets.Scripts.Objects.Abstraction;
using System.Linq;
using JetBrains.Annotations;
using Objects;
using Objects.Abstraction;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance => _instance;
    [CanBeNull] private static GameManager _instance;

    public Camera MainCamera;
    public MapPrefabs MapPrefabs;
    public GameObject MapContainer;
    public EventSystem EventSystem;
    public UIManager UIManager;
    public IClickable LastClickable;

    void Start()
    {
        _instance = this;
        InstantiateEntranceRoom();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            var mousePosition = Input.mousePosition;
            var worldPosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.RaycastAll(worldPosition, Vector3.forward, 200);

            if (EventSystem.IsPointerOverGameObject())
                return;

            if (hit.Length != 0)
            {
                if(UIManager.IsUIOpen)
                    UIManager.HideUI();

                var clickable = hit[^1].collider.gameObject.GetComponent<IClickable>();
                if (LastClickable != clickable)
                {
                    LastClickable?.ClickedOut();
                    LastClickable = clickable;
                }
                LastClickable?.Clicked(worldPosition, mousePosition);
            }
            else
            {
                LastClickable?.ClickedOut();
                LastClickable = null;
                Debug.Log("Clicked nowhere");
            }
        }
    }

    public void InstantiateEntranceRoom()
    {
        var entranceId = Room.GenerateEntranceId();
        var entrancePrefab = MapPrefabs.RoomsPrefabs.First(room => room.GetComponent<Room>().Id == entranceId);
        Instantiate(entrancePrefab, Vector3.zero, Quaternion.identity, MapContainer.transform);
    }

    public void InstantiateRoom(string roomId)
    {
        var roomPrefab = MapPrefabs.RoomsPrefabs.First(room => room.GetComponent<Room>().Id == roomId);
        var clickedDoor = LastClickable.Transform.GetComponent<Door>();

        var createdRoom = Instantiate(roomPrefab, Vector3.zero, Quaternion.identity, MapContainer.transform);
        var createdRoomDoor = createdRoom.GetComponentInChildren<Door>();
        
        createdRoomDoor.ConnectedDoor = clickedDoor;
        clickedDoor.ConnectedDoor = createdRoomDoor;

        createdRoomDoor.transform.SetParent(MapContainer.transform);
        createdRoom.transform.SetParent(createdRoomDoor.transform);

        int clickedDoorRotationZAxis = (int)clickedDoor.transform.rotation.eulerAngles.z;
        int newDoorZRotation = 0;

        switch (clickedDoorRotationZAxis)
        {
            case 0:
                newDoorZRotation = 180;
                break;
            case 360:
                newDoorZRotation = 180;
                break;
            case 180:
                newDoorZRotation = 0;
                break;
            case -180:
                newDoorZRotation = 0;
                break;
            case 90:
                newDoorZRotation = -90;
                break;
            case -270:
                newDoorZRotation = -90;
                break;
            case -90:
                newDoorZRotation = 90;
                break;
            case 270:
                newDoorZRotation = 90;
                break;
            default:
                throw new ArgumentException($"The clicked Door rotation Z axis is {clickedDoorRotationZAxis}.");
        }
        var newPosition = new Vector3(Mathf.Round(clickedDoor.transform.position.x * 10f) / 10f, Mathf.Round(clickedDoor.transform.position.y * 10f) / 10f, clickedDoor.transform.position.z);
        createdRoomDoor.transform.position = newPosition;
        createdRoomDoor.transform.rotation = Quaternion.Euler(0, 0, newDoorZRotation);

        createdRoom.transform.SetParent(MapContainer.transform);
        createdRoomDoor.transform.SetParent(createdRoom.transform);
    }
}
