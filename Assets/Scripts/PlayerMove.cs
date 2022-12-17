using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float speed = 4f;
    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");
        var input = new Vector3(h, 0, v);

        var direction = _mainCamera.transform.TransformDirection(input);
        direction.y = 0;
        transform.position += direction * (speed * Time.deltaTime);
    }
}