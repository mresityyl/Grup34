using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dream1Ray : MonoBehaviour
{
    private int rayDistance = 5;
    public LayerMask interactableLayer;
    bool Pacifier, Bear, Hammer;
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
                    case "Pacifier":
                        Pacifier = true;
                        PickUpSound.Play();
                        Animator PacifierAnimator = hit.collider.gameObject.GetComponent<Animator>();
                        PlayerPrefs.SetInt("Collected", PlayerPrefs.GetInt("Collected") + 1);
                        PacifierAnimator.Play("PacifierAnim");
                        BölümKontrol();
                        break;

                    case "Bear":
                        Bear = true;
                        PickUpSound.Play();
                        Animator BearAnimator = hit.collider.gameObject.GetComponent<Animator>();
                        PlayerPrefs.SetInt("Collected", PlayerPrefs.GetInt("Collected") + 1);
                        BearAnimator.Play("BearAnim");
                        BölümKontrol();
                        break;

                    case "Hammer":
                        Hammer = true;
                        PickUpSound.Play();
                        Animator HammerAnimator = hit.collider.gameObject.GetComponent<Animator>();
                        PlayerPrefs.SetInt("Collected", PlayerPrefs.GetInt("Collected") + 1);
                        HammerAnimator.Play("HammerAnim");
                        BölümKontrol();
                        break;

                    default:
                        Debug.Log("Hicbir islem yapilmadi.");
                        break;
                }
            }
        }
    }


    private void BölümKontrol()
    {
        CollectedText.text = PlayerPrefs.GetInt("Collected").ToString()+"/3";
        if (Pacifier == true && Bear == true && Hammer == true)
        {
            PlayerPrefs.SetInt("Dream1WakeUp", 1);
            SceneManager.LoadScene("HospitalRoomMap");
            Debug.Log("Bölüm Kazanildi.");
        }
        else { return; }
    }


}
