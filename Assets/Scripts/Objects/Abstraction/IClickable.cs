using UnityEngine;

namespace Objects.Abstraction
{
    public interface IClickable
    {
        public Transform Transform { get; }
        public GameObject GameObject { get; }
        public void Clicked(Vector3 worldPosition, Vector3 screenPosition);
        public void ClickedOut();
    }
}
