using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb = default;

    [Header("Speed up")]
    [SerializeField] private float _accelerationY = 3.5f;
    [SerializeField] private float _accelerationZ = 1.0f;
    [SerializeField] private float _accelerationTime = 15f;
    
    [Header("Rotation")]
    [Tooltip("Start the rotation based on acceleration time.How much smaller is the number more early will start.")]
    [SerializeField] [Range(0.0f, 1.0f)] private float _startRotation = 0.0f;
    [SerializeField] private float _maxRotation = 35.0f;

    [SerializeField] private Vector3 _accelerationForce;
    [SerializeField] private Vector3 _torqueForce;
    
    private Vector3 _startPos;
    private float _time;

    private bool _isAddedTorque;

    private void Awake()
    {
        _startPos = transform.position;
        enabled = false;
    }

    private void Update()
    {       
        float timeElapsed = Time.time - _time;
        bool isAcceleration = timeElapsed < _accelerationTime;
        
        if (isAcceleration)
        {
            _accelerationForce += transform.up * _accelerationY * Time.deltaTime;

            bool canRotate = timeElapsed >= _accelerationTime * _startRotation && transform.eulerAngles.z < _maxRotation;
            if (canRotate)
            {
                _rb.angularDrag = 0.05f;
                _torqueForce += Vector3.forward * _accelerationZ * Time.deltaTime;
            }
            else
            {
                _rb.angularDrag = 20f;
            }

            Debug.Log("Its accelerating...");
            _rb.AddForce(_accelerationForce, ForceMode.Acceleration);
            _rb.AddTorque(_torqueForce);
        }
    }

    public void Launch()
    {
        _time = Time.time;
        _rb.useGravity = true;
        enabled = true;
    }

    public void AddTorque()
    {
        _torqueForce += Vector3.forward * _accelerationZ * Time.deltaTime;
        _rb.AddTorque(_torqueForce, ForceMode.Acceleration);
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
