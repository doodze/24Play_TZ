using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private CameraFollow _camera;
    [SerializeField] private PlayerBehaviour _playerBehaviour;
    [SerializeField] private TrackManager _trackManager;
    [SerializeField] private UIManager _uiManager;

    private void Awake()
    {
        Application.targetFrameRate = 60;       

        _uiManager.GameStarted += OnGameStarted;
        _playerBehaviour.GameLost += OnGameLost;
        _uiManager.GameRestarted += OnGameRestarted;
        _playerBehaviour.PlayerPositionChanged += OnPlayerPositionChanged;
        _playerBehaviour.PlayerSpeedChanged += OnPlayerSpeedChanged;
        _playerBehaviour.CubeCollected += OnCubeCollected;
        _playerBehaviour.CubeLost += OnCubeLost;                
    }

    private void OnGameStarted()
    {
        _uiManager.ChangeToGameUI();
        _trackManager.StartSpawning();
        _playerBehaviour.StartMoving();
    }

    private void OnGameLost()
    {
        _playerBehaviour.StopMoving();
        _trackManager.StopSpawning();
        _camera.ShakeCamera();
        _uiManager.ShowLoseScreen();        
    }

    private void OnGameRestarted()
    {
        _uiManager.ShowStartScreen();
        _trackManager.ResetTrack();
        _playerBehaviour.ResetPlayer();
    }

    private void OnPlayerPositionChanged(Transform player)
    {
        _camera.SetPlayerPosition(player);
        _uiManager.SetPlayerPosition(player);
    }

    private void OnPlayerSpeedChanged(float speed)
    {
        _trackManager.SetPlayerSpeed(speed);        
    }

    private void OnCubeCollected()
    {
        _uiManager.ShowCollectedText();
    }

    private void OnCubeLost()
    {
        _camera.ShakeCamera();
    }
}
