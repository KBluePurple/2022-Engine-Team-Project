using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Attack")]
public class AttackSo : ScriptableObject
{
    public float speed;
    public float cooldown;
    public GameObject prefab;
    public float chargeGage;
}