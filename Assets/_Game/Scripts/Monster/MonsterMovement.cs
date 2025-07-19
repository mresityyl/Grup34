using DG.Tweening;
using System.Collections;
using System.Threading;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MonsterMovement : MonoBehaviour
{
    public Transform player;
    public float detectionDistance = 5f;
    public float viewAngle = 60f;
    
    public NavMeshAgent agent;

    public AudioSource AudioSource;
    public RectTransform scareImage;
    public bool jumpScare;
    public float distance = 2;
    public LayerMask obstacleLayer;

    public bool isChasing;
    private bool isWaiting;

    public GameObject monsterJumpScare;
    public GameObject monster;
    [Header("Patrol")]
    [SerializeField] private Transform[] points;
    private int destPoint = 0;
    private float waitTime = 3f;
    public Animator controller;
    
    AsyncOperation sceneLoadingOperation;

    private void Start()
    {
        agent.autoBraking = false;

        sceneLoadingOperation = SceneManager.LoadSceneAsync("HospitalRoomMap");
        sceneLoadingOperation.allowSceneActivation = false; // Anýnda geçmesin!

        GotoNextPoint();
    }

    void GotoNextPoint()
    {
        if (points.Length == 0 || isWaiting)
            return;

        StartCoroutine(WaitatPoint());
    }

    IEnumerator WaitatPoint()
    {
        isWaiting = true;
        controller.SetBool("isWaiting", isWaiting);
        yield return new WaitForSeconds(waitTime);

        agent.destination = points[destPoint].position;
        destPoint = (destPoint + 1) % points.Length;
        isWaiting = false;
        controller.SetBool("isWaiting", isWaiting);
    }

    void Update()
    {
        if (agent.remainingDistance < 0.5f && !isChasing)
            GotoNextPoint();

        if (Input.GetMouseButton(1))
        {
            agent.SetDestination(player.transform.position);
        }

        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;
        Vector3 normalizedDirectionToPlayer = (distanceToPlayer > 0.001f) ? directionToPlayer.normalized : Vector3.zero;

        if (distanceToPlayer < detectionDistance)
        {
            float dotProduct = Vector3.Dot(transform.forward, normalizedDirectionToPlayer);
            float angleThreshold = Mathf.Cos(viewAngle * 0.5f * Mathf.Deg2Rad);

            if (dotProduct > angleThreshold)
            {
                Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
                Vector3 rayTarget = player.position + Vector3.up * 0.5f;

                Vector3 rayDirection = (rayTarget - rayOrigin).normalized;
                float rayDistance = Vector3.Distance(rayOrigin, rayTarget);

                if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, rayDistance, obstacleLayer))
                {
                    Debug.Log("Oyuncu mesafede ve açýda, AMA görüþ hattý engellendi: "/* + hit.collider.name*/);
                    isChasing = false;

                    // agent.ResetPath(); // Eðer oyuncuyu takip ediyorsa durdur
                }
                else
                {
                    Debug.Log("Oyuncu tespit edildi! Saldýrýlýyor!");
                    isChasing = true;
                    agent.SetDestination(player.transform.position);
                    agent.speed = 3;
                }

            }
            else
            {
                isChasing = false;
                agent.speed = 1;
            }
        }
        else
        {
            isChasing = false;
                    agent.speed = 1;

        }

    }
    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.green;
        //Gizmos.DrawLine(transform.position, player.position);

        if (player == null) return;

        // Yaratýðýn pozisyonundan baktýðý yön
        Vector3 forward = transform.forward * detectionDistance;

        // Görüþ konisinin sýnýr çizgileri
        Quaternion leftRayRotation = Quaternion.AngleAxis(-viewAngle * 0.5f, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(viewAngle * 0.5f, Vector3.up);

        Vector3 leftRayDirection = leftRayRotation * transform.forward * detectionDistance;
        Vector3 rightRayDirection = rightRayRotation * transform.forward * detectionDistance;

        // Renkler
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, forward);

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, leftRayDirection);
        Gizmos.DrawRay(transform.position, rightRayDirection);

        // Oyuncuya giden çizgi
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, player.position);

        // Mesafe küresi
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionDistance);
    }
    bool scareActive= false;
    private void OnTriggerEnter(Collider other)
    {
        if (!jumpScare) return;
        //if (scareImage == null) return;

        AudioSource.Play();
        if(!scareActive)
            StartCoroutine(JumpScareTimer());
        
        //scareImage.DOScale(Vector3.one * 2f, 0.5f).OnComplete(() =>
        //{
        //    // 2 saniye bekle
        //    DOVirtual.DelayedCall(3.5f, () =>
        //    {
        //        scareImage.DOScale(Vector3.zero, 0.4f);
        //    });
        //}); 

    }

    IEnumerator JumpScareTimer()
    {
        scareActive = true;
        monster.SetActive(false);
        monsterJumpScare.SetActive(true);
        player.gameObject.GetComponent<PlayerController>().canMove = false;
        yield return new WaitForSeconds(3);
        //monsterJumpScare.SetActive(false);
        //monster.SetActive(true);
        //scareActive = false;

        //SceneManager.LoadScene("HospitalRoomMap");
        sceneLoadingOperation.allowSceneActivation = true;

    }
}
