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

    private void Update()
    {
        var mainCamera = GameManager.Instance.mainCamera;
        var position = transform.position;

        Move(mainCamera);
        Rotate(mainCamera, position);
    }

    private void Rotate(Camera mainCamera, Vector3 position)
    {
        var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hit, 100)) return;

        var targetPosition = hit.point;
        targetPosition.y = position.y;
        var lookRotation = Quaternion.LookRotation(targetPosition - position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10 * Time.deltaTime);
    }

    private void Move(Component mainCamera)
    {
        var rotationY = mainCamera.transform.rotation.eulerAngles.y;
        var inputRaw = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        var directionRaw = Quaternion.Euler(0, rotationY, 0) * inputRaw;

        if (directionRaw.magnitude <= speed)
            _rigidbody.velocity = directionRaw * speed;

        if (Input.GetMouseButtonDown(1) && directionRaw.magnitude != 0)
        {
            StartCoroutine(Dash(directionRaw));
        }
    }

    private IEnumerator Dash(Vector3 directionRaw)
    {
        _trailRenderer.emitting = true;

        Vector3 targetPosition;

        if (Physics.Raycast(transform.position, directionRaw, out var hit, dashSpeed))
            targetPosition = hit.point - directionRaw * 0.8f;
        else
            targetPosition = transform.position + directionRaw * dashSpeed;

        transform.DOMove(targetPosition, 0.1f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.1f);

        _trailRenderer.emitting = false;
    }
}