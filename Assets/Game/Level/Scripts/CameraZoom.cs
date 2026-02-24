using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    public float zoomSpeed = 10f;
    public float zoomSmoothness = 10f;

    public float minZoom = 2f;
    public float maxZoom = 40f;

    private float _currentZoom;

    [SerializeField] private Camera _camera;
    [SerializeField] private InputActionReference zoomAction;

    private void OnEnable()
    {
        zoomAction.action.Enable();
    }

    private void OnDisable()
    {
        zoomAction.action.Disable();
    }

    private void Start()
    {
        _currentZoom = _camera.orthographicSize;
    }

    private void Update()
    {
        float zoomInput = zoomAction.action.ReadValue<float>();

        //little code to avoid zoom triggering if stick drifts 

        if (Mathf.Abs(zoomInput) < 0.1f)
            return;

        _currentZoom -= zoomInput * zoomSpeed * Time.deltaTime;
        _currentZoom = Mathf.Clamp(_currentZoom, minZoom, maxZoom);

        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize,_currentZoom,zoomSmoothness * Time.deltaTime);
    }
}
