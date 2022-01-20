using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parachute : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb = default;
    [SerializeField] private MeshRenderer _parachuteRender = default;
    
    [Tooltip("Speed decrease/second")] [SerializeField] private float _speed = 5.0f;
    [SerializeField] private float _rotationSpeed = 5.0f;
    [SerializeField] private float _maxDrag = 20.0f;
    
    [Header("Parachute rotation speed")]
    [SerializeField] private float _minSpeed = 15f;
    [SerializeField] private float _maxSpeed = 30f;
    
    [Tooltip("Use to rotation slow down when rocket is looking down")] 
    [SerializeField] private float _decreaseSpeedRotation = 5.0f;

    [Header("Swing simulation")]
    [SerializeField] private Vector3 _diving00 = Vector3.zero;
    [SerializeField] private Vector3 _diving01 = Vector3.zero;

    private readonly Vector3[] _divings = new Vector3[2];
    [SerializeField] private int _index;
    
    [SerializeField] private bool _isTurnDown;               //Its true when the Nose Cone is looking for floor
    [SerializeField] private float _turnSpeed;               //Use as speed rotation when Nose Cone is already looking down. Simulate a pendulum move

    private void Awake()
    {
        _divings[0] = _diving00;
        _divings[1] = _diving01;
        enabled = false;
    }

    private void Update()
    {
        //Slow down fall speed smoothly
        if(_rb.drag < _maxDrag)
        {
            _rb.drag += _speed * Time.deltaTime;
        }
#if UNITY_EDITOR
        else
        {
            _rb.drag = _maxDrag;
        }
#endif

        //Check if rocket is already complete the rotation
        if (transform.eulerAngles.z >= 150f && !_isTurnDown)
            _isTurnDown = true;

        if(transform.eulerAngles == _diving00)
        {
            _index = 1;
        }
        else if(transform.eulerAngles == _diving01)
        {
            _index = 0;
        }

        //Rotation has three different speed. The '_rotationSpeed' is used while rocket isn't turned down
        //The range of speed, '_minSpeed' and '_maxSpeed', is used to simulate the swing while is falling.
        if (_isTurnDown)
        {
            _turnSpeed = IsOnRange(_turnSpeed) ? 
                GetRandomSpeed() 
                : _turnSpeed - _decreaseSpeedRotation * Time.deltaTime;
        }
        else
        {
            _turnSpeed = _rotationSpeed;
        }

        transform.eulerAngles = Vector3.MoveTowards(transform.eulerAngles, _divings[_index], _turnSpeed * Time.deltaTime);
    }

    public void OpenParachute()
    {
        _rb.angularDrag = 0.0f;
        _parachuteRender.enabled = true;
        enabled = true;
    }

    public void CloseParachute()
    {
        _parachuteRender.enabled = false;
        enabled = false;
    }

    private float GetRandomSpeed()
    {
        return Random.Range(_minSpeed, _maxSpeed);
    }

    private bool IsOnRange(float speed)
    {
        return speed >= _minSpeed && speed <= _maxSpeed;
    }
}
