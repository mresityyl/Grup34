using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;

public class RaycastScript : MonoBehaviour
{
    public static RaycastScript instance;
    public float rayDistance = 5f; // Ray'in ne kadar uzaða gideceði
    public Canvas TimerCanvas;
    public LayerMask layerMask, interactableLayer;
    public Canvas DreamCanvas;

    public TMP_Text interactableText;


    // bulunacak nesneler
    bool Emzik, Ayýcýk, Çýngýrak;


    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (interactableText != null)
        {
            interactableText.enabled = false;
        }
        else { return; }

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        // Etkileþimli nesneler için raycast
        if (Physics.Raycast(ray, out hit, rayDistance, interactableLayer))
        {
            if (interactableText != null)
            {
                interactableText.enabled = true;
            }
            else { return; }

            // E tuþuna basýldýysa etkileþimi çalýþtýr
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("e");
                switch (hit.collider.tag)
                {
                    case "Yatak":
                        StartCoroutine(DreamLoad());                        
                        break;

                    default:
                        Debug.Log("Hiçbir iþlem yapýlmadý.");
                        break;
                }
            }
        }
    }

    IEnumerator DreamLoad()
    {
        DreamCanvas.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.49f);
        SceneManager.LoadScene("DreamNo1");
    }
}
