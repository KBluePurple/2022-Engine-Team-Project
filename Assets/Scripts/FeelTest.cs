using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;

public class FeelTest : MonoBehaviour
{
    
    public MMFeedbacks jumpFeedBack;
    public MMFeedbacks landFeedBack;

    private Rigidbody rb = null;

    private bool isJumping = false;
    private float lastVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Physics.gravity = Vector3.down * 30;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isJumping == false)
        {
            rb.AddForce(Vector3.up * 7.5f, ForceMode.Impulse);
            jumpFeedBack?.PlayFeedbacks();
            isJumping = true;
        }

        if (isJumping && (lastVelocity < 0) && (Mathf.Abs(rb.velocity.y) < 0.1))
        {
            isJumping = false;
            landFeedBack?.PlayFeedbacks();
        }

        lastVelocity = rb.velocity.y;
    }
    
}
