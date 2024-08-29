using System.Collections.Generic;
using UnityEngine;

public class CubesSpawner : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private float _shardsChanceMultiplier = 0.5f;
    [SerializeField] private int _shardsMinNumber = 2;
    [SerializeField] private int _shardsMaxNumber = 6;
    [SerializeField] private float _force = 200;
    [SerializeField] private float _scaleMultiplier = 0.5f;
    [SerializeField] private int _initialCubesQuantity = 10;
    private List<Cube> _cubes = new List<Cube>();
    private System.Random _rand = new System.Random();

    public void Spawn(Cube explodingCube)
    {
        double randomDouble = _rand.NextDouble();

        if (randomDouble < explodingCube.ShardsChance)
        {
            int shardsNumber = _rand.Next(_shardsMinNumber, _shardsMaxNumber + 1);

            for (int i = 0; i < shardsNumber; i++)
            {
                CreateCube(explodingCube.transform.position, explodingCube.transform.lossyScale * _scaleMultiplier, explodingCube.ShardsChance * _shardsChanceMultiplier);
            }
        }
    }

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
            cube.CubeExploding -= Spawn;
            cube.CubeDisabled -= RemoveFromList;
        }
    }

    private void RemoveFromList(Cube cube)
    {
        if(_cubes.Contains(cube))///////////////////////////////////////////////////////////////////////////
        {
            _cubes.Remove(cube);
        }
    }

    private Cube CreateCube(Vector3 position, Vector3 scale, double chance = 1)
    {
        Vector3 force = new Vector3(_rand.Next(-100, 100), _rand.Next(-100, 100), _rand.Next(-100, 100)).normalized * _force;
        Cube newCube = Instantiate(_cubePrefab, position, Quaternion.Euler(Vector3.zero));
        _cubes.Add(newCube);
        newCube.transform.localScale = scale;
        newCube.SetShardsChance(chance);
        newCube.CubeExploding += Spawn;
        newCube.CubeDisabled += RemoveFromList;
        newCube.AddForce(force);
        return newCube;
    }
}
