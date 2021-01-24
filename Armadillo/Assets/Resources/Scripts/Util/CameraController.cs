using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float shakeDuration;
    [SerializeField] private float shakeMagnitude;
    [SerializeField] private float shakeDampening;

    private Vector3 shakePos;
    private bool isShaking;

    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        isShaking = false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateShake();
    }

    void LateUpdate()
    {
        // don't allow the camera to move backwards
        UpdateCameraToPlayer();
    }

    /// <summary>
    /// Moves the camera forward with the player
    /// but never backwards.
    /// </summary>
    private void UpdateCameraToPlayer()
    {
        if (!GlobalManager.instance.HasPlayer()) return;

        transform.position =
                new Vector3(Mathf.Max(transform.position.x,
                                      GlobalManager.instance.player.transform.position.x),
                            transform.position.y,
                            transform.position.z);
    }

    private void UpdateShake()
    {
        if (shakeDuration > 0)
        {
            transform.localPosition = shakePos + Random.insideUnitSphere * shakeMagnitude;
            shakeDuration -= Time.deltaTime * shakeDampening;
        }
        else if (isShaking)
        { 
            transform.localPosition = shakePos;
            isShaking = false;
        }
    }

    /// <summary>
    /// Initiates a camera shake if the camera is not currently shaking.
    /// </summary>
    /// <param name="shakeTime">Duration for the camera to shake.</param>
    /// <param name="shakeMagnitude">Strength of the camera shake.</param>
    /// <param name="shakeDampening">Smoothness of the camera shake.</param>
    public void Shake(float shakeTime=0.25f, 
                      float shakeMagnitude=0.7f,
                      float shakeDampening=1f)
    {
        if (isShaking) return;

        // update fields
        this.shakeDuration = shakeTime;
        this.shakeMagnitude = shakeMagnitude;
        this.shakeDampening = shakeDampening;

        shakePos = transform.localPosition;
        isShaking = true;
    }
}
