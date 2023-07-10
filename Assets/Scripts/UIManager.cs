using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Pool;
using DG.Tweening;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _startScreen;
    [SerializeField] private Button _startButton;
    [SerializeField] private GameObject _loseScreen;
    [SerializeField] private Button _restartButton;

    [Space][Header("Collected Text Settings")][Space]
    [SerializeField] private GameObject _collectedCubeTextPrefab;    
    [SerializeField] private float _endPos;
    [SerializeField] private float _animTime;    
    [SerializeField] private float _distanceToRelease;
    [SerializeField] private List<GameObject> _activeTextList;

    private Transform _player;    
    private ObjectPool<GameObject> _textPool;

    public event Action GameStarted;
    public event Action GameRestarted;

    private void Awake()
    {
        _startButton.onClick.AddListener(OnGameStarted);
        _restartButton.onClick.AddListener(OnGameRestarted);

        _textPool = new ObjectPool<GameObject>(CreateCollectedText, actionOnGet: OnGetText, actionOnRelease: OnReleaseText, collectionCheck: true);
    }

    private void Start()
    {
        ShowStartScreen();
    }

    private void Update()
    {
        if (_activeTextList.Count == 0)
        {
            return;
        }

        for (int i = 0; i < _activeTextList.Count; i++)
        {
            var distance = Vector3.Distance(_player.position, _activeTextList[i].transform.position);
            
            if (distance >= _distanceToRelease)
            {
                _textPool.Release(_activeTextList[i]);
            }
        }   
    }

    public void ShowStartScreen()
    {
        _startScreen.SetActive(true);

        if (_loseScreen.activeInHierarchy)
        {
            _loseScreen.SetActive(false);
        }
    }

    public void ChangeToGameUI()
    {
        _startScreen.SetActive(false);
    }

    public void ShowLoseScreen()
    {
        _loseScreen.SetActive(true);
    }

    public void SetPlayerPosition(Transform player)
    {
        _player = player;
    }

    public void ShowCollectedText()
    {
        _textPool.Get();       
    }

    private GameObject CreateCollectedText()
    {
        return Instantiate(_collectedCubeTextPrefab, transform);
    }

    private void OnGetText(GameObject textObject)
    {        
        textObject.transform.position = new Vector3(_player.position.x, _player.position.y + _player.localScale.y, _player.position.z);
        textObject.SetActive(true);
        textObject.transform.DOMoveY(_endPos, _animTime);
        _activeTextList.Add(textObject);
    }

    private void OnReleaseText(GameObject textObject)
    {
        textObject.SetActive(false);
        _activeTextList.Remove(textObject);        
    }

    private void OnGameStarted()
    {
        GameStarted?.Invoke();
    }

    private void OnGameRestarted()
    {
        GameRestarted?.Invoke();
    }
}
