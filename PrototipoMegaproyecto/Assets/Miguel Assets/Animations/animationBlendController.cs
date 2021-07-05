using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationBlendController : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
    float velocity = 0.0f;
    public float acceleration = 0.1f;
    public float deceleration = 0.5f;
    int velocityHash;
    void Start()
    {
        animator = GetComponent<Animator>();
        velocityHash = Animator.StringToHash("Velocity");
    }

    // Update is called once per frame
    void Update()
    {
        bool forwardPressed = Input.GetKey("w");
        bool runPressed = Input.GetKey("left shift");

        if (forwardPressed && velocity < 1.0f)
        {
            velocity += Time.deltaTime * acceleration;
        }

        if (!forwardPressed && velocity > 0.0f)
        {
            velocity -= Time.deltaTime * deceleration;
        }

        if (velocity < 0.0f)
        {
            velocity = 0.0f;
        }
        animator.SetFloat(velocityHash,velocity);
    }
}
