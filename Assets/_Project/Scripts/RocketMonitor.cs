using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RocketMonitor : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb = default;
    [SerializeField] private Transform _noseCone = default;
    [SerializeField] private float _raycastDistance = 3.5f;

    [Space]
    public UnityEvent<float> _OnReachMaximumHeight;

    public float Speed => _rb.velocity.magnitude;
    public float CurrentHeight => _rb.transform.position.y;
    public bool IsCloseFromFloor => _isCloseFromFloor;
    public bool IsAccelerating => _isAccelerating;

    [SerializeField] private bool _isAccelerating;
    private bool _isCloseFromFloor;
    private bool _isReachedMaximumHeight;

    private float _currSpeed;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_noseCone.transform.position, _noseCone.transform.position + (_noseCone.transform.forward * _raycastDistance));
    }

    public void UpdateMonitor()
    {
        CheckSpeed();
        CheckCloseFromFloor();
        CheckMaximumHeight();
    }

    private void CheckSpeed()
    {
        //Because of little tiny difference between these two values,
        //I need check if they are bigger than 0.01f, else not, keep same value
        _isAccelerating = Mathf.Abs(Speed - _currSpeed) > 0.01f ? Speed > _currSpeed : _isAccelerating;  
        _currSpeed = Speed;
    }

    private void CheckCloseFromFloor()
    {
        Ray ray = new Ray(_noseCone.transform.position, _noseCone.transform.forward);
        _isCloseFromFloor = Physics.Raycast(ray, _raycastDistance);
    }

    private void CheckMaximumHeight()
    {
        if (!_isReachedMaximumHeight && _rb.velocity.y < 0.0f && CurrentHeight > 1.0f)
        {
            _OnReachMaximumHeight?.Invoke(CurrentHeight);
            _isReachedMaximumHeight = true;
            Debug.Log("Reached maximum height");
        }
    }
}
