using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;

public class RaycastScript : MonoBehaviour
{
    public static RaycastScript instance;
    public float rayDistance = 5f; // Ray'in ne kadar uza�a gidece�i
    public Canvas TimerCanvas;
    public LayerMask layerMask, interactableLayer;
    public Canvas DreamCanvas;

    public TMP_Text interactableText;


    // bulunacak nesneler
    bool Emzik, Ayicik, Cingirak;


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
        // Etkile�imli nesneler i�in raycast
        if (Physics.Raycast(ray, out hit, rayDistance, interactableLayer))
        {
            if (interactableText != null)
            {
                interactableText.enabled = true;
            }
            else { return; }

            // E tu�una bas�ld�ysa etkile�imi �al��t�r
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("e");
                switch (hit.collider.tag)
                {
                    case "Yatak":
                        StartCoroutine(DreamLoad());                        
                        break;

                    default:
                        Debug.Log("Hi�bir i�lem yap�lmad�.");
                        break;
                }
            }
        }
    }

    IEnumerator DreamLoad()
    {
        DreamCanvas.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.49f);
        
        if (PlayerPrefs.GetInt("Dream1WakeUp") == 0)
        {
            SceneManager.LoadScene("DreamNo1");
        }
        else if (PlayerPrefs.GetInt("Dream2WakeUp") == 0)
        {
            SceneManager.LoadScene("Dream2_New");
        }
        else if (PlayerPrefs.GetInt("Dream3WakeUp") == 0)
        {
            SceneManager.LoadScene("DreamNo3");
        }
    }
}
