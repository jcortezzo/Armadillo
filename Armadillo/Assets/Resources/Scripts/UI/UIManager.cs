using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] public GameObject floatingTextGO;
    [SerializeField] private GameObject restart;

    private void Awake()
    {
        SetUpSingleton();
    }

    public void PopUpKarma(GameObject parent, int value, Color color)
    {
        GameObject go = Instantiate(floatingTextGO, 
                                    parent.transform.position + Vector3.up * 2, 
                                    Quaternion.identity);
        FloatingText ft = go.GetComponent<FloatingText>();
        TextMesh tmp = ft.gameObject.GetComponent<TextMesh>();
        tmp.text = "Karma " + (value >= 0 ? "+" : "") + value;
        tmp.color = color;
        //Debug.Log(tmp.text);
    }

    public void PopUpText(GameObject parent, string value, Color color)
    {
        GameObject go = Instantiate(floatingTextGO,
                                    parent.transform.position + Vector3.up * 2,
                                    Quaternion.identity);
        FloatingText ft = go.GetComponent<FloatingText>();
        TextMesh tmp = ft.gameObject.GetComponent<TextMesh>();
        tmp.text = "" + value;
        tmp.color = color;
    }

    // Start is called before the first frame update
    void Start()
    {
        //restart.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalManager.instance.player == null)
        {
            //restart.SetActive(true);
            restart.GetComponent<TextMeshProUGUI>().text =
                 "YOU DIED\nSPACE TO RESTART";
        }
        else
        {
            restart.GetComponent<TextMeshProUGUI>().text =
                 "";
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
}
