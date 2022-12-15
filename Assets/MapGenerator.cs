using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    public Transform parent;
    public GameObject[] trees;
    
    public Vector2 mapSize;

    [ContextMenu("Generate")]
    public void GenerateMap()
    {
        for (int i = 0; i < mapSize.x / 20; i++)
        {
            for (int j = 0; j < mapSize.y / 20; j++)
            {
                Vector3 pos = new Vector3(i * 20, 0, j * 20);
                var sphere = Random.insideUnitSphere * 10;
                sphere.y = 0;
                pos += sphere;
                pos -= new Vector3(mapSize.x / 2, 0, mapSize.y / 2);
                var tree = Instantiate(trees[Random.Range(0, trees.Length)], pos, Quaternion.identity, parent);
                tree.transform.localScale = Vector3.one * Random.Range(0.5f, 1.5f);
            }
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(mapSize.x, 0, mapSize.y));
    }
}