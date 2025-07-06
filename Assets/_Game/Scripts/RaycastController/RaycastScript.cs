using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class RaycastScript : MonoBehaviour
{
    public static RaycastScript instance;
    public float rayDistance = 5f; // Ray'in ne kadar uzaða gideceði
    public Transform[] spawnpoints;
    CharacterController controller;
    public Canvas TimerCanvas;
    public LayerMask layerMask;

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
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayDistance, layerMask))
            {
                Debug.Log("Çarptýðýn nesne: " + hit.collider.name);

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
            else
            {
                Debug.Log("Hiçbir þeye çarpmadý.");
            }
        }

    }

    private IEnumerator TeleportTo(Vector3 targetPos)
    {
        controller.enabled = false;      // CharacterController'ý devre dýþý býrak
        transform.position = targetPos;  // Pozisyonu deðiþtir
        yield return null;               // Bir frame bekle
        controller.enabled = true;       // Tekrar aktif et
    }
}
