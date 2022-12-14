using System;
using System.Collections;
using UnityEngine;

public class FollowBullet : MonoBehaviour
{
    public float speed = 10f;
    public Transform target;
    private Rigidbody _rb;
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        StartCoroutine(Follow());
    }

    private IEnumerator Follow()
    {
        yield return new WaitForSeconds(1f);
        _rb.velocity = transform.forward * speed;
    }
}