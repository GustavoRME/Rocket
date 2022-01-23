using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RocketMonitor : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb = default;

    [Space]
    public UnityEvent _OnReachMaximumHeight;

    public float Speed => _rb.velocity.magnitude;
    public float CurrentHeight => _rb.transform.position.y;

    private bool _isReachedMaximumHeight;

    public void UpdateMonitor()
    {
        if(!_isReachedMaximumHeight && _rb.velocity.y <= 0.0f)
        {
            _OnReachMaximumHeight?.Invoke();
            _isReachedMaximumHeight = true;
        }
    }
}
