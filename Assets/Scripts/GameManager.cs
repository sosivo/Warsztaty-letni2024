using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [field: SerializeField] public GameObject PlayerPrefab { get; private set; }

    private Transform playerSpawnPoint;
    public Player Player { get; private set; }
    public CameraController CameraController { get; private set; }

    private void Awake()
    {
        Instance ??= this;
        if (Instance != this) Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        playerSpawnPoint = GameObject.FindWithTag("PlayerSpawnPoint").transform;
        Player = GameObject.FindWithTag("Player").GetComponent<Player>();

        CameraController = GameObject.FindWithTag("MainCamera").GetComponent<CameraController>();
    }

    private Player SpawnPlayer()
    {
        return Instantiate(PlayerPrefab, playerSpawnPoint.position, Quaternion.identity).GetComponent<Player>();
    }

    public void KillPlayer()
    {
        StartCoroutine(PerformKillPlayerSequence());
    }

    private IEnumerator PerformKillPlayerSequence()
    {
        // TODO: Create a nicer sequence
        Destroy(Player.gameObject);
        CameraController.TrackedObject = null;
        yield return new WaitForSeconds(0.6f);

        Player = SpawnPlayer();
        CameraController.TrackedObject = Player.transform;
    }
}