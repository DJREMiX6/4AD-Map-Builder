using System;
using Assets.Scripts.Objects.Abstraction;
using Objects.Abstraction;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace Objects
{
    public class Room : MonoBehaviour, IPlaceableItem, IClickable
    {
        private const int MIN_ID_VALUE = 1;
        private const int MAX_ID_VALUE = 7;

        [SerializeField] private bool _isRoom;
        [SerializeField] private bool _isEntrance;
        [SerializeField] private string _id;

        public bool IsRoom => _isRoom;
        public bool IsEntrance => _isEntrance;
        public string Id => _id;

        public Door[] Doors;

        public Transform Transform => transform;
        public GameObject GameObject => gameObject;

        private void Awake()
        {
            Doors = GetComponentsInChildren<Door>();
        }

        public void Clicked(Vector3 position, Vector3 screenPosition)
        {
            Debug.Log($"Clicked Room {Id}");
        }

        public void ClickedOut()
        {
            Debug.Log($"Clicked out of Room {Id}");
        }

        private static int GenerateIdPart() => (int)Random.Range(MIN_ID_VALUE, MAX_ID_VALUE);

        public static string GenerateEntranceId() => GenerateIdPart().ToString();

        public static string GenerateRoomId() => string.Concat(GenerateIdPart(), GenerateIdPart());

    }
}
