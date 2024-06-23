using UnityEngine;

namespace Barista.Environment
{
    public class LightHandler : MonoBehaviour
    {
        [SerializeField] private Light m_DirectionalLight;
        [SerializeField] private LightPreset m_Preset;
        [SerializeField,Range(0,24)] private float TimeOfDay;

        private void Update()
        {
            if(m_Preset)
            {
                TimeOfDay += Time.deltaTime * 0.1f;
                TimeOfDay %= 24; //clamp from 0 -24
                UpadteLighting(TimeOfDay);
            }
        }

        private void UpadteLighting(float timePercent)
        {
            RenderSettings.ambientLight = m_Preset.AmbientColor.Evaluate(TimeOfDay);
            RenderSettings.fogColor = m_Preset.FogColor.Evaluate(TimeOfDay);

            if(m_DirectionalLight)
            {
                m_DirectionalLight.color = m_Preset.DirectionalColor.Evaluate(TimeOfDay);
                m_DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f),-30f,0f));
            }
        }

    }
}
