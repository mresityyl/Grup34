using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float sprintSpeed = 7f;
    [SerializeField] private float crouchSpeed = 1f;
    private Vector3 moveDirection;
    private Vector2 smoothInput;
    private Vector2 smoothVelocity;

    [Header("Camera")]
    [SerializeField] private float sensitivity = 0.2f;
    [SerializeField] private float maxLookAngle = 70f;
    private float lookAtVertical;
    private float lookAtHorizontal;
    private float yawVelocity = 0f;
    private float pitchVelocity = 0f;
    [SerializeField] private float walkFOV = 60f;
    [SerializeField] private float runFOV = 75f;
    [SerializeField] private float fovDuration = 0.3f;
    private Tweener fovTween;

    [Header("Headbob")]
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private float headbobSpeed = 10f;
    [SerializeField] private float headbobAmount = 0.05f;
    private float headbobTimer = 0f;
    private Vector3 cameraInitialLocalPos;

    [Header("Physics")]
    [SerializeField] private float gravity = -9.81f;
    private float verticalVelocity;
     
    [Header("Crouch")]
    [SerializeField, Range(0f, 1f)] private float crouchRatio = 0.5f;
    [SerializeField] private float crouchSmoothTime = 0.1f;
    private bool wasCrouching = false;
    private float originalHeight;
    private Vector3 originalCenter;
    private Vector3 originalCameraLocalPos;
    private float currentHeight;
    private float heightVelocity;
    private Vector3 currentCenter;
    private Vector3 centerVelocity;
    private Vector3 currentCameraPos;
    private Vector3 cameraVelocity;
    public LayerMask crouchLayerMask;

    #endregion

    public void InitializeHeadbob()
    {
        cameraInitialLocalPos = cameraHolder.localPosition;
    }

    public void InitializeCrouchDefaults(CharacterController characterController, Transform camera)
    {
        originalHeight = characterController.height;
        originalCenter = characterController.center;
        originalCameraLocalPos = camera.localPosition;

        currentHeight = originalHeight;
        currentCenter = originalCenter;
        currentCameraPos = originalCameraLocalPos;
    }

    public void PlayerMove(CharacterController characterController, Camera playerCamera, Vector2 input, bool sprintToggledOn)
    {
        smoothInput = Vector2.SmoothDamp(smoothInput, input, ref smoothVelocity, 0.1f);

        Vector3 horizontalMove = Vector3.zero;
        horizontalMove += transform.forward * smoothInput.y;
        horizontalMove += transform.right * smoothInput.x;

        bool isSprint = sprintToggledOn && !wasCrouching;
        //bool isCrouch = !sprintToggledOn && wasCrouching;
        float speed = isSprint ? sprintSpeed : wasCrouching ? crouchSpeed : walkSpeed;
        horizontalMove *= speed;

        //Gravity
        if (characterController.isGrounded && verticalVelocity < 0)
            verticalVelocity = -1f;
        else
            verticalVelocity += gravity * Time.deltaTime;

        moveDirection = horizontalMove;
        moveDirection.y = verticalVelocity;

        characterController.Move(moveDirection * Time.deltaTime);

        //HeadBob
        bool isMoving = smoothInput.magnitude > 0.1f && characterController.isGrounded;

        if (isMoving)
        {
            headbobTimer += Time.deltaTime * headbobSpeed;

            float currentBobAmount = wasCrouching ? headbobAmount * 0.5f : sprintToggledOn ? headbobAmount * 2f : headbobAmount;
            float bobX = Mathf.Sin(headbobTimer) * currentBobAmount;
            float bobY = Mathf.Cos(headbobTimer * 2) * currentBobAmount;

            Vector3 bobPosition = cameraInitialLocalPos + new Vector3(bobX, bobY, 0);
            cameraHolder.localPosition = bobPosition;
        }
        else
        {
            headbobTimer = 0f;
            cameraHolder.localPosition = Vector3.Lerp(cameraHolder.localPosition, cameraInitialLocalPos, Time.deltaTime * headbobSpeed);
        }

        //FOV
        if (!wasCrouching)
        {
            float targetFOV = sprintToggledOn && isMoving ? runFOV : walkFOV;

            if (Mathf.Abs(playerCamera.fieldOfView - targetFOV) > 0.1f)
            {
                if (fovTween != null && fovTween.IsActive())
                    fovTween.Kill();

                fovTween = playerCamera.DOFieldOfView(targetFOV, fovDuration);
            }
        }

    }

    public void PlayerSetCamera(Transform camera, Vector2 input)
    {
        // Yatay dönüþ (karakter gövdesi dönüyor)
        lookAtHorizontal += input.x * sensitivity;
        float smoothYaw = Mathf.SmoothDampAngle(transform.eulerAngles.y, lookAtHorizontal, ref yawVelocity, 0.05f);
        transform.rotation = Quaternion.Euler(0f, smoothYaw, 0f);

        // Dikey dönüþ (kamera dönüyor)
        lookAtVertical -= input.y * sensitivity;
        lookAtVertical = Mathf.Clamp(lookAtVertical, -maxLookAngle, maxLookAngle);

        float currentPitch = camera.localEulerAngles.x;
        currentPitch = (currentPitch > 180f) ? currentPitch - 360f : currentPitch;

        float smoothPitch = Mathf.SmoothDampAngle(currentPitch, lookAtVertical, ref pitchVelocity, 0.05f);
        camera.localRotation = Quaternion.Euler(smoothPitch, 0f, 0f);
    }

    public void PlayerCrouch(CharacterController characterController, Transform camera, bool crouchToggledOn)
    {
        bool blockedAbove = false;
        //Kafa üstünde engel var mý
        if (!crouchToggledOn)
        {
            float castDistance = originalHeight - currentHeight + 0.05f;

            Vector3 start = transform.position + Vector3.up * (currentHeight * 0.5f);
            float radius = characterController.radius * 0.9f;

            if (Physics.SphereCast(start, radius, Vector3.up, out RaycastHit hit, castDistance, crouchLayerMask, QueryTriggerInteraction.Ignore))
                blockedAbove = true;
        }

        bool shouldCrouch = crouchToggledOn || blockedAbove;

        if (shouldCrouch && !wasCrouching)
            wasCrouching = true;
        else if (!shouldCrouch && wasCrouching)
            wasCrouching = false;

        float targetHeight = shouldCrouch ? originalHeight * crouchRatio : originalHeight;

        Vector3 targetCenter = shouldCrouch
            ? new Vector3(originalCenter.x, originalCenter.y * crouchRatio, originalCenter.z)
            : originalCenter;

        Vector3 targetCameraPos = shouldCrouch
            ? new Vector3(originalCameraLocalPos.x, originalCameraLocalPos.y * crouchRatio, originalCameraLocalPos.z)
            : originalCameraLocalPos;

        currentHeight = Mathf.SmoothDamp(currentHeight, targetHeight, ref heightVelocity, crouchSmoothTime);
        characterController.height = currentHeight;

        currentCenter = Vector3.SmoothDamp(currentCenter, targetCenter, ref centerVelocity, crouchSmoothTime);
        characterController.center = currentCenter;

        currentCameraPos = Vector3.SmoothDamp(currentCameraPos, targetCameraPos, ref cameraVelocity, crouchSmoothTime);
        camera.localPosition = currentCameraPos;
    }
}
