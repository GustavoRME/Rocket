using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decoupler : MonoBehaviour
{
    [SerializeField] private GameObject _toDocoupleStage = default;
    [SerializeField] private Rigidbody _upperStageRb = default;

    [Space]
    [SerializeField] private float _pushForce = 1.0f;

    public void DecoupleStage()
    {
        Rigidbody rb = _toDocoupleStage.AddComponent(typeof(Rigidbody)) as Rigidbody;
        Vector3 force = _pushForce * _toDocoupleStage.transform.forward;

        rb.velocity = _upperStageRb.velocity;
        rb.transform.SetParent(null);

        rb.AddForce(-force, ForceMode.Impulse);
        _upperStageRb.AddForce(force, ForceMode.Impulse);
    }
    
}
