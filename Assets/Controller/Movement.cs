using UnityEngine;
using UnityEngine.InputSystem;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private Rigidbody capsule;
    public Vector2 moveVal;
    public float moveSpeed = 10;

    private void Awake()
    {
        capsule = GetComponent<Rigidbody>();
    }

    public void Moving(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            Debug.Log("Performed.");
            moveVal = value.ReadValue<Vector2>();
            Debug.Log(moveVal.x + " " + moveVal.y);
        }
    }
}
