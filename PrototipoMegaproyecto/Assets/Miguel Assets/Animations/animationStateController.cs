using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationStateController : MonoBehaviour
{
    Animator animator;
    int isWalkingHash, isRunningHash;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
    }

    // Update is called once per frame
    void Update()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool forwardPressed = Input.GetKey("w");
        bool isRunning = animator.GetBool(isRunningHash);
        bool runPressed = Input.GetKey("left shift");
        if (!isWalking && forwardPressed)
        {
            animator.SetBool(isWalkingHash,true);
        }
        if (isWalking && !forwardPressed){
            animator.SetBool(isWalkingHash,false);
        }

        if (!isRunning && (runPressed && forwardPressed))
        {
            animator.SetBool(isRunningHash,true);
        }
        if (isRunning && (!forwardPressed || !runPressed)){
            animator.SetBool(isRunningHash,false);
        }
    }
}
