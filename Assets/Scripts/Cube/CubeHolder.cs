using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CubeHolder : MonoBehaviour
{
    [SerializeField] private CubesFactory _cubesFactory;    
    [SerializeField] private List<Cube> _collectedCubes;
    [SerializeField] private List<Cube> _lostCubes;    
    [SerializeField] private float _distanceToRelease;
    [SerializeField] private float _onCreatePositionOffset;
    [SerializeField] private Transform _lostCubesHolder;

    private bool _isCubeLost = false;
    private ObjectPool<Cube> _cubesPool;

    public event Action<float> CubeCollected;
    public event Action CubeLost;
    public event Action HolderEmpty;

    private void Awake()
    {
        _cubesPool = new ObjectPool<Cube>(CreateCube, actionOnGet: OnGetCube, actionOnRelease: OnReleaseCube, collectionCheck: true);
    }

    private void Start()
    {
        _cubesPool.Get();
    }

    private void Update()
    {
        if (_lostCubes.Count == 0)
        {
            return;
        }

        for (int i = 0; i < _lostCubes.Count; i++)
        {
            var distance = Vector3.Distance(_lostCubes[i].transform.position, transform.position);

            if (distance >= _distanceToRelease)
            {
                _cubesPool.Release(_lostCubes[i]);                
            }
        }

        if (_isCubeLost)
        {
            _isCubeLost = false;
            UpdateCubesMass();
            CubeLost?.Invoke();            
        }
    }

    public void ResetHolder()
    {
        if (_collectedCubes.Count != 0)
        {
            foreach (var cube in _collectedCubes)
            {
                Destroy(cube.gameObject);
            }

            _collectedCubes.Clear();
        }

        _cubesPool.Get();
    }

    private Cube CreateCube()
    {
        var newCube = _cubesFactory.GetCube(transform, transform);
        newCube.gameObject.SetActive(false);

        return newCube; 
    }

    private void OnGetCube(Cube cube)
    {
        cube.CubeCollected += OnCubeCollected;
        cube.CubeGotCollision += OnCubeGotCollision;
        cube.gameObject.transform.SetParent(transform);
        cube.transform.position = transform.position;
        cube.UpdateConstraints();
        UpdateCubePosition(cube.transform);        
        cube.gameObject.SetActive(true);
        _collectedCubes.Add(cube);

        CalculateCubesMass();
    }

    private void OnReleaseCube(Cube cube)
    {
        cube.gameObject.SetActive(false);
        cube.SetMassToDefault();
        _lostCubes.Remove(cube);
    }

    private void UpdateCubePosition(Transform cube)
    {
        if (_collectedCubes.Count == 0)
        {
            return;
        }

        var upperCube = _collectedCubes[_collectedCubes.Count - 1].transform;

        cube.position = new Vector3(transform.position.x, upperCube.position.y + (upperCube.localScale.y + _onCreatePositionOffset), transform.position.z);
    }

    private void CalculateCubesMass()
    {
        if (_collectedCubes.Count != 0)
        {
            for (int i = 0; i < _collectedCubes.Count - 1; i++)
            {
                float mass = _collectedCubes[i].CubeMass;

                for (int j = i + 1; j < _collectedCubes.Count; j++)
                {
                    mass += _collectedCubes[j].CubeMass;    
                }

                _collectedCubes[i].SetMass(mass);
            }
        }
    }

    private void UpdateCubesMass()
    {
        for (int i = 0; i < _collectedCubes.Count - 1; i++)
        {
            _collectedCubes[i].SetMass(_collectedCubes[i + 1].CubeMass * 2);
        }
    }

    private void OnCubeCollected()
    {
        _cubesPool.Get();

        var lastElementYScale = _collectedCubes[_collectedCubes.Count - 1].transform.localScale.y;

        CubeCollected?.Invoke(lastElementYScale);
    }

    private void OnCubeGotCollision(Cube cube)
    {
        cube.CubeCollected -= OnCubeCollected;
        cube.CubeGotCollision -= OnCubeGotCollision;
        cube.gameObject.transform.SetParent(_lostCubesHolder);
        _collectedCubes.Remove(cube);
        _lostCubes.Add(cube);

        if (_collectedCubes.Count == 0)
        {
            HolderEmpty?.Invoke();
        }

        _isCubeLost = true;
    }
}
