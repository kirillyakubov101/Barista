using UnityEngine;

namespace Barista.Environment
{
    public class LightHandler : MonoBehaviour
    {
        [SerializeField] private Material m_GlowLightMat;

        private const string c_EmissionVar = "_EmissionColor";
        private const string c_EmissionKeyword = "_EMISSION";

        private float m_turnOnIntensity = 1.0f;
        private Color m_emissionColor = Color.white;

        public void TurnOnLights()
        {
            m_GlowLightMat.SetColor(c_EmissionVar, m_emissionColor * m_turnOnIntensity);
            m_GlowLightMat.EnableKeyword(c_EmissionKeyword);
        }

        public void TurnOffLights()
        {
            m_GlowLightMat.SetColor(c_EmissionVar, m_emissionColor * 0f);
            m_GlowLightMat.EnableKeyword(c_EmissionKeyword);
        }
    }
}
