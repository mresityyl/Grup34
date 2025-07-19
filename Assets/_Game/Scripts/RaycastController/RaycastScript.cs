using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class RaycastScript : MonoBehaviour
{
    public static RaycastScript instance;
    public float rayDistance = 5f; // Ray'in ne kadar uza�a gidece�i
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

        // Etkile�imli nesneler i�in raycast
        if (Physics.Raycast(ray, out hit, rayDistance, interactableLayer))
        {
            interactableText.enabled = true;

            // E tu�una bas�ld�ysa etkile�imi �al��t�r
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
                            Debug.Log("R�yaya Dal�yorsunuz.");
                        }
                        break;

                    default:
                        Debug.Log("Hi�bir i�lem yap�lmad�.");
                        break;
                }
            }
        }
    }

    private IEnumerator TeleportTo(Vector3 targetPos) // art�k sahne de�i�ece�i i�in buras� iptal edilecek.
    {
        controller.enabled = false;      // CharacterController'� devre d��� b�rak
        transform.position = targetPos;  // Pozisyonu de�i�tir
        yield return null;               // Bir frame bekle
        controller.enabled = true;       // Tekrar aktif et
    }
}
