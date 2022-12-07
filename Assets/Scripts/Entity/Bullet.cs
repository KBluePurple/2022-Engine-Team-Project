using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Collider))]
public class Bullet : Entity
{
    [SerializeField] private float speed;
    
    public Bullet() : base(EntityType.Projectile)
    {
    }

    private void Start()
    {
        transform.Rotate(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(new Attack(1, this));
            Destroy(gameObject);
        }
    }
}