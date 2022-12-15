using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    void Update()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");
        var input = new Vector3(h, 0, v);
        transform.Translate(input * (Time.deltaTime * 5));
    }
}