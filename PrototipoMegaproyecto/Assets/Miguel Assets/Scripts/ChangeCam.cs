using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class ChangeCam : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    [SerializeField]
    private PlayerInput playerInput;
    [SerializeField]
    private int priorityBoostAmount = 10;
    [SerializeField]
    private Canvas aimCanvas;
    private InputAction aimAction;
    private void Awake() {
        aimAction = playerInput.actions["Aim"];
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        aimCanvas.enabled = false;
    }

    private void OnEnable() {
        aimAction.performed += _ => StartAim();
        aimAction.canceled += _ => CancelAim();
    }

    private void OnDisable() {
        aimAction.performed -= _ => StartAim();
        aimAction.canceled -= _ => CancelAim();
    }

    private void StartAim() {
        virtualCamera.Priority += priorityBoostAmount;
        aimCanvas.enabled = true;
    }

    private void CancelAim() {
        virtualCamera.Priority -= priorityBoostAmount;
        aimCanvas.enabled = false;
    }
    
}
