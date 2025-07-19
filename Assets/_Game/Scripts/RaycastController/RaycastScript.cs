using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class RaycastScript : MonoBehaviour
{
    public static RaycastScript instance;
    public float rayDistance = 5f; // Ray'in ne kadar uzaða gideceði
    public Transform[] spawnpoints;
    CharacterController controller;
    public Canvas TimerCanvas;
    public LayerMask layerMask, interactableLayer;

    public TMP_Text interactableText;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        interactableText.enabled = false;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // Etkileþimli nesneler için raycast
        if (Physics.Raycast(ray, out hit, rayDistance, interactableLayer))
        {
            interactableText.enabled = true;

            // E tuþuna basýldýysa etkileþimi çalýþtýr
            if (Input.GetKeyDown(KeyCode.E))
            {
                switch (hit.collider.tag)
                {
                    case "Yatak":
                        if (spawnpoints.Length > 0)
                        {
                            StartCoroutine(TeleportTo(spawnpoints[0].position));
                        }
                        else
                        {
                            Debug.Log("Rüyaya Dalýyorsunuz.");
                        }
                        break;

                    default:
                        Debug.Log("Hiçbir iþlem yapýlmadý.");
                        break;
                }
            }
        }
    }

    private IEnumerator TeleportTo(Vector3 targetPos) // artýk sahne deðiþeceði için burasý iptal edilecek.
    {
        controller.enabled = false;      // CharacterController'ý devre dýþý býrak
        transform.position = targetPos;  // Pozisyonu deðiþtir
        yield return null;               // Bir frame bekle
        controller.enabled = true;       // Tekrar aktif et
    }
}
