using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public PlayerController controller;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        controller = GetComponent<PlayerController>();
    }
}
