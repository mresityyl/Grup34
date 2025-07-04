using System;
using UnityEngine;

[RequireComponent (typeof(PlayerInput), typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerMovement playerMovement;

    [SerializeField] private Camera playerCamera;
    private CharacterController characterController;


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }
    private void Start()
    {
        playerInput = PlayerInput.Instance;
        playerMovement = GetComponent<PlayerMovement>();

        playerMovement.InitializeHeadbob();
        playerMovement.InitializeCrouchDefaults(characterController, playerCamera.transform);
    }

    private void Update()
    {
        playerMovement.PlayerMove(characterController, playerInput.MoveInput, playerInput.SprintToggledOn);

        playerMovement.PlayerCrouch(characterController, playerCamera.transform, playerInput.CrouchToggledOn);

    }

    private void LateUpdate()
    {
        playerMovement.PlayerSetCamera(playerCamera.transform, playerInput.LookInput);
    }
}
