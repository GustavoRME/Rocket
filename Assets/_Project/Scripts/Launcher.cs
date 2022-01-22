using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb = default;
    [SerializeField] private ParticleSystem _particle = default;

    [Header("Speed up")]
    [SerializeField] private float _accelerationY = 3.5f;
    [SerializeField] private float _accelerationX = 5.0f;
    [SerializeField] private float _accelerationTime = 15f;
    
    [Header("Rotation")]
    [Tooltip("Start the rotation based on acceleration time.How much smaller is the number more early will start.")]
    [SerializeField] [Range(0.0f, 1.0f)] private float _startRotation = 0.0f;

    private Vector3 _accelerationForce;
    private Vector3 _startPos;
    
    private float _time;
    private bool _isLaunching;
    private bool _canRotate;

    private void Awake()
    {
        _startPos = transform.position;
        enabled = false;
    }

    private void OnDrawGizmos()
    {
        if (_rb == null)
            return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, _rb.velocity.normalized + transform.position);
    }

    private void Update()
    {
        float timeElapsed = Time.time - _time;
        bool isAccelerating = timeElapsed < _accelerationTime;

        if (isAccelerating)
        {
            _canRotate = timeElapsed >= _accelerationTime * _startRotation;
            if (_canRotate)
            {
                _accelerationForce += transform.right * _accelerationX * Time.deltaTime;
            }

            _accelerationForce += transform.up * _accelerationY * Time.deltaTime;
            _rb.AddForce(_accelerationForce, ForceMode.Acceleration);
            Debug.Log("Its accelerating...");
        }

        if (_canRotate)
        {
            Vector3 rigidBodyDirection = _rb.velocity.normalized;
            Vector3 localDirection = transform.up;

            float angleDiff = Vector3.SignedAngle(localDirection, rigidBodyDirection, Vector3.forward);
            transform.Rotate(Vector3.forward, angleDiff);
        }
    }

    public void Launch()
    {
        _time = Time.time;
        _rb.useGravity = true;
        _isLaunching = true;
        _particle.Play();

        enabled = true;
    }

    public void Restart()
    {
        _rb.useGravity = false;
        _rb.velocity = Vector3.zero;
        _rb.drag = 0.0f;
        _accelerationForce = Vector3.zero;
        transform.position = _startPos;
        enabled = false;
    }
}
