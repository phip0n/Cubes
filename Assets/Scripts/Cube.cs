using UnityEngine;
using UnityEngine.Events;

public class Cube : MonoBehaviour
{
    public UnityEvent<Cube> Exploding;
    public UnityEvent<Cube> Disabled;

    public float ShardsChance { get; private set; }

    private void Start()
    {
        ChangeColor();
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
        transform.localScale = scale;
        SetShardsChance(shardsChance);
        Vector3 force = Random.insideUnitSphere.normalized * forceValue;
        AddForce(force);
    }

    public void SetShardsChance(float chance)
    {
        ShardsChance = chance;
    }

    public void ChangeColor()
    {
        if (TryGetComponent<Renderer>(out Renderer renderer))
        {
            renderer.material.color = Random.ColorHSV();
        }
    }

    private void AddForce(Vector3 force)
    {
        if (TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
        {
            rigidbody.AddForce(force);
        }
    }

    private void Explode()
    {
        float randomValue = Random.value;

        if (randomValue < ShardsChance)
        {
            Exploding?.Invoke(this);
        }

        Destroy(gameObject);
    }
}
