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

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (EventSystem.IsPointerOverGameObject())
                return;

            var mousePosition = Input.mousePosition;
            var worldPosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.RaycastAll(worldPosition, Vector3.forward, 200);

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
            }
        }
    }

    /// <summary>
    /// Creates a new instance of a <see cref="Room"/> that is an Entrance.
    /// </summary>
    /// <param name="entranceId"></param>
    public Room InstantiateEntranceRoom(string entranceId = null)
    {
        entranceId ??= Room.GenerateEntranceId();
        var entrancePrefab = MapPrefabs.RoomsPrefabs.First(room => room.GetComponent<Room>().Id == entranceId);
        var roomGameObject = Instantiate(entrancePrefab, Vector3.zero, Quaternion.identity, MapContainer.transform);
        return roomGameObject.GetComponent<Room>();
    }

    /// <summary>
    /// Creates a new instance of a <see cref="Room"/>.
    /// </summary>
    /// <param name="roomId"></param>
    public Room InstantiateRoom(string roomId = null)
    {
        roomId ??= Room.GenerateRoomId();

        var roomPrefab = MapPrefabs.RoomsPrefabs.First(room => room.GetComponent<Room>().Id == roomId);
        var clickedDoor = LastClickable.Transform.GetComponent<Door>();

        var createdRoom = Instantiate(roomPrefab, Vector3.zero, Quaternion.identity, MapContainer.transform);
        
        PositionRoom(createdRoom, clickedDoor);

        return createdRoom.GetComponent<Room>();
    }

    /// <summary>
    /// Positions the newly created <see cref="Room"/>
    /// </summary>
    /// <param name="createdRoomGameObject"></param>
    /// <param name="referenceDoor"></param>
    private void PositionRoom(GameObject createdRoomGameObject, Door referenceDoor)
    {
        var createdRoomDoor = createdRoomGameObject.GetComponentInChildren<Door>();

        createdRoomDoor.ConnectedDoor = referenceDoor;
        referenceDoor.ConnectedDoor = createdRoomDoor;

        var doorPosition = CalculateDoorPosition(referenceDoor);
        var doorRotation = CalculateDoorRotation(referenceDoor);

        createdRoomDoor.transform.SetParent(MapContainer.transform);
        createdRoomGameObject.transform.SetParent(createdRoomDoor.transform);

        createdRoomDoor.transform.position = doorPosition;
        createdRoomDoor.transform.rotation = doorRotation;

        createdRoomGameObject.transform.SetParent(MapContainer.transform);
        createdRoomDoor.transform.SetParent(createdRoomGameObject.transform);
    }

    /// <summary>
    /// Calculates the new <see cref="Door"/> rotation based on the referenceDoor rotation.
    /// </summary>
    /// <param name="referenceDoor"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    private Quaternion CalculateDoorRotation(Door referenceDoor)
    {
        var clickedDoorRotationZAxis = Math.Floor(referenceDoor.transform.rotation.eulerAngles.z);

        switch (clickedDoorRotationZAxis)
        {
            case 0:
                return Quaternion.Euler(0, 0, 180);
            case 360:
                return Quaternion.Euler(0, 0, 180);
            case 180:
                return Quaternion.identity;
            case -180:
                return Quaternion.identity;
            case 90:
                return Quaternion.Euler(0, 0, 270);
            case -270:
                return Quaternion.Euler(0, 0, 270);
            case -90:
                return Quaternion.Euler(0, 0, 90);
            case 270:
                return Quaternion.Euler(0, 0, 90);
            default:
                throw new ArgumentException($"The clicked Door rotation Z axis is {clickedDoorRotationZAxis}.");
        }
    }

    /// <summary>
    /// Calculates the <see cref="Door"/> world position snapping it to the first decimal.
    /// </summary>
    /// <param name="referenceDoor"></param>
    /// <returns></returns>
    private Vector3 CalculateDoorPosition(Door referenceDoor) => new Vector3(SnapToFirstDecimal(referenceDoor.transform.position.x), SnapToFirstDecimal(referenceDoor.transform.position.y), referenceDoor.transform.position.z);

    /// <summary>
    /// Rounds a value to the first decimal.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private static float SnapToFirstDecimal(float value) => Mathf.Round(value * 10f) / 10f;
}
