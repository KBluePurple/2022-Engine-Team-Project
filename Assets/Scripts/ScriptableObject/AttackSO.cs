using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Attack")]
public class AttackSO : ScriptableObject
{
    public float speed;
    public float cooldown;
    public GameObject prefab;
}