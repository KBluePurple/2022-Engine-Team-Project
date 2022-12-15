using UnityEngine;
using Random = UnityEngine.Random;

public class MapGenerator : MonoBehaviour
{
    public Transform parent;
    public GameObject[] trees;

    public Vector2 mapSize;
    
    public float spacing = 10;

    [ContextMenu("Generate Map")]
    public void GenerateMap()
    {
        for (int i = 0; i < mapSize.x / spacing; i++)
        {
            for (int j = 0; j < mapSize.y / spacing; j++)
            {
                Vector3 pos = new Vector3(i * spacing, 0, j * spacing);
                var sphere = Random.insideUnitSphere * (spacing / 2);
                sphere.y = 0;
                pos += sphere;
                pos -= new Vector3(mapSize.x / 2, 0, mapSize.y / 2);
                var tree = Instantiate(trees[Random.Range(0, trees.Length)], pos, Quaternion.identity, parent);
                tree.transform.localScale = Vector3.one * Random.Range(0.5f, 1.5f);
            }
        }
    }
    
    [ContextMenu("Reset Map")]
    public void ResetMap()
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(mapSize.x, 0, mapSize.y));
    }
}