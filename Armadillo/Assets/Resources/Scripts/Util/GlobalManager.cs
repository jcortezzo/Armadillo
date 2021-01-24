﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager instance;
    public Camera cam;
    public CameraController camController;
    public PaletteSwap palette;
    private bool isStarted;
    public Player player;

    public static Color[] NORMAL_PALETTE = 
            { Color.black,
              Color.white,
              Color.red };

    public static Color[] HELL_PALETTE = 
            { Color.black,
              Color.red,
              new Color(0.654902f, 0, 0.03137255f) };  // 0xA70008

    public static Color[] HEAVEN_PALETTE =
            { new Color(0.03921569f, 0.1098039f, 0.2156863f),  // 0x0A1C37
              new Color(0.9843138f, 0.9254903f, 0.6705883f),  // 0xFBECAB
              new Color(0.4588236f, 0.7921569f, 0.9333334f) };  // 0x75CAEE

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
        palette = cam.gameObject.GetComponent<PaletteSwap>();
    }

    private void SetUpPlayer()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>();  // should only ever be one
        }
    }
}
