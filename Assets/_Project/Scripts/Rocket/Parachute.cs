using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parachute : MonoBehaviour
{
    [SerializeField] private Rigidbody _rocketRigidbody = default;
    [SerializeField] private Rigidbody _parachuteRigidbody = default;

    [Space]
    [SerializeField] private Transform _noseCone = default;
    [SerializeField] private Transform _parentRocket = default;
    
    [Header("Speed and Limits")]
    [Tooltip("Slow down fall speed/s")]     [SerializeField] private float _slowDownSpeed = 5.0f;
    [Tooltip("Slow down rotation speed/s")] [SerializeField] private float _slowDownAngular = 10.0f;
    [SerializeField] private float _maxLinearDrag = 20.0f;
    [SerializeField] private float _maxAngularDrag = 8.0f;
   
    private float _startDrag;
    private float _startAngularDrag;

    public bool IsSlowedDown { get; private set; }
    public bool IsOpened { get; private set; }

    private void Awake()
    {
        _startDrag = _rocketRigidbody.drag;
        _startAngularDrag = _parachuteRigidbody.angularDrag;
        enabled = false;
    }

    public void UpdateMe()
    {
        //Slow down fall speed smoothly
        if(_rocketRigidbody.drag < _maxLinearDrag)
        {
            _rocketRigidbody.drag += _slowDownSpeed * Time.deltaTime;
        }

        //Slow down rotation speed smoothly
        if(_parachuteRigidbody.angularDrag < _maxAngularDrag)
        {
            _parachuteRigidbody.angularDrag += _slowDownAngular * Time.deltaTime;
        }

        IsSlowedDown = _rocketRigidbody.drag >= _maxLinearDrag && _parachuteRigidbody.angularDrag >= _maxAngularDrag;
    }

    public void OpenParachute()
    {
        if (IsOpened)
            return;

        _noseCone.SetParent(_parachuteRigidbody.transform);
        _parachuteRigidbody.gameObject.SetActive(true);
        IsOpened = true;
    }

    public void CloseParachute()
    {
        if (!IsOpened)
            return;

        IsOpened = false;
        _rocketRigidbody.drag = _startDrag;
        _rocketRigidbody.angularDrag = _startAngularDrag;
        _noseCone.SetParent(_parentRocket);
        _parachuteRigidbody.gameObject.SetActive(false);
    }
}
