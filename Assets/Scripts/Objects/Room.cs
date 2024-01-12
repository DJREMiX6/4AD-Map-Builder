using System;
using System.Collections.Generic;
using Assets.Scripts.Objects.Abstraction;
using Objects.Abstraction;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace Objects
{
    public class Room : MonoBehaviour, IPlaceableItem, IClickable
    {

        #region CONSTS

        private const int MIN_ID_VALUE = 1;
        private const int MAX_ID_VALUE = 7;

        #endregion CONSTS

        #region PRIVATE FIELDS

        [SerializeField] private RoomType _roomType;
        [SerializeField] private string _id;
        private Door[] _doors;

        #endregion PRIVATE FIELDS

        #region PUBLIC PROPS

        public RoomType RoomType => _roomType;
        public string Id => _id;
        public Door[] Doors => _doors;

        public Room ParentRoom { get; set; }
        public List<Room> ChildRooms { get; set; } = new(); //TODO add childRooms
        public List<Room> ChildRooms = new();
        #endregion PUBLIC PROPS

        #region IClickable PROPS IMPLEMENTATION

        public Transform Transform => transform;
        public GameObject GameObject => gameObject;

        #endregion IClickable PROPS IMPLEMENTATION

        public int Priority { get; } = 0;

        private void Awake()
        {
            _doors = GetComponentsInChildren<Door>();
        }

        #region IClickable METHODS IMPLEMENTATION

        public void Clicked(Vector3 position, Vector3 screenPosition)
        {
            //TODO
        }

        public void ClickedOut()
        {
            //TODO
        }

        #endregion IClickable METHODS IMPLEMENTATION

        private static int GenerateIdPart() => Random.Range(MIN_ID_VALUE, MAX_ID_VALUE);

        public static string GenerateEntranceId() => GenerateIdPart().ToString();

        public static string GenerateRoomId() => string.Concat(GenerateIdPart(), GenerateIdPart());

        //TODO add check for invalid numbers like 17 or 1
        public static bool IsValidRoomId(string roomId) => roomId.Length == 2 && char.IsDigit(roomId[0]);
    }
}
