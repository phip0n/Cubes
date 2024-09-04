using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploder : MonoBehaviour
{
    [SerializeField] private float _range = 10;
    [SerializeField] private float _maxForce = 400;

    public float Range => _range;
    public float MaxForce => _maxForce;

    public void Init(float range, float maxForce)
    {
        _range = range;
        _maxForce = maxForce;
    }

    public void Explode()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _range);

        foreach (Collider hit in hits)
        {
            if (hit.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            {
                float force = _maxForce * (_range - (hit.transform.position - transform.position).magnitude);
                rigidbody.AddForce((hit.transform.position - transform.position).normalized * force);
            }
        }
    }
}
