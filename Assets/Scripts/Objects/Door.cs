using Assets.Scripts.Objects.Abstraction;
using Objects.Abstraction;
using UnityEngine;

namespace Objects
{
    public class Door : MonoBehaviour, IClickable
    {
        private static Color _selectedColor = Color.white;
        private static Color _unselectedColor => new Color(0.706f, 0.706f, 0.706f);

        private SpriteRenderer _spriteRenderer;
        private UIManager _uiManager;

        public Room ParentRoom;
        public Door ConnectedDoor;

        public Transform Transform => transform;
        public GameObject GameObject => gameObject;

        void Awake()
        {
            ParentRoom = GetComponentInParent<Room>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _uiManager = UIManager.Current;
        }

        public void Clicked(Vector3 worldPosition, Vector3 screenPosition)
        {
            _spriteRenderer.color = _selectedColor;
            _uiManager.ShowContextMenu();
        }

        public void ClickedOut()
        {
            _spriteRenderer.color = _unselectedColor;
            _uiManager.HideContextMenu();
        }

    }
}
