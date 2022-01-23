using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb = default;
    [SerializeField] private ParticleSystem _particle = default;
    [SerializeField] private AudioSource _audioSource = default;

    [Header("Speed up")]
    [SerializeField] private float _accelerationY = 3.5f;
    [SerializeField] private float _accelerationX = 5.0f;

    [Header("Rotation")]
    [Tooltip("Start the rotation based on acceleration time.How much smaller is the number more early will start.")]
    [SerializeField] [Range(0.0f, 1.0f)] private float _startRotation = 0.0f;

    [Space]
    [SerializeField] private float _minVolume = 0.2f;
    [SerializeField] private float _maxVolume = 1.0f;
    [SerializeField] private float _volumeSpeed = 2.0f;

    private Vector3 _accelerationForce;
    private Vector3 _startPos;

    private float _accelerationDuration = 15f;
    private float _time;

    private bool _canRotate;
    public bool IsAccelerating { get; private set; }
    public bool IsStopEffects { get; private set; }

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

    public void UpdateMe()
    {
        float timeElapsed = Time.time - _time;
        IsAccelerating = timeElapsed < _accelerationDuration;

        if (IsAccelerating)
        {
            _canRotate = timeElapsed >= _accelerationDuration * _startRotation;
            if (_canRotate)
            {
                _accelerationForce += transform.right * _accelerationX * Time.deltaTime;
            }

            _accelerationForce += transform.up * _accelerationY * Time.deltaTime;
            _rb.AddForce(_accelerationForce, ForceMode.Acceleration);
        }

        if (_canRotate)
        {
            Vector3 rigidBodyDirection = _rb.velocity.normalized;
            Vector3 localDirection = transform.up;

            float angleDiff = Vector3.SignedAngle(localDirection, rigidBodyDirection, Vector3.forward);
            transform.Rotate(Vector3.forward, angleDiff);
        }

        if (_audioSource.isPlaying)
        {
            _audioSource.volume = _audioSource.volume < _maxVolume ?
                _audioSource.volume + _volumeSpeed * Time.deltaTime :
                 _maxVolume;
        }
    }

    public void Launch(float duration)
    {
        _accelerationDuration = duration;
        _time = Time.time;
        _rb.useGravity = true;
        _audioSource.volume = _minVolume;
        _audioSource.Play();
        _particle.Play();
        IsAccelerating = true;
    }    

    public void StopEffects()
    {
        _audioSource.Stop();
        _particle.Stop();
        IsStopEffects = true;
    }

    public void Restart()
    {
        StopEffects();
        _rb.useGravity = false;
        _rb.velocity = Vector3.zero;
        _rb.drag = 0.0f;
        _accelerationForce = Vector3.zero;
        transform.position = _startPos;
    }
}
