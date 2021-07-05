using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class thirdPersonCharacterController : MonoBehaviour
{
    // Start is called before the first frame update
     Animator animator;
    public CharacterController controller;
    public Transform cam;
    public float speed = 2f;
    public float sprint = 4f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    float velocityZ,velocityX = 0.0f;
    public float acceleration = 0.1f;
    public float deceleration = 0.5f;
    int velocityXHash, velocityZHash;
    float speedLimit = 0.5f;
    float currentSpeed = 0.0f;
    void Start()
    {
        animator = GetComponent<Animator>();
        velocityXHash = Animator.StringToHash("VelocityX");
        velocityZHash = Animator.StringToHash("VelocityZ");
    }

    // Update is called once per frame
    void Update()
    {
        bool forwardPressed = Input.GetKey(KeyCode.W);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);
        bool rightPressed = Input.GetKey(KeyCode.D);
        bool leftPressed = Input.GetKey(KeyCode.A);
        bool backPressed = Input.GetKey(KeyCode.S);
        speedLimit = runPressed ? 2.0f : 0.5f;

        changeVelocity(forwardPressed, backPressed, leftPressed, rightPressed, runPressed, speedLimit);        
        lockOrResetVelocity(forwardPressed, backPressed, leftPressed, rightPressed, runPressed, speedLimit);

        animator.SetFloat(velocityXHash,velocityX);
        animator.SetFloat(velocityZHash,velocityZ);

        Vector3 direction = new Vector3(velocityX, 0f, velocityZ).normalized;
        currentSpeed = new Vector3(velocityX, 0f, velocityZ).magnitude * speed;
        //currentSpeed = Mathf.Max(new float[] {velocityX,velocityZ})*speed;
        //Debug.Log(currentSpeed);
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, cam.eulerAngles.y, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f)*Vector3.forward;
            controller.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
        }
    }

    void changeVelocity(bool forwardPressed, bool backPressed, bool leftPressed, bool rightPressed, bool runPressed, float currentMaxVelocity)
    {
        //forward movement
        if (forwardPressed && velocityZ < currentMaxVelocity)
        {
            velocityZ += Time.deltaTime * acceleration;
        }
        //backward movement
        if (backPressed && velocityZ > -currentMaxVelocity)
        {
            velocityZ -= Time.deltaTime * acceleration;
        }
        //left movement
        if (leftPressed && velocityX > -currentMaxVelocity)
        {
            velocityX -= Time.deltaTime * acceleration;
        }
        //right movement
        if (rightPressed && velocityX < currentMaxVelocity)
        {
            velocityX += Time.deltaTime * acceleration;
        }
        //decrease velocityZ if forward not pressed
        if (!forwardPressed && velocityZ > 0.0f)
        {
            velocityZ -= Time.deltaTime * deceleration;
        }
        //increase velocityZ if back not pressed
        if (!backPressed && velocityZ < 0.0f)
        {
            velocityZ += Time.deltaTime * deceleration;
        }
        //increase velocityX if left is not pressed
        if (!leftPressed && velocityX < 0.0f)
        {
            velocityX += Time.deltaTime * deceleration;
        }
        //decrease velocityX if right is not pressed
        if (!rightPressed && velocityX > 0.0f)
        {
            velocityX -= Time.deltaTime * deceleration;
        }
    }
    void lockOrResetVelocity(bool forwardPressed, bool backPressed, bool leftPressed, bool rightPressed, bool runPressed, float currentMaxVelocity)
    {
        //reset velocityZ
        if(!forwardPressed && !backPressed && velocityZ != 0.0f && (velocityZ > -0.05f && velocityZ < 0.05f))
        {
            velocityZ = 0.0f;
        }
        //reset velocityX
        if(!leftPressed && !rightPressed && velocityX != 0.0f && (velocityX > -0.05f && velocityX < 0.05f))
        {
            velocityX = 0.0f;
        }
        //lock forward
        if (forwardPressed && runPressed && velocityZ > currentMaxVelocity)
        {
            velocityZ = currentMaxVelocity;
        }
        //decelerate to the maximum walk velocity
        else if (forwardPressed && velocityZ > currentMaxVelocity)
        {
            velocityZ -= Time.deltaTime * deceleration;
            //round to the currentMaxVelocity if within offset
            if (velocityZ > currentMaxVelocity && velocityZ < (currentMaxVelocity + 0.05))
            {
                velocityZ = currentMaxVelocity;
            }
        }
        //round to the currentMaxVelocity if within offset
        else if (forwardPressed && velocityZ < currentMaxVelocity && velocityZ > (currentMaxVelocity - 0.05f))
        {
            velocityZ = currentMaxVelocity;
        }

        //lock backward
        if (backPressed && runPressed && velocityZ < -currentMaxVelocity)
        {
            velocityZ = -currentMaxVelocity;
        }
        //decelerate to the maximum walk back velocity
        else if (backPressed && velocityZ < -currentMaxVelocity)
        {
            velocityZ += Time.deltaTime * deceleration;
            //round to the currentMaxVelocity if within offset
            if (velocityZ < -currentMaxVelocity && velocityZ > (-currentMaxVelocity - 0.05))
            {
                velocityZ = -currentMaxVelocity;
            }
        }
        //round to the currentMaxVelocity if within offset
        else if (backPressed && velocityZ > -currentMaxVelocity && velocityZ < (-currentMaxVelocity + 0.05f))
        {
            velocityZ = -currentMaxVelocity;
        }

        //locking left
        if (leftPressed && runPressed && velocityX < -currentMaxVelocity)
        {
            velocityX = -currentMaxVelocity;
        }
        //decelerate to the maximum walk velocity
        else if (leftPressed && velocityX < -currentMaxVelocity)
        {
            velocityX += Time.deltaTime * deceleration;
            //round to the currentMaxVelocity if within offset
            if (velocityX < -currentMaxVelocity && velocityX > (-currentMaxVelocity - 0.05f)) {
                velocityX = -currentMaxVelocity;
            }
        }
        //round to the currentMaxVelocity if within offset
        else if (leftPressed && velocityX > -currentMaxVelocity && velocityX < (-currentMaxVelocity+0.05f))
        {
            velocityX = -currentMaxVelocity;
        }
        //locking right
        if (rightPressed && runPressed && velocityX > currentMaxVelocity)
        {
            velocityX = currentMaxVelocity;
        }
        //decelerate to the maximum walk velocity
        else if (rightPressed && velocityX > currentMaxVelocity)
        {
            velocityX -= Time.deltaTime * deceleration;
            //round to the currentMaxVelocity if within offset
            if (velocityX > currentMaxVelocity && velocityX < (currentMaxVelocity + 0.05f)) {
                velocityX = currentMaxVelocity;
            }
        }
        //round to the currentMaxVelocity if within offset
        else if (rightPressed && velocityX < currentMaxVelocity && velocityX > (currentMaxVelocity - 0.05f))
        {
            velocityX = currentMaxVelocity;
        }
    }
}
