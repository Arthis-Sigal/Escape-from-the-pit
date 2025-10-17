using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightSwing : MonoBehaviour
{
    [Header("Paramètres du mouvement")]
    public float amplitude = 6f; // distance maximale de déplacement
    public float speed = 1f;     // vitesse de l'aller-retour

    [Header("Optionnel : effet visuel")]
    public bool pulsateLight = true; // fait varier légèrement l’intensité
    public float intensityAmplitude = 0.2f;

    private float startX;
    private Light2D light2D;

    void Start()
    {
        startX = transform.position.x;
        light2D = GetComponent<Light2D>();
        Time.timeScale = 1f;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * speed) * amplitude;

        // mouvement sur l’axe X uniquement
        transform.position = new Vector3(startX + offset, transform.position.y, transform.position.z);

        // optionnel : effet de pulsation d’intensité
        if (pulsateLight && light2D != null)
        {
            light2D.intensity = 1f + Mathf.Sin(Time.time * speed * 2f) * intensityAmplitude;
        }
    }
}
