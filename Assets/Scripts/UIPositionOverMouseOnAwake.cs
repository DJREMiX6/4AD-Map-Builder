using UnityEngine;

namespace Assets.Scripts
{
    public class UIPositionOverMouseOnAwake : MonoBehaviour
    {
        void OnEnable()
        {
            transform.position = Input.mousePosition;
        }
    }
}
