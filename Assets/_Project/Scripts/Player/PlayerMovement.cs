using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    [SerializeField]
    private Rigidbody rb;

    void Update()
    {
        rb.linearVelocity = new Vector3(Input.GetAxis("Horizontal") * _speed, 0, Input.GetAxis("Vertical") * _speed);
    }
}
