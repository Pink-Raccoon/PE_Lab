using System;
using UnityEngine;

public class RopeController : MonoBehaviour
{
    public Vector3 suspensionPoint = new(0, 0, 0);
    public bool active = false;

    public float airDensity = 1.2f;
    public float dragCoefficient = 1.1f;
    public float crossSectionalArea = 1.5f * 1.5f;

    public Transform rope;
    public Collider floor;

    private Rigidbody _rigidbody;


    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

    }

    private float CalculateRadialComponentOfWeightForce(float angle) =>
        _rigidbody.mass * Physics.gravity.magnitude * Mathf.Cos(angle);

    private float CalculateCentripetalForce(Vector3 velocity, float radius) =>
        _rigidbody.mass * velocity.sqrMagnitude / radius;

    private Vector3 CalculateRopeForce(Vector3 velocity, float angle, float radius)
    {
        var gravitationalForce = CalculateRadialComponentOfWeightForce(angle);
        var centripetalForce = CalculateCentripetalForce(velocity, radius);
        var totalForce = centripetalForce + gravitationalForce;

        var horizontalForce = totalForce * Mathf.Sin(angle);
        var verticalForce = totalForce * Mathf.Cos(angle);

        return new Vector3(horizontalForce, verticalForce);
    }

    private Vector3 CalculateFrictionForce(Vector3 velocity) =>
        -0.5f * crossSectionalArea * airDensity * dragCoefficient * velocity.sqrMagnitude * velocity.normalized;

    private void FixedUpdate()
    {
        var position = transform.position;
        var velocity = _rigidbody.velocity;

        if (!active)
        {
            if (position.x < suspensionPoint.x)
                return;

            active = true;
            // floor.enabled = false;
        }

        var diff = suspensionPoint - new Vector3(position.x, position.y, 0);
        var angle = Mathf.Atan(diff.x / diff.y);

        var ropeForce = CalculateRopeForce(velocity, angle, 6f);
        var frictionForce = CalculateFrictionForce(velocity);

        var resultingForce = ropeForce + frictionForce;
        _rigidbody.AddForce(resultingForce);

        if (!ReferenceEquals(rope, null))
        {
            rope.position = (suspensionPoint - position) / 2 + position;
            rope.rotation = Quaternion.Euler(0, 0, -Mathf.Rad2Deg * angle);
        }



        Debug.DrawLine(position, position + ropeForce, Color.green);
        Debug.DrawLine(position, position + frictionForce, Color.yellow);

        var gravitationalForce = _rigidbody.mass * Physics.gravity;
        var globalResultingForce = resultingForce + gravitationalForce;
        Debug.DrawLine(position, position + globalResultingForce, Color.red);
    }
}