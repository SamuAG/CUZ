using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "InputManagerSO")]
public class InputManagerSO : ScriptableObject
{
    Inputs inputs;

    public event Action<Vector2> OnMovementStarted;
    public event Action OnMovementCanceled;
    public event Action OnJumpStarted;
    public event Action OnCrouchStarted;
    public event Action OnCrouchCanceled;
    public event Action<Vector2> OnCameraPerformed;
    public event Action OnCameraCanceled;
    public event Action OnSprintStarted;
    public event Action OnSprintCanceled;

    private void OnEnable()
    {
        inputs = new Inputs();
        inputs.Enable(); // nacen inhabilitado se pueden habilitar tambien por mapa de teclas Gameplay en este caso
        inputs.Gameplay.Movement.performed += MovementStarted;
        inputs.Gameplay.Movement.canceled += MovementCanceled;
        inputs.Gameplay.Jump.started += JumpStarted;
        inputs.Gameplay.Crouch.started += CrouchStarted;
        inputs.Gameplay.Crouch.canceled += CrouchCanceled;
        inputs.Gameplay.Camera.performed += CameraPerformed;
        inputs.Gameplay.Camera.canceled += CameraCanceled;
        inputs.Gameplay.Sprint.started += SprintStarted;
        inputs.Gameplay.Sprint.canceled += SprintCanceled;
    }

    private void MovementStarted(InputAction.CallbackContext context)
    {
        OnMovementStarted?.Invoke(context.ReadValue<Vector2>());
    }

    private void MovementCanceled(InputAction.CallbackContext context)
    {
        OnMovementCanceled?.Invoke();
    }

    private void JumpStarted(InputAction.CallbackContext context)
    {
        OnJumpStarted?.Invoke();
    }
    private void CrouchStarted(InputAction.CallbackContext context)
    {
        OnCrouchStarted?.Invoke();
    }

    private void CrouchCanceled(InputAction.CallbackContext context)
    {
        OnCrouchCanceled?.Invoke();
    }

    private void CameraPerformed(InputAction.CallbackContext context)
    {
        Debug.Log(context);
        OnCameraPerformed?.Invoke(context.ReadValue<Vector2>());
    }

    private void CameraCanceled(InputAction.CallbackContext context)
    {
        OnCameraCanceled?.Invoke();
    }

    private void SprintStarted(InputAction.CallbackContext context)
    {
        OnSprintStarted?.Invoke();
    }

    private void SprintCanceled(InputAction.CallbackContext context)
    {
        OnSprintCanceled?.Invoke();
    }
}
