using UnityEngine;
using UnityEngine.InputSystem;

public class LightController : MonoBehaviour
{
    [Header("Light Settings")]
    public Light roomLight;

    [Range(0f, 100f)]
    public float threshold = 30f;

    [Header("Brightness")]
    public float onIntensity = 5f;
    public float offIntensity = 0f;

    [Header("Bulb Visual")]
    public Renderer lightBulbRenderer;

    [ColorUsage(true, true)]
    public Color emissionColor = new Color(1f, 0.7f, 0.3f);

    public float emissionOnIntensity = 3f;
    public float emissionOffIntensity = 0f;

    private Material bulbMaterial;

    void Start()
    {
        // Create a material instance so we don't modify the original asset
        if (lightBulbRenderer != null)
        {
            bulbMaterial = lightBulbRenderer.material;
        }

        // Start with the light OFF
        SetVisualState(false);
    }

    public void SetLightLevel(float ldrValue)
    {
        Debug.Log("Received LDR Value: " + ldrValue + "%");

        if (ldrValue < threshold)
        {
            TurnLightOn();
        }
        else
        {
            TurnLightOff();
        }
    }

    private void TurnLightOn()
    {
        SetVisualState(true);

        Debug.Log("💡 Living Room Light: ON");
    }

    private void TurnLightOff()
    {
        SetVisualState(false);

        Debug.Log("💡 Living Room Light: OFF");
    }

    private void SetVisualState(bool isOn)
    {
        // Control actual Unity light
        if (roomLight != null)
        {
            roomLight.enabled = isOn;

            if (isOn)
            {
                roomLight.intensity = onIntensity;
            }
            else
            {
                roomLight.intensity = offIntensity;
            }
        }

        // Control bulb emission
        if (bulbMaterial != null)
        {
            if (isOn)
            {
                bulbMaterial.EnableKeyword("_EMISSION");

                bulbMaterial.SetColor(
                    "_EmissionColor",
                    emissionColor * emissionOnIntensity
                );
            }
            else
            {
                bulbMaterial.DisableKeyword("_EMISSION");

                bulbMaterial.SetColor(
                    "_EmissionColor",
                    emissionColor * emissionOffIntensity
                );
            }
        }
    }
    void Update()
    {
        if (Keyboard.current != null)
        {
            if (Keyboard.current.lKey.wasPressedThisFrame)
            {
                TurnLightOn();
            }

            if (Keyboard.current.oKey.wasPressedThisFrame)
            {
                TurnLightOff();
            }
        }
    }
}