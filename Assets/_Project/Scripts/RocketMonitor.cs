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
    public UnityEvent _OnReachMaximumHeight;

    public float Speed => _rb.velocity.magnitude;
    public float CurrentHeight => _rb.transform.position.y;

    public bool IsCloseFromFloor { get; set; }

    private bool _isReachedMaximumHeight;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_noseCone.transform.position, _noseCone.transform.position + (_noseCone.transform.forward * _raycastDistance));
    }

    public void UpdateMonitor()
    {
        if(!_isReachedMaximumHeight && _rb.velocity.y <= 0.0f)
        {
            _OnReachMaximumHeight?.Invoke();
            _isReachedMaximumHeight = true;
        }

        Ray ray = new Ray(_noseCone.transform.position, _noseCone.transform.forward);
        IsCloseFromFloor = Physics.Raycast(ray, _raycastDistance);
    }
}
