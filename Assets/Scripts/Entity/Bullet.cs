using AchromaticDev.Util.Pooling;
using Script.Manager;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class Bullet : Entity
{
    [SerializeField] private float speed;
    [SerializeField] private float errorRange = 0.1f;

    public Bullet() : base(EntityType.Projectile)
    {
    }

    private void Start()
    {
        float GetRange() => Random.Range(-errorRange, errorRange);
        transform.Rotate(GetRange(), GetRange(), GetRange());
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));

        if (Vector3.Distance(GameManager.Instance.player.transform.position, transform.position) > 100)
        {
            PoolManager.Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Enemy")) return;

        other.gameObject.GetComponent<Enemy>().TakeDamage(new Attack(1, this));
        Destroy(gameObject);
    }
}