using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour, InputSystem_Actions.IGameMenuActions
{
    private InputSystem_Actions inputActions;
    public static GameInput Instance { get; private set; }

    public event Action OnEscapeTriggered;

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
        inputActions.GameMenu.Enable();
        inputActions.GameMenu.SetCallbacks(this);
    }
    private void OnDisable()
    {
        inputActions?.GameMenu.Disable();
        inputActions?.GameMenu.RemoveCallbacks(this);
    }

    public void OnEscape(InputAction.CallbackContext context)
    {
        if (context.started)
            OnEscapeTriggered?.Invoke();
    }
}
