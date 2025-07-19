using System;
using UnityEngine;

[RequireComponent (typeof(PlayerInput), typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerMovement playerMovement;
    private PlayerAudioController playerAudioController;

    [SerializeField] private Camera playerCamera;
    private CharacterController characterController;

    public bool canMove = true;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerMovement = GetComponent<PlayerMovement>();
        playerAudioController = GetComponentInChildren<PlayerAudioController>();
    }
    private void Start()
    {
        playerInput = PlayerInput.Instance;

        playerMovement.InitializeHeadbob();
        playerMovement.InitializeCrouchDefaults(characterController, playerCamera.transform);
    }

    private void Update()
    {
        if (!canMove) return;

        playerMovement.PlayerMove(characterController, playerCamera, playerInput.MoveInput, playerInput.SprintToggledOn);

        playerAudioController.SetValues(playerInput.MoveInput, playerInput.SprintToggledOn, playerInput.CrouchToggledOn);

        playerMovement.PlayerCrouch(characterController, playerCamera.transform, playerInput.CrouchToggledOn);
    }

    private void LateUpdate()
    {
        playerMovement.PlayerSetCamera(playerCamera.transform, playerInput.LookInput);
    }
}
