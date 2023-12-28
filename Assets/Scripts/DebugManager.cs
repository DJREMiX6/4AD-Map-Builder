using System;
using System.Linq;
using Objects;
using UnityEngine;
using Random = UnityEngine.Random;

public class DebugManager : MonoBehaviour
{

    public GameManager GameManager;

    public bool SpawnRoomsOnLClick = false;
    public bool GenerateRandomRooms = false;
    public uint RandomRoomsCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SpawnRoomsOnLClick)
            SpawnRoomOnLClick();
        if (GenerateRandomRooms)
        {
            GenerateRandomRooms = false;
            try
            {
                //TODO TEST WHY THROWS EXCEPTION
                for (var i = 0; i < RandomRoomsCount; i++)
                {
                    var roomId = Room.GenerateRoomId();
                    InstantiateRoom(roomId);
                }
            }
            catch
            {
                Debug.Log("Encountered room with 1 door");
            }
        }
    }

    private void SpawnRoomOnLClick()
    {
        /*Used to test the creation of rooms*/
        if (Input.GetMouseButtonDown(0))
        {
            var entranceIdPart1 = ((int)Random.Range(1, 7)).ToString();
            var entranceIdPart2 = ((int)Random.Range(1, 7)).ToString();
            var entranceId = entranceIdPart1 + entranceIdPart2;
            var entrancePrefab = GameManager.MapPrefabs.RoomsPrefabs.First(room => room.GetComponent<Room>().Id == entranceId);
            var mouseScreenToWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var newInstancePosition = new Vector3(mouseScreenToWorldPoint.x, mouseScreenToWorldPoint.y, 0);
            Instantiate(entrancePrefab, newInstancePosition, Quaternion.identity, GameManager.MapContainer.transform);
        }
    }

    private void InstantiateRoom(string roomId)
    {
        var roomPrefab = GameManager.MapPrefabs.RoomsPrefabs.First(room => room.GetComponent<Room>().Id == roomId);
        var clickedDoor = GameManager.LastClickable.Transform.GetComponent<Door>();

        var createdRoom = Instantiate(roomPrefab, Vector3.zero, Quaternion.identity, GameManager.MapContainer.transform);
        var createdRoomDoor = createdRoom.GetComponentInChildren<Door>();

        createdRoomDoor.ConnectedDoor = clickedDoor;
        clickedDoor.ConnectedDoor = createdRoomDoor;

        createdRoomDoor.transform.SetParent(GameManager.MapContainer.transform);
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

        createdRoom.transform.SetParent(GameManager.MapContainer.transform);
        createdRoomDoor.transform.SetParent(createdRoom.transform);

        var nextLastClickable = createdRoom.GetComponent<Room>().Doors.FirstOrDefault(door => door != clickedDoor);
        if (nextLastClickable != null)
            GameManager.LastClickable = nextLastClickable;
        else
            throw new Exception();
    }
}
