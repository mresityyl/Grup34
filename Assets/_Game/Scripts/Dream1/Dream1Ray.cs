using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dream1Ray : MonoBehaviour
{
    private int rayDistance = 5;
    public LayerMask interactableLayer;
    bool Emzik, Ayýcýk, Çýngýrak;
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
                        Debug.Log("Emzik Toplandý");
                        PickUpSound.Play();
                        Animator EmzikAnimator = hit.collider.gameObject.GetComponent<Animator>();
                        PlayerPrefs.SetInt("Collected", PlayerPrefs.GetInt("Collected") + 1);
                        EmzikAnimator.Play("EmzikAnim");
                        BölümKontrol();
                        break;

                    case "Ayýcýk":
                        Ayýcýk = true;
                        Debug.Log("Ayýcýk Toplandý");
                        PickUpSound.Play();
                        Animator AyýcýkAnimator = hit.collider.gameObject.GetComponent<Animator>();
                        PlayerPrefs.SetInt("Collected", PlayerPrefs.GetInt("Collected") + 1);
                        AyýcýkAnimator.Play("AyýcýkAnim");
                        BölümKontrol();
                        break;

                    case "Çýngýrak":
                        Çýngýrak = true;
                        Debug.Log("Çýngýrak Toplandý");
                        PickUpSound.Play();
                        Animator ÇýngýrakAnimator = hit.collider.gameObject.GetComponent<Animator>();
                        PlayerPrefs.SetInt("Collected", PlayerPrefs.GetInt("Collected") + 1);
                        ÇýngýrakAnimator.Play("ÇýngýrakAnim");
                        BölümKontrol();
                        break;

                    default:
                        Debug.Log("Hiçbir iþlem yapýlmadý.");
                        break;
                }
            }
        }
    }


    private void BölümKontrol()
    {
        CollectedText.text = PlayerPrefs.GetInt("Collected").ToString()+"/3";
        if (Emzik == true && Ayýcýk == true && Çýngýrak == true)
        {
            PlayerPrefs.SetInt("Dream1WakeUp", 1);
            SceneManager.LoadScene("HospitalRoomMap");
            Debug.Log("Bölüm Kazanýldý");
        }
        else { return; }
    }


}
