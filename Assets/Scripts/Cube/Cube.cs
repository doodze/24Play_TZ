using System;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private Rigidbody _cubeRigidbody;
    [SerializeField] private ParticleSystem _stackEffect;

    private float _cubeStartMass;

    public event Action CubeCollected;
    public event Action<Cube> CubeGotCollision;
    public float CubeMass => _cubeRigidbody.mass;

    private void Start()
    {
        _cubeStartMass = _cubeRigidbody.mass;
    }

    public void SetMassToDefault()
    {
        _cubeRigidbody.mass = _cubeStartMass;
    }

    public void SetMass(float value)
    {
        _cubeRigidbody.mass = value;
    }

    public void UpdateConstraints()
    {
        _cubeRigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(LayerNames.PICKUP_LAYER_NAME))
        {
            other.gameObject.SetActive(false);
            CubeCollected?.Invoke();
        }

        if (other.gameObject.layer == LayerMask.NameToLayer(LayerNames.OBSTACLE_LAYER_NAME))
        {
            _cubeRigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX;
            other.enabled = false;
            CubeGotCollision?.Invoke(this);
        }
    }
}
