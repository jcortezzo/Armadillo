using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : Quantum
{
    [SerializeField] GameObject lightningGO;
    [SerializeField] private float ZAP_RATE;
    [SerializeField] private float LIGHTNING_LIFE_SPAN;
    [SerializeField] private int NUM_ZAPS;

    private float zapTimer;

    // Start is called before the first frame update
    void Start()
    {
        zapTimer = ZAP_RATE;
    }

    // Update is called once per frame
    void Update()
    {
        if (zapTimer <= 0)
        {
            Zap(this.transform.position, NUM_ZAPS);
            zapTimer = ZAP_RATE;
        }
        else
        {
            zapTimer -= Time.deltaTime;
        }
    }

    private void Zap(Vector3 pos, int n)
    {
        if (n == 0) return;

        GameObject lightning = Instantiate(lightningGO, pos, Quaternion.identity);
        BoxCollider2D bc = lightning.GetComponent<BoxCollider2D>();
        Vector3 newPos = bc.bounds.center - new Vector3(0, bc.bounds.extents.y, 0);  // bottom of lightning
        Zap(newPos, n - 1);
        Destroy(lightning, LIGHTNING_LIFE_SPAN);
    }
}
