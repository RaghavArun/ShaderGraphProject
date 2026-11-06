using UnityEngine;

public class Lightsaber : MonoBehaviour
{
    public float extendSpeed = 0.1f;
    public bool weaponActive;
    public float SCALE_MIN = 0f;
    public float scaleMax;
    public float extendDelta;
    public float scaleCurrent;
    public float localScaleX;
    public float localScaleZ;
    public GameObject saber;

    public AudioSource saberAudio;

    public AudioClip saberOnSound;
    public AudioClip saberOffSound;

    public Renderer saberRenderer;
    public Color[] saberColors;

    private static readonly int SaberColorID =
        Shader.PropertyToID("_SaberColor");

    void Start()
    {
        localScaleX = transform.localScale.x;
        localScaleZ = transform.localScale.z;

        scaleMax = transform.localScale.y;

        scaleCurrent = scaleMax;

        extendDelta = scaleMax / extendSpeed;

        weaponActive = true;
    }

    void Update()
    {
        // Check for spacebar input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            extendDelta = weaponActive
                ? -Mathf.Abs(extendDelta)
                : Mathf.Abs(extendDelta);

            if (weaponActive)
            {
                saberAudio.PlayOneShot(saberOffSound);
            }
            else
            {
                saberAudio.PlayOneShot(saberOnSound);
                ChangeSaberColor();

            }
        }

        scaleCurrent += extendDelta * Time.deltaTime;

        scaleCurrent = Mathf.Clamp(scaleCurrent, SCALE_MIN, scaleMax);

        transform.localScale = new Vector3(
            localScaleX,
            scaleCurrent,
            localScaleZ
        );

        weaponActive = scaleCurrent > 0;

        if (weaponActive && !saber.activeSelf)  
        {
            saber.SetActive(true);
        }
        else if (!weaponActive && saber.activeSelf)
        {
            saber.SetActive(false);
        }
    }

    void ChangeSaberColor()
    {
        if (saberColors.Length == 0) return;

        Color newColor = saberColors[Random.Range(0, saberColors.Length)];

        saberRenderer.material.SetColor(
            Shader.PropertyToID("_SaberColor"),
            newColor
        );
    }
}