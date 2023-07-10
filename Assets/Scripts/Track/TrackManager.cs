using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    [SerializeField] private PartsFactory _partsFactory;
    [SerializeField] private List<GameObject> _activeParts;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private int _preloadPartsCount;
    [SerializeField] private float _lerpTime;
   
    private float _partSpawnTime;
    private float _playerSpeed;
    private Coroutine _spawningRoutine;

    private void Start()
    {
        SpawnPreloadParts();   
    }

    private void Update()
    {
        if (_activeParts.Count == 0)
        {
            return;
        }

        var firstPart = _activeParts[0];

        if (_spawnPoint.transform.position.z - firstPart.transform.position.z > firstPart.transform.localScale.z)
        {
            Destroy(firstPart);
            _activeParts.Remove(firstPart);
        }
    }

    private void SpawnPreloadParts()
    {
        var firstPart = _partsFactory.GetEmptyPart(transform);
        _activeParts.Add(firstPart);

        for (int i = 0; i < _preloadPartsCount; i++)
        {
            var lastElement = _activeParts[_activeParts.Count - 1].transform;

            var newPart = _partsFactory.GetRandomPart(transform);
            newPart.transform.position = new Vector3(0, 0, lastElement.position.z + lastElement.localScale.z);
            _activeParts.Add(newPart);
        }
    }

    private IEnumerator StartSpawningParts()
    {
        while (true)
        {
            yield return new WaitForSeconds(_partSpawnTime);
            var newPart = _partsFactory.GetRandomPart(transform);
            newPart.transform.position = _spawnPoint.position;
            StartCoroutine(NormalizePartPosition(newPart.transform));
            _activeParts.Add(newPart);
        }
    }

    private IEnumerator NormalizePartPosition(Transform trackPart)
    {
        var lastElement = _activeParts[_activeParts.Count - 1].transform;

        Vector3 startPos = trackPart.position;
        Vector3 endPos = new Vector3(0f, 0f, lastElement.position.z + lastElement.localScale.z);

        float startTime = 0;
        float endTime = _lerpTime;

        while (startTime < endTime)
        {
            trackPart.transform.position = Vector3.Lerp(startPos, endPos, (startTime / endTime));
            startTime += Time.deltaTime;
            yield return null;
        }

        trackPart.position = endPos;
        _spawnPoint.transform.position = new Vector3(_spawnPoint.position.x, _spawnPoint.position.y, _spawnPoint.position.z + lastElement.localScale.z);
    }

    public void SetPlayerSpeed(float speed)
    {
        _playerSpeed = speed;

        if (_activeParts.Count < 1)
        {
            return;
        }

        _partSpawnTime = _activeParts[_activeParts.Count - 1].transform.localScale.z / _playerSpeed;
    }

    public void StartSpawning()
    {
        _spawningRoutine = StartCoroutine(StartSpawningParts());
    }

    public void StopSpawning()
    {
        StopCoroutine(_spawningRoutine);
    }

    public void ResetTrack()
    {
        for (int i = 0; i <= _activeParts.Count - 1; i++)
        {
            Destroy(_activeParts[i]);
        }       

        _activeParts.Clear();
        _spawnPoint.position = new Vector3(0, _spawnPoint.position.y, 0);

        SpawnPreloadParts();
    }
}
