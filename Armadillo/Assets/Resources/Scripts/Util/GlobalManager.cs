using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager instance;
    public Camera cam;
    public CameraController camController;
    private bool isStarted;
    public Player player;

    // Start is called before the first frame update
    void Awake()
    {
        SetUpSingleton();
    }

    void Start()
    {
        SetUpCamera();
        SetUpPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        SetUpPlayer();
    }

    public bool HasPlayer()
    {
        return player != null;
    }

    public bool IsGameStarted()
    {
        return isStarted;
    }

    public void StartGame()
    {
        isStarted = true;
    }

    public void EndGame()
    {
        isStarted = false;
    }

    private void SetUpSingleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void SetUpCamera()
    {
        cam = Camera.main;
        camController = cam.gameObject.GetComponent<CameraController>();  // sus
    }

    private void SetUpPlayer()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>();  // should only ever be one
        }
    }
}
