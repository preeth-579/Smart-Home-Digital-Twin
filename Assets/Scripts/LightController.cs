using UnityEngine;
using UnityEngine.InputSystem;

public class LightController : MonoBehaviour
{
    [Header("Smart Home Lights")]
    public Light[] roomLights;

    [Header("Directional / Sun Light")]
    public Light directionalLight;

    [Range(0f, 100f)]
    public float threshold = 30f;

    [Header("Brightness")]
    public float onIntensity = 5f;
    public float offIntensity = 0f;

    [Header("Bulb Visuals")]
    public Renderer[] lightBulbRenderers;

    [ColorUsage(true, true)]
    public Color emissionColor = new Color(1f, 0.7f, 0.3f);

    public float emissionOnIntensity = 3f;
    public float emissionOffIntensity = 0f;

    private Material[] bulbMaterials;

    private void Start()
    {
        // Create material instances for all bulbs
        if (lightBulbRenderers != null)
        {
            bulbMaterials = new Material[lightBulbRenderers.Length];

            for (int i = 0; i < lightBulbRenderers.Length; i++)
            {
                if (lightBulbRenderers[i] != null)
                {
                    bulbMaterials[i] = lightBulbRenderers[i].material;
                }
            }
        }

        // Start with smart lights OFF
        SetVisualState(false);
    }

    public void SetLightLevel(float ldrValue)
    {
        Debug.Log("Received LDR Value: " + ldrValue + "%");

        if (ldrValue < threshold)
        {
            TurnLightsOn();
        }
        else
        {
            TurnLightsOff();
        }
    }

    private void TurnLightsOn()
    {
        SetVisualState(true);

        Debug.Log("💡 All Smart Lights: ON");
        Debug.Log("☀️ Directional Light: OFF");
    }

    private void TurnLightsOff()
    {
        SetVisualState(false);

        Debug.Log("💡 All Smart Lights: OFF");
        Debug.Log("☀️ Directional Light: ON");
    }

    private void SetVisualState(bool isOn)
    {
        // --------------------------------
        // SMART HOME LIGHTS
        // --------------------------------

        if (roomLights != null)
        {
            foreach (Light roomLight in roomLights)
            {
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
            }
        }

        // --------------------------------
        // BULB EMISSION
        // --------------------------------

        if (bulbMaterials != null)
        {
            foreach (Material bulbMaterial in bulbMaterials)
            {
                if (bulbMaterial == null)
                    continue;

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

        // --------------------------------
        // DIRECTIONAL LIGHT
        // --------------------------------

        if (directionalLight != null)
        {
            // Smart lights ON → Directional light OFF
            // Smart lights OFF → Directional light ON

            directionalLight.enabled = !isOn;
        }
    }

    private void Update()
    {
        if (Keyboard.current != null)
        {
            // L = Smart Lights ON
            if (Keyboard.current.lKey.wasPressedThisFrame)
            {
                TurnLightsOn();
            }

            // O = Smart Lights OFF
            if (Keyboard.current.oKey.wasPressedThisFrame)
            {
                TurnLightsOff();
            }
        }
    }
}