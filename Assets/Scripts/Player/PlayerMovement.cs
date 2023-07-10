using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float maxDisplacement = 0.2f;
    [SerializeField] private float maxPositionX = 2f;

    private Vector2 _anchorPosition;
    private float _forwardSpeed;
    private bool _shouldMove = false;

    private void Update()
    {
        if (_shouldMove)
        {
            transform.position += transform.forward * _forwardSpeed * Time.deltaTime;            

            var inputX = GetInput();

            var displacementX = GetDisplacement(inputX);

            displacementX = SmoothOutDisplacement(displacementX);

            var newPosition = GetNewLocalPosition(displacementX);

            newPosition = GetLimitedLocalPosition(newPosition);

            transform.localPosition = newPosition;
        }
    }

    public void SetForwardSpeed(float value, bool shouldMove)
    {
        _forwardSpeed = value;
        _shouldMove = shouldMove;
    }

    private Vector3 GetLimitedLocalPosition(Vector3 position)
    {
        position.x = Mathf.Clamp(position.x, -maxPositionX, maxPositionX);
        return position;
    }
    private Vector3 GetNewLocalPosition(float displacementX)
    {
        var lastPosition = transform.localPosition;
        var newPositionX = lastPosition.x + displacementX;
        var newPosition = new Vector3(newPositionX, lastPosition.y, lastPosition.z);
        return newPosition;
    }
    private float GetInput()
    {
        var inputX = 0f;

        if (Input.GetMouseButtonDown(0))
        {
            _anchorPosition = Input.mousePosition;
        }

        else if (Input.GetMouseButton(0))
        {
            inputX = (Input.mousePosition.x - _anchorPosition.x);
            _anchorPosition = Input.mousePosition;
        }
        return inputX;
    }
    private float GetDisplacement(float inputX)
    {
        float displacementX = 0f;
        displacementX = inputX * Time.deltaTime;
        return displacementX;
    }
    private float SmoothOutDisplacement(float displacementX)
    {
        return Mathf.Clamp(displacementX, -maxDisplacement, maxDisplacement);
    }
}
    