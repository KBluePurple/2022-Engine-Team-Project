using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player player;
    
    private float _time;
    private int _level;
    
    private void Update()
    {
        _time += Time.deltaTime;
        if (_time >= 60 * _level)
            player.Skills[_level++].Unlock();
    }
}