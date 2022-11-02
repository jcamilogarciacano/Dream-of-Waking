using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float WalkVelocity = 16;
    public float JumpVelocity = 32;
    public float Gravity = 112;
    public float Acceleration = 64;

    private CharacterController _controller;
    private Vector2 _currentVelocity;
    private bool _isJumping;
    private int _walkDirection;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            _walkDirection = 1;
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            _walkDirection = -1;
        }
        else
        {
            _walkDirection = 0;
        }

        _isJumping = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);

        var newV = new Vector2(GetHorizontalVelocity(), GetVerticalVelocity());
        _currentVelocity = newV;

        if (_walkDirection < 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.x, 270, transform.rotation.z), 0.2f);
        }
        else if (_walkDirection > 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(transform.rotation.x, 90, transform.rotation.z), 0.2f);
        }

        _controller.Move(newV * Time.deltaTime);
    }

    private float GetHorizontalVelocity()
    {
        var v = _currentVelocity.x;

        var maxVelocity = WalkVelocity * _walkDirection;

        if ((_controller.collisionFlags & CollisionFlags.Sides) != CollisionFlags.None)
        {
            v = 0;
        }

        if (_walkDirection == 0 && (_controller.collisionFlags & CollisionFlags.Below) == CollisionFlags.None)
        {
            return v;
        }

        // If no acceleration or maxVelocity is reached
        if (Acceleration == 0 || (v == maxVelocity))
            return v;

        var accelerationWithDirection =
            (maxVelocity < 0 || (v > 0 && maxVelocity == 0))
            ? -Acceleration
            : Acceleration;

        v += accelerationWithDirection * Time.deltaTime;

        // If cross from negative to positive velocity or vice versa when direction == 0
        if ((_walkDirection == 0)
            && ((accelerationWithDirection < 0 && v < 0)
                || (accelerationWithDirection > 0 && v > 0)))
        {
            return 0;
        }

        // If overshoot negative maxVelocity
        if (maxVelocity < 0)
        {
            if (v <= maxVelocity)
                return maxVelocity;
        }

        // If overshoot positive maxVelocity
        if (maxVelocity > 0)
        {
            if (v >= maxVelocity)
                return maxVelocity;
        }

        return Mathf.Min(WalkVelocity, Mathf.Max(-WalkVelocity, v));
    }

    private float GetVerticalVelocity()
    {
        var v = _currentVelocity.y;
        // Resets velocity when on a surface
        // Set as negative to prevent bumping when moving down slopes
        if ((_controller.collisionFlags & CollisionFlags.Below) != CollisionFlags.None
            || (_controller.collisionFlags & CollisionFlags.Above) != CollisionFlags.None)
        {
            v = -10;
        }

        // Apply gravity
        v -= Gravity * Time.deltaTime;

        // Apply jump
        if (_isJumping)
        {
            v = JumpVelocity;
        }

        return v;
    }
}
