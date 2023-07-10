using System;
using UnityEngine;
using DG.Tweening;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _warpEffect;
    [SerializeField] private TrailRenderer _trailRenderer;
    [SerializeField] private float _jumpTime;
    [SerializeField] private float _jumpOffset;
    [SerializeField] private GameObject _ragdoll;
    [SerializeField] private GameObject[] _playerBodyParts;
    [SerializeField] private GameObject[] _ragDollBodyParts;
    [SerializeField] private float _ragDollPosOffset;

    private Vector3 _playViewStartPosition;   

    public event Action PlayerKilled;

    private void Start()
    {
        _playViewStartPosition = transform.position;        
    }
    public void SetPosition(float value)
    {
        _animator.SetTrigger("Jump");
        transform.DOComplete();
        transform.DOMoveY(transform.position.y + value + _jumpOffset, _jumpTime);
    }

    public void ResetView()
    {
        _trailRenderer.Clear();
        transform.position = _playViewStartPosition;
        gameObject.SetActive(true);
        _ragdoll.SetActive(false);
    }

    public void ShowRagdoll()
    {
        SetRagdollPosition();
        gameObject.SetActive(false);
        _ragdoll.SetActive(true);
    }

    public void ShowWarp()
    {
        _warpEffect.SetActive(true);          
    }

    public void HideWarp()
    {
        _warpEffect.SetActive(false); 
    }   

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(LayerNames.OBSTACLE_LAYER_NAME))
        {
            other.enabled = false;
            PlayerKilled?.Invoke();
        }
    }

    private void SetRagdollPosition()
    {
        if (_playerBodyParts.Length != 0 && _ragDollBodyParts.Length != 0)
        {
            for (int i = 0; i < _playerBodyParts.Length; i++)
            {
                _ragDollBodyParts[i].transform.position = _playerBodyParts[i].transform.position;
                _ragDollBodyParts[i].transform.rotation = _playerBodyParts[i].transform.rotation;
            }
        }

        _ragdoll.transform.position = new Vector3(_ragdoll.transform.position.x, _ragdoll.transform.position.y, _ragdoll.transform.position.z - _ragDollPosOffset);
    }
}
