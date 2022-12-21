using UnityEngine;
using KBluePurple.Util;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float speed = 4f;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private float nowTime;
    [SerializeField] private GameObject target;
    
    private Vector3 targetPos;

    private void OnEnable()
    {
        target = GameObject.Find("Player");
        targetPos = target.transform.position;
        transform.rotation = Quaternion.LookRotation(Vector3.up);
    }

    private void Update()
    {
        nowTime += Time.deltaTime;
        var transformCache = transform;

        if (nowTime >= lifeTime)
        {
            var rotation = Quaternion.LookRotation(Vector3.down);
            transformCache.rotation = Quaternion.Slerp(transformCache.rotation, rotation, Time.deltaTime * 2f);
        }
        else
        {
            var lookPos = targetPos - transformCache.position;
            var rotation = Quaternion.LookRotation(lookPos);
            transformCache.rotation = Quaternion.Slerp(transformCache.rotation, rotation, Time.deltaTime * 2f);
        }

        transformCache.position += transformCache.forward * (speed * Time.deltaTime);
        
        if (transform.position.y < -10)
        {
            DestoryAction();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward);
    }

    public void DestoryAction()
    {
        nowTime = 0;
        PoolManager.Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        var hitAble = other.GetComponent<IHitAble>();
        if (hitAble == null) return;
        
        if (hitAble.Hit(damage))
            DestoryAction();
    }
}