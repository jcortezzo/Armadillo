using System.Collections;
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
    [SerializeField] private int lives;
    private float score;
    private float karma;
    [SerializeField] private bool gameOver;

    bool restarted = false;


    [SerializeField] private Biomes biome;

    public static Color[] DEFAULT_PALETTE = 
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

    [Header("Palette")]
    [SerializeField] private bool enableDefault;
    [SerializeField] private bool enableHell;
    [SerializeField] private bool enableHeaven;

    // Start is called before the first frame update
    void Awake()
    {
        SetUpSingleton();
    }

    void Start()
    {
        SetUpCamera();
        SetUpPlayer();
        StartNewGame();
        Jukebox.Instance.PlayMusic("DefaultOverworld", 0.025f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        SetUpPlayer();
        SetBiomePalette();

        if (restarted)
        {
            restarted = false;
            SetUpCamera();
            SetUpPlayer();
            StartNewGame();
        }

        if (player == null && Input.GetKeyDown(KeyCode.Space))
        {
            SceneManagerSingleton.instance.RestartScene();
            score = 0;
            karma = 0;
            lives = 3;
            isStarted = false;
            restarted = true;
            biome = Biomes.DEFAULT;
        }
        //PaletteChange();
    }

    public bool IsGameRunning()
    {
        return gameOver;
    }

    public void StartNewGame()
    {
        gameOver = false;
    }

    public void EndCurrentGame()
    {
        gameOver = true;
    }

    public void AddLives(int n)
    {
        lives += n;
    }

    public int GetLives()
    {
        return lives;
    }

    public void SetBiome(Biomes b)
    {
        this.biome = b;
    }

    public Biomes GetBiome()
    {
        return biome;
    }

    public void AddScore(float n)
    {
        score += n;
    }

    public int GetScore()
    {
        return Mathf.FloorToInt(score);
    }

    public void AddKarma(float n)
    {
        karma += n;
    }

    public int GetKarma()
    {
        return Mathf.FloorToInt(karma);
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

    private void SetBiomePalette()
    {
        if (biome == Biomes.DEFAULT)
        {
            palette.SetColors(DEFAULT_PALETTE);
        }
        else if (biome == Biomes.HELL)
        {
            palette.SetColors(HELL_PALETTE);
        }
        else if (biome == Biomes.HEAVEN)
        {
            palette.SetColors(HEAVEN_PALETTE);
        }
    }

    private void PaletteChange()
    {
        if (enableDefault)
        {
            palette.SetColors(DEFAULT_PALETTE);
        }
        else if (enableHell)
        {
            palette.SetColors(HELL_PALETTE);
        }
        else if (enableHeaven)
        {
            palette.SetColors(HEAVEN_PALETTE);
        }
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
