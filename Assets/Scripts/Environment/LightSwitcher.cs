using System.Collections.Generic;
using UnityEngine;

namespace Barista
{
    public class LightSwitcher : MonoBehaviour
    {
        [SerializeField] private Texture2D[] DarkLightMapDir, DarkLightMapColor;
        [SerializeField] private Texture2D[] BrightLightMapDir, BrightLightMapColor;

        public LightmapData[] m_darkLightMap, m_brightLightMap;

        public bool test = false;

        private void Start()
        {
            //Dark map
            List<LightmapData> dlightmap = new List<LightmapData>();

            for (int i = 0; i < DarkLightMapDir.Length; i++)
            {
                LightmapData lmData = new LightmapData();
                lmData.lightmapDir = DarkLightMapDir[i];
                lmData.lightmapColor = DarkLightMapColor[i];

                dlightmap.Add(lmData);
            }

            m_darkLightMap = dlightmap.ToArray();


            //Light Map
            List<LightmapData> blightmap = new List<LightmapData>();

            for (int i = 0; i < BrightLightMapDir.Length; i++)
            {
                LightmapData lmData2 = new LightmapData();
                lmData2.lightmapDir = BrightLightMapDir[i];
                lmData2.lightmapColor = BrightLightMapColor[i];

                blightmap.Add(lmData2);
            }

            m_brightLightMap = blightmap.ToArray();
        }

        bool startWithoutLights = false;
        private void Update()
        {
            if(test)
            {
                SwitchLightMaps(startWithoutLights);
                startWithoutLights = !startWithoutLights;
                test = false;
            }
        }

        private void SwitchLightMaps(bool state)
        {
            if (state)
            {
                LightmapSettings.lightmaps = m_darkLightMap;
            }
            else
            {
                LightmapSettings.lightmaps = m_brightLightMap;
            }
          
            
        }
    }
}
