using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PartsFactory : MonoBehaviour
{
    [SerializeField] private GameObject _emptyPart;
    [SerializeField] private List<GameObject> _trackParts;

    public GameObject GetRandomPart(Transform parent)
    {
        return Instantiate(_trackParts[Random.Range(0, _trackParts.Count)], parent);
    }

    public GameObject GetEmptyPart(Transform parent)
    {
        return Instantiate(_emptyPart, parent);
    }
}
