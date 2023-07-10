using UnityEngine;
using DG.Tweening;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] private Vector3 _shakePositionStrenght;   
    [SerializeField] private float _shakeTime;

    public void ShakeCamera()
    {
        transform.DOComplete();
        transform.DOShakePosition(_shakeTime, _shakePositionStrenght);
    }
}
