using System.Collections.Generic;
using UnityEngine;

public class CubesSpawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private float _shardsChanceMultiplier = 0.5f;
    [SerializeField] private int _shardsMinNumber = 2;
    [SerializeField] private int _shardsMaxNumber = 6;
    [SerializeField] private float _forceValue = 200;
    [SerializeField] private float _scaleMultiplier = 0.5f;
    [SerializeField] private float _explosionRangeMultiplier = 2f;
    [SerializeField] private float _explosionForceMultiplier = 8f;
    [SerializeField] private int _initialCubesQuantity = 10;
    private List<Cube> _cubes = new List<Cube>();

    private void Start()
    {
        for(int i = 0; i < _initialCubesQuantity; i++)
        {
            CreateCube(transform.position, Vector3.one);
        }
    }

    private void OnDisable()
    {
        foreach (Cube cube in _cubes)
        {
            cube.Exploding -= Spawn;
            cube.Disabled -= RemoveFromList;
        }
    }

    private void Spawn(Cube explodingCube)
    {
        int shardsNumber = Random.Range(_shardsMinNumber, _shardsMaxNumber + 1);

        for (int i = 0; i < shardsNumber; i++)
        {
            CreateCube(explodingCube.transform.position, explodingCube.transform.lossyScale * _scaleMultiplier, explodingCube.ShardsChance * _shardsChanceMultiplier,
                explodingCube.ExplosionRange * _explosionRangeMultiplier, explodingCube.ExplosionForce * _explosionForceMultiplier);
        }
    }

    private void RemoveFromList(Cube cube)
    {
        if(_cubes.Contains(cube))
        {
            _cubes.Remove(cube);
        }

        cube.Exploding -= Spawn;
        cube.Disabled -= RemoveFromList;
    }

    private void CreateCube(Vector3 position, Vector3 scale, float chance, float explosionRange, float explosionForce)
    {
        Cube newCube = CreateCube(position, scale, chance);

        if (newCube.gameObject.TryGetComponent<Exploder>(out Exploder exploder))
        {
            exploder.Init(explosionRange, explosionForce);
        }
    }

    private Cube CreateCube(Vector3 position, Vector3 scale, float chance = 1)
    {
        Cube newCube = Instantiate(_cubePrefab, position, Quaternion.Euler(Vector3.zero));
        _cubes.Add(newCube);
        newCube.Init(scale, chance, _forceValue);
        newCube.Exploding += Spawn;
        newCube.Disabled += RemoveFromList;
        return newCube;
    }
}