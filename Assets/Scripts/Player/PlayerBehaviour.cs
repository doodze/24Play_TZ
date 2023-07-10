using System;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] private PlayerView _playerView;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private CubeHolder _cubeHolder;

    [Space][Header("Player Data")][Space]
    [SerializeField] private PlayerModel _playerModel;

    private float _playerSpeed;
    private Transform _playerPosition;    

    public event Action<Transform> PlayerPositionChanged;
    public event Action<float> PlayerSpeedChanged;
    public event Action CubeCollected;
    public event Action CubeLost;
    public event Action GameLost;    

    private void Awake()
    {
        _playerView.PlayerKilled += OnPlayerKilled;
        _cubeHolder.CubeCollected += OnCubeCollected;
        _cubeHolder.CubeLost += OnCubeLost;
        _cubeHolder.HolderEmpty += OnHolderEmpty;
    }

    private void Update()
    {
        Transform position = _playerView.transform;

        if (_playerPosition != position)
        {
            _playerPosition = position;
            PlayerPositionChanged?.Invoke(_playerPosition);
        }

        float speed = _playerModel.PlayerForwardSpeed;

        if (_playerSpeed != speed)
        {
            _playerSpeed = speed;
            PlayerSpeedChanged?.Invoke(_playerSpeed);
        }
    }

    public void StartMoving()
    {
        _playerMovement.SetForwardSpeed(_playerSpeed, true);
        _playerView.ShowWarp();
    }

    public void StopMoving()
    {
        _playerMovement.SetForwardSpeed(0, false);
        _playerView.HideWarp();
    }

    public void ResetPlayer()
    {
        _cubeHolder.ResetHolder();
        transform.position = Vector3.zero;
        _playerView.ResetView();
    }

    private void OnPlayerKilled()
    {
        _playerView.ShowRagdoll();
        GameLost?.Invoke();        
    }

    private void OnCubeCollected(float value)
    {
        _playerView.SetPosition(value);
        CubeCollected?.Invoke();
    }

    private void OnCubeLost()
    {
        CubeLost?.Invoke();        
    }

    private void OnHolderEmpty()
    {
        _playerView.ShowRagdoll();
        GameLost?.Invoke();
    }
}
        
  