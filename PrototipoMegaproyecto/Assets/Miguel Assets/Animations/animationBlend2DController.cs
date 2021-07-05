using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationBlend2DController : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
    float velocityZ,velocityX = 0.0f;
    public float acceleration = 0.1f;
    public float deceleration = 0.5f;
    int velocityXHash, velocityZHash;
    float speedLimit = 0.5f;
    void Start()
    {
        animator = GetComponent<Animator>();
        velocityXHash = Animator.StringToHash("VelocityX");
        velocityZHash = Animator.StringToHash("VelocityZ");
    }

    void changeVelocity(bool forwardPressed, bool leftPressed, bool rightPressed, bool runPressed, float speedLimit)
    {
        if (forwardPressed && velocityZ < speedLimit)
        {
            velocityZ += Time.deltaTime * acceleration;
        }

        if (!forwardPressed && velocityZ > 0.0f)
        {
            velocityZ -= Time.deltaTime * deceleration;
        }

        if (rightPressed && velocityX < speedLimit)
        {
            velocityX += Time.deltaTime * acceleration;
        }

        if (!rightPressed && velocityX > 0.0f)
        {
            velocityX -= Time.deltaTime * deceleration;
        }

        if (leftPressed && velocityX > -speedLimit)
        {
            velocityX -= Time.deltaTime * acceleration;
        }

        if (!leftPressed && velocityX < 0.0f)
        {
            velocityX += Time.deltaTime * deceleration;
        }
    }

    void lockVelocity(bool forwardPressed, bool leftPressed, bool rightPressed, bool runPressed, float speedLimit)
    {
        if (velocityZ < 0.0f)
        {
            velocityZ = 0.0f;
        }
/*
        if (!leftPressed && !rightPressed && velocityX != 0.0f && velocityX > -speedLimit && velocityX < speedLimit)
        {
            velocityX = 0.0f;
        }
*/
        if (velocityX > speedLimit)
        {
            velocityX -= Time.deltaTime * deceleration;
            if (leftPressed && velocityX < speedLimit && velocityX > (speedLimit - 0.05f))
            {
                velocityX = speedLimit;
            }
        }

        if (velocityZ > speedLimit)
        {
            velocityZ -= Time.deltaTime * deceleration;
            if (forwardPressed && velocityZ < speedLimit && velocityZ > (speedLimit - 0.05f))
            {
                velocityZ = speedLimit;
            }
        }

        if (velocityX < -speedLimit)
        {
            velocityX += Time.deltaTime * deceleration;
            if (leftPressed && velocityX > -speedLimit && velocityX < (-speedLimit + 0.05f))
            {
                velocityX = -speedLimit;
            }
        }

        if (velocityZ < -speedLimit)
        {
            velocityZ += Time.deltaTime * deceleration;
            if (leftPressed && velocityZ > -speedLimit && velocityZ < (-speedLimit + 0.05f))
            {
                velocityZ = -speedLimit;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        bool forwardPressed = Input.GetKey(KeyCode.W);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);
        bool rightPressed = Input.GetKey(KeyCode.D);
        bool leftPressed = Input.GetKey(KeyCode.A);
        speedLimit = runPressed ? 2.0f : 0.5f;

        changeVelocity(forwardPressed, leftPressed, rightPressed, runPressed, speedLimit);        
        lockVelocity(forwardPressed, leftPressed, rightPressed, runPressed, speedLimit);

        animator.SetFloat(velocityXHash,velocityX);
        animator.SetFloat(velocityZHash,velocityZ);
    }
}
