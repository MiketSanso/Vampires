using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private CharacterController _target;
    [SerializeField] private Vector3 _offset = new Vector3(0, 2, -5);

    private void LateUpdate()
    {
        if (_target != null)
        {
            transform.position = _target.transform.position + _offset;
        }
    }
}