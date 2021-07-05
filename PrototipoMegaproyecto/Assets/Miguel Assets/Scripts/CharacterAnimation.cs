using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterAnimation : MonoBehaviour
{
    // Start is called before the first frame update
     // Start is called before the first frame update
    [SerializeField]
    private Rig aimLayer;
    [SerializeField]
    private float aimDuration = 0.3f;
    private bool aimLayerBool;
    Animator animator;
    public float speed = 2f;
    public float sprint = 4f;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    float velocityZ,velocityX = 0.0f;
    public float acceleration = 0.1f;
    public float deceleration = 0.5f;
    int velocityXHash, velocityZHash;
    void Start()
    {
        animator = GetComponent<Animator>();
        velocityXHash = Animator.StringToHash("VelocityX");
        velocityZHash = Animator.StringToHash("VelocityZ");
    }

    // Update is called once per frame
    void Update()
    {
        //changeVelocity(forwardPressed, backPressed, leftPressed, rightPressed, runPressed, speedLimit);        
        //lockOrResetVelocity(forwardPressed, backPressed, leftPressed, rightPressed, runPressed, speedLimit);

        animator.SetFloat(velocityXHash,velocityX);
        animator.SetFloat(velocityZHash,velocityZ);
        if(aimLayerBool){
            if(aimLayer.weight < 1.0f){
                aimLayer.weight += Time.deltaTime / aimDuration;
            }
        } else {
            if(aimLayer.weight > 0.0f){
                aimLayer.weight -= Time.deltaTime / aimDuration;
            }
        }
    }

    public void changeVelocity(bool forwardPressed, bool backPressed, bool leftPressed, bool rightPressed, bool runPressed, float currentMaxVelocity)
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
    public void lockOrResetVelocity(bool forwardPressed, bool backPressed, bool leftPressed, bool rightPressed, bool runPressed, float currentMaxVelocity)
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

    public void enableAimLayer() {
        aimLayerBool = true;
    }

    public void disableAimLayer() {
        aimLayerBool = false;
    }
}
