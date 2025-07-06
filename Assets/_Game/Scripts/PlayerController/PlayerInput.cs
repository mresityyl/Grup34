using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    private InputSystem_Actions inputActions;
    public static PlayerInput Instance { get; private set; }

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool SprintToggledOn { get; private set; }

    [SerializeField] private bool holdToCrouch;
    public bool CrouchToggledOn { get; private set; }

    public event Action OnFlashlightToggle;
    public event Action OnFlashlightTapFeedback;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    private void OnEnable()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();
        inputActions.Player.SetCallbacks(this);
    }
    private void OnDisable()
    {
        inputActions?.Player.Disable();
        inputActions?.Player.RemoveCallbacks(this);
    }


    public void OnMove(InputAction.CallbackContext context) => MoveInput = context.ReadValue<Vector2>();

    public void OnLook(InputAction.CallbackContext context) => LookInput = context.ReadValue<Vector2>();

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.performed) SprintToggledOn = true;
        else if (context.canceled) SprintToggledOn = false;
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
            CrouchToggledOn = holdToCrouch || !CrouchToggledOn;
        else if (context.canceled)
            CrouchToggledOn = !holdToCrouch && CrouchToggledOn;
    }

    public void OnFlashLight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnFlashlightToggle?.Invoke();
        }
        else if (context.canceled)
        {
            OnFlashlightTapFeedback?.Invoke();
        }

    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        
    }
}
