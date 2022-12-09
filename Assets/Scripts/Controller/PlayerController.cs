using System.Collections;
using DG.Tweening;
using Script.Manager;
using UnityEngine;

[RequireComponent(typeof(PlayerAttackController))]
[RequireComponent(typeof(TrailRenderer))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    #region SerializeField

    public float speed = 5;
    public float dashSpeed = 10;
    public GameObject dashEffect;
    [SerializeField] private LayerMask groundLayer;

    #endregion SerializeField

    private Rigidbody _rigidbody;
    private TrailRenderer _trailRenderer;
    private PlayerAttackController _playerAttackController;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _trailRenderer = GetComponent<TrailRenderer>();
        _playerAttackController = GetComponent<PlayerAttackController>();
    }

    private void Update()
    {
        Camera mainCamera = GameManager.Instance.mainCamera;
        Vector3 position = transform.position;

        Move();
        Rotate();

        if (Input.GetMouseButtonDown(0))
        {
            _playerAttackController.Attack();
        }

        if (Input.GetMouseButtonDown(1))
        {
            _playerAttackController.HeavyAttack();
        }

        void Rotate()
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit, 100, groundLayer)) return;

            Vector3 targetPosition = hit.point;
            targetPosition.y = position.y;
            Quaternion lookRotation = Quaternion.LookRotation(targetPosition - position);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 10 * Time.deltaTime);
            Debug.DrawLine(mainCamera.transform.position, hit.point, Color.red);
        }

        void Move()
        {
            float rotationY = mainCamera.transform.rotation.eulerAngles.y;
            Vector3 inputRaw = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
            Vector3 directionRaw = Quaternion.Euler(0, rotationY, 0) * inputRaw;

            if (directionRaw.magnitude <= speed)
                _rigidbody.velocity = directionRaw * speed;

            if (Input.GetMouseButtonDown(1) && directionRaw.magnitude != 0)
            {
                StartCoroutine(Dash(directionRaw));
            }
        }
    }

    private IEnumerator Dash(Vector3 directionRaw)
    {
        _trailRenderer.emitting = true;

        Vector3 targetPosition;

        if (Physics.Raycast(transform.position, directionRaw, out RaycastHit hit, dashSpeed))
            targetPosition = hit.point - directionRaw * 0.8f;
        else
            targetPosition = transform.position + directionRaw * dashSpeed;

        transform.DOMove(targetPosition, 0.1f).SetEase(Ease.Linear);
        yield return new WaitForSeconds(0.1f);

        _trailRenderer.emitting = false;
    }
}