using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubesFactory : MonoBehaviour
{
    [SerializeField] private Cube _cubePrefab;

    public Cube GetCube(Transform point, Transform parent)
    {
        var cube = Instantiate(_cubePrefab, point.position, point.rotation, parent);        

        return cube;
    }
}
