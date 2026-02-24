using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float turnSpeed = 360f;

    [SerializeField] private InputActionReference moveAction;

    private Vector3 _input;

    private void OnEnable()
    {
        moveAction.action.Enable();
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
    }

    void Update()
    {
        Vector2 input2D = moveAction.action.ReadValue<Vector2>();
        _input = new Vector3(input2D.x, 0, input2D.y);

        Look();
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Look()
    {
        if (_input.sqrMagnitude > 0.01f)
        {
            Vector3 isoDirection = _input.ToIso();

            Quaternion rot = Quaternion.LookRotation(isoDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                rot,
                turnSpeed * Time.deltaTime
            );
        }
    }

    void Move()
    {
        if (_input.sqrMagnitude < 0.01f)
            return;

        Vector3 moveDir = transform.forward;

        _rb.MovePosition(
            _rb.position + moveDir * _speed * Time.fixedDeltaTime
        );
    }
}