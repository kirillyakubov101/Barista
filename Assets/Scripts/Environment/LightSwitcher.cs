using System.Collections.Generic;
using UnityEngine;

namespace Barista
{
    public class LightSwitcher : MonoBehaviour
    {
        //[SerializeField] private Texture2D[] DarkLightMapDir, DarkLightMapColor;
        [SerializeField] private Texture2D[] BrightLightMapDir, BrightLightMapColor;

        [SerializeField] private Material NightSkybox;

        public LightmapData[] m_brightLightMap;


        private void Start()
        {
            ////Dark map
            //List<LightmapData> dlightmap = new List<LightmapData>();

            //for (int i = 0; i < DarkLightMapDir.Length; i++)
            //{
            //    LightmapData lmData = new LightmapData();
            //    lmData.lightmapDir = DarkLightMapDir[i];
            //    lmData.lightmapColor = DarkLightMapColor[i];

            //    dlightmap.Add(lmData);
            //}

            //m_darkLightMap = dlightmap.ToArray();


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


            //LightmapSettings.lightmaps = m_darkLightMap;
        }


        public void ToggleNightLights()
        {
            LightmapSettings.lightmaps = m_brightLightMap;
            RenderSettings.skybox = NightSkybox;
        }
    }
}
