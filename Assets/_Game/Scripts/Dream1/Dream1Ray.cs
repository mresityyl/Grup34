using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dream1Ray : MonoBehaviour
{
    private int rayDistance = 5;
    public LayerMask interactableLayer;
    bool Emzik, Ay�c�k, ��ng�rak;
    public TMP_Text CollectedText;
    public AudioSource PickUpSound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerPrefs.SetInt("Collected", 0);
        CollectedText.text = PlayerPrefs.GetInt("Collected").ToString() + "/3";
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayDistance, interactableLayer))
        {

            if (Input.GetKeyDown(KeyCode.E))
            {
                switch (hit.collider.tag)
                {
                    case "Emzik":
                        Emzik = true;
                        Debug.Log("Emzik Topland�");
                        PickUpSound.Play();
                        Animator EmzikAnimator = hit.collider.gameObject.GetComponent<Animator>();
                        PlayerPrefs.SetInt("Collected", PlayerPrefs.GetInt("Collected") + 1);
                        EmzikAnimator.Play("EmzikAnim");
                        B�l�mKontrol();
                        break;

                    case "Ay�c�k":
                        Ay�c�k = true;
                        Debug.Log("Ay�c�k Topland�");
                        PickUpSound.Play();
                        Animator Ay�c�kAnimator = hit.collider.gameObject.GetComponent<Animator>();
                        PlayerPrefs.SetInt("Collected", PlayerPrefs.GetInt("Collected") + 1);
                        Ay�c�kAnimator.Play("Ay�c�kAnim");
                        B�l�mKontrol();
                        break;

                    case "��ng�rak":
                        ��ng�rak = true;
                        Debug.Log("��ng�rak Topland�");
                        PickUpSound.Play();
                        Animator ��ng�rakAnimator = hit.collider.gameObject.GetComponent<Animator>();
                        PlayerPrefs.SetInt("Collected", PlayerPrefs.GetInt("Collected") + 1);
                        ��ng�rakAnimator.Play("��ng�rakAnim");
                        B�l�mKontrol();
                        break;

                    default:
                        Debug.Log("Hi�bir i�lem yap�lmad�.");
                        break;
                }
            }
        }
    }


    private void B�l�mKontrol()
    {
        CollectedText.text = PlayerPrefs.GetInt("Collected").ToString()+"/3";
        if (Emzik == true && Ay�c�k == true && ��ng�rak == true)
        {
            PlayerPrefs.SetInt("Dream1WakeUp", 1);
            SceneManager.LoadScene("HospitalRoomMap");
            Debug.Log("B�l�m Kazan�ld�");
        }
        else { return; }
    }


}
