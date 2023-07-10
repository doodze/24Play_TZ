using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private CameraShaker _cameraShaker;
    [SerializeField] private float _zOffset;
    [SerializeField] private float _smoothTime;

    private Vector3 _velocity = Vector3.zero;
    private Transform _player;

    private void Update()
    {
        if (_player == null)
        {
            return;
        }

        transform.position = Vector3.SmoothDamp(transform.position,
            new Vector3(transform.position.x, transform.position.y, _player.position.z - _zOffset), ref _velocity, _smoothTime);
    }

    public void SetPlayerPosition(Transform player)
    {
        _player = player;
    }

    public void ShakeCamera()
    {
        _cameraShaker.ShakeCamera();
    }
}
