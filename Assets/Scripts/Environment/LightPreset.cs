using UnityEngine;

namespace Barista.Environment
{
    [CreateAssetMenu(fileName = "LightPreset", menuName = "CreateLightPreset/LightPreset", order = 1)]
    public class LightPreset : ScriptableObject
    {
        public Gradient AmbientColor;
        public Gradient DirectionalColor;
        public Gradient FogColor;
    }
}
