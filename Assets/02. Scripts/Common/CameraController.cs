using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private LayerMask _obstacleLayer;

    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _distance;

    [SerializeField] private Transform _target;  // 카메라가 따라갈 대상
    [SerializeField] private Vector3 _offset;  // 카메라와 대상 사이의 거리
    private Vector2 _lookVector;

    private float _azimuthAngle;
    private float _polarAngle;

    private void Awake()
    {
        _azimuthAngle = 0f;
        _polarAngle = 0f;
    }

    private void LateUpdate()
    {
        if (_target != null)
        {
            // 마우스 x, y 값을 이용해 카메라 이동
            _azimuthAngle += _lookVector.x * _rotationSpeed * Time.deltaTime;
            _polarAngle += _lookVector.y * _rotationSpeed * Time.deltaTime;
            _polarAngle = Mathf.Clamp(_polarAngle, -20f, 60f);

            // 벽이 있을 경우 Distance 조정
            var adjustCameraDistance = AdjustCameraDistance();

            // 카메라 위치 설정
            var cartesianPosition = GetCameraPosition(adjustCameraDistance, _polarAngle, _azimuthAngle);
            transform.position = _target.position - cartesianPosition;
            transform.LookAt(_target);
        }
    }

    private Vector3 GetCameraPosition(float r, float polarAngle, float azimuthAngle)
    {
        float b = r * Mathf.Cos(polarAngle * Mathf.Deg2Rad);
        float x = b * Mathf.Sin(azimuthAngle * Mathf.Deg2Rad);
        float y = r * Mathf.Sin(polarAngle * Mathf.Deg2Rad) * -1f;
        float z = b * Mathf.Cos(azimuthAngle * Mathf.Deg2Rad);

        return new Vector3(x, y, z);
    }

    public void SetTarget(Transform target, PlayerInput playerInput)
    {
        _target = target;

        var cartesianPosition = GetCameraPosition(_distance, _polarAngle, _azimuthAngle);
        transform.position = target.position - cartesianPosition;
        transform.LookAt(target);

        // 마우스 이동에 대한 처리
        playerInput.actions["Look"].performed += OnActionLook;
        playerInput.actions["Look"].canceled += OnActionLook;
    }

    private void OnActionLook(InputAction.CallbackContext context)
    {
        _lookVector = context.ReadValue<Vector2>();
    }

    private float AdjustCameraDistance()
    {
        var currentDistance = _distance;

        Vector3 direction = GetCameraPosition(1, _polarAngle, _azimuthAngle).normalized;

        RaycastHit hit;
        if (Physics.Raycast(_target.position, -direction, out hit, 
            _distance, _obstacleLayer))
        {
            float offset = 0.3f;
            currentDistance = hit.distance - offset;
            currentDistance = Mathf.Max(currentDistance, 0.5f);
        }

        return currentDistance;
    }
}
