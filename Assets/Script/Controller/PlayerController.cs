using System.Collections;
using DG.Tweening;
using Script.Manager;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5;
    public float dashSpeed = 10;
    private Rigidbody _rigidbody;
    private TrailRenderer _trailRenderer;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _trailRenderer = GetComponent<TrailRenderer>();
    }
    
    void Update()
    {
        var mainCamera = GameManager.Instance.mainCamera;
        
        Move(mainCamera);
        Rotate(mainCamera);
    }

    private void Rotate(Camera mainCamera)
    {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hit, 100)) return;
        
        var position = transform.position;
        var targetPosition = hit.point;
        targetPosition.y = position.y;
        var lookRotation = Quaternion.LookRotation(targetPosition - position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10 * Time.deltaTime);
    }

    private void Move(Component mainCamera)
    {
        var rotationY = mainCamera.transform.rotation.eulerAngles.y;

        var input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        var inputRaw = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        
        var direction = Quaternion.Euler(0, rotationY, 0) * input;
        var directionRaw = Quaternion.Euler(0, rotationY, 0) * inputRaw;

        if (direction.magnitude <= speed)
            _rigidbody.velocity = direction * speed;


        if (Input.GetMouseButtonDown(1))
        {
            StartCoroutine(Dash(directionRaw));
        }
    }

    private IEnumerator Dash(Vector3 directionRaw)
    {
        _trailRenderer.emitting = true;
        yield return new WaitForSeconds(0.1f);
        GameManager.Instance.virtualCameraController.FOV = 100;
        GameManager.Instance.virtualCameraController.DOFieldOfView(65, 0.5f);
        if (Physics.Raycast(transform.position, directionRaw, out var hit, dashSpeed))
        {
            transform.position = hit.point;
        }
        else
        {
            transform.position += directionRaw * dashSpeed;
        }
        yield return new WaitForSeconds(0.1f);
        _trailRenderer.emitting = false;
    }
}
