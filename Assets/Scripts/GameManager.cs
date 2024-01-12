using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Objects;
using Objects.Abstraction;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

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

    private bool _isCreatedRoomColliding = false;

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

            if (UIManager.IsUIOpen)
                UIManager.HideUI();

            var mousePosition = Input.mousePosition;
            var worldPosition = MainCamera.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.RaycastAll(worldPosition, Vector3.forward, 200);

            if (hit.Length != 0)
            {
                var highestPriorityClickable = hit
                    .Where(raycastHit => raycastHit.collider.GetComponent<IClickable>() != null)
                    .Select(clickable => clickable.collider.GetComponent<IClickable>())
                    .OrderByDescending(clickable => clickable.Priority)
                    .First();

                if (LastClickable != highestPriorityClickable)
                {
                    LastClickable?.ClickedOut();
                    LastClickable = highestPriorityClickable;
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
    public IEnumerator InstantiateRoom(string roomId = null)
    {
        roomId ??= Room.GenerateRoomId();

        var temporaryRoomPrefab = MapPrefabs.RoomsInvisiblePrefabs.First(room => room.GetComponent<Room>().Id == roomId);
        var temporaryRoom = Instantiate(temporaryRoomPrefab, Vector3.zero, Quaternion.identity, MapContainer.transform);
        var clickedDoor = LastClickable.Transform.GetComponent<Door>();


        PositionRoom(temporaryRoom, clickedDoor);

        yield return StartCoroutine(CheckRoomCollisions(temporaryRoom));

        if (!_isCreatedRoomColliding)
        {
            var roomPrefab = MapPrefabs.RoomsPrefabs.First(room => room.GetComponent<Room>().Id == roomId);
            var createdRoom = Instantiate(roomPrefab, temporaryRoom.transform.position, temporaryRoom.transform.rotation, MapContainer.transform);

            var room = createdRoom.GetComponent<Room>();
            room.ParentRoom = LastClickable.GameObject.GetComponentInParent<Room>();
        }
        else
            UIManager.ShowRoomNotFittingMessage();

        Destroy(temporaryRoom);
    }

    /// <summary>
    /// This method waits for a FixedUpdate to complete and update the Collider information and then check for collisions.
    /// </summary>
    /// <param name="room"></param>
    /// <returns></returns>
    private IEnumerator CheckRoomCollisions(GameObject room)
    {
        yield return new WaitForFixedUpdate();
        _isCreatedRoomColliding = IsRoomCollidingWithAnotherRoom(room);
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
    private Vector3 CalculateDoorPosition(Door referenceDoor) => new(SnapToFirstDecimal(referenceDoor.transform.position.x), SnapToFirstDecimal(referenceDoor.transform.position.y), 0);

    /// <summary>
    /// Rounds a value to the first decimal.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    private static float SnapToFirstDecimal(float value) => Mathf.Round(value * 10f) / 10f;

    /// <summary>
    /// Checks weather the passed Room is colliding with another Room
    /// </summary>
    /// <param name="room"></param>
    /// <returns></returns>
    private static bool IsRoomCollidingWithAnotherRoom(GameObject room)
    {
        //Checks if the given GameObject is a Room
        var roomComponent = room.GetComponent<Room>();
        if (roomComponent == null)
            throw new Exception("The passed GameObject is not a Room.");

        var roomCollider = room.GetComponent<Collider2D>();
        var bounds = roomCollider.bounds;

        /* When calling the Physics2D.OverlapAreaAll() method, if I pass the LayerMask to filter only the Room GameObjects (LayerMask == 7) then the method will return an empty array,
         * if instead I do not pass the LayerMask then it finds all the overlapping colliders
         * then I filter them manually and call the IsTouching() method to only the colliders that are attached to a GameObject with LayerMask == "Room" */
        var overlapColliders = Physics2D.OverlapAreaAll(bounds.min, bounds.max);
        var roomLayerMask = LayerMask.NameToLayer("Room");

        var isOverlapping = overlapColliders.Any(collider =>collider.gameObject.layer == roomLayerMask && roomCollider.IsTouching(collider));
        return isOverlapping;
    }
}
