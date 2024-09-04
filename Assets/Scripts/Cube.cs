using UnityEngine;
using System;

[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Exploder))]
public class Cube : MonoBehaviour
{
    private Renderer _renderer;
    private Rigidbody _rigidbody;
    private Exploder _exploder;

    public event Action<Cube> Exploding;
    public event Action<Cube> Disabled;

    public float ShardsChance { get; private set; }
    public float ExplosionRange => _exploder.Range;
    public float ExplosionForce => _exploder.MaxForce;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _rigidbody = GetComponent<Rigidbody>();
        _exploder = GetComponent<Exploder>();
    }

    private void OnDisable()
    {
        Disabled?.Invoke(this);
    }

    private void OnMouseUpAsButton()
    {
        Explode();
    }

    public void Init(Vector3 scale, float shardsChance, float forceValue)
    {
        ChangeColor();
        transform.localScale = scale;
        SetShardsChance(shardsChance);
        Vector3 force = UnityEngine.Random.insideUnitSphere.normalized * forceValue;
        AddForce(force);
    }

    private void SetShardsChance(float chance)
    {
        ShardsChance = chance;
    }

    private void ChangeColor()
    {
        _renderer.material.color = UnityEngine.Random.ColorHSV();
    }

    private void AddForce(Vector3 force)
    {
        _rigidbody.AddForce(force);
    }

    private void Explode()
    {
        float randomValue = UnityEngine.Random.value;

        if (randomValue < ShardsChance)
        {
            Exploding?.Invoke(this);
        }
        else
        {
            _exploder.Explode();
        }

        Destroy(gameObject);
    }
}
