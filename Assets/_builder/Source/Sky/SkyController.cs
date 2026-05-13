using nobodyworks.builder.clock;
using UnityEngine;

namespace nobodyworks.builder.sky
{
    public class SkyController
    {
        private readonly SkySettings _settings;
        private readonly ClockController _clockController;
        
        private SkyScenerySettings _currentSkyScenerySettings;

        public SkyController(SkySettings settings, ClockController clockController)
        {
            _settings = settings;
            _clockController = clockController;
            
            _clockController.OnPhaseChanged += ClockPhaseChangedHandler;

            ChangeScenery(_settings.NightScenerySettings);
            ClockPhaseChangedHandler();
        }

        public void Dispose()
        {
            
        }

        private void ClockPhaseChangedHandler()
        {
            var phase = _clockController.Phase;

            if (phase == DayPhase.Day)
            {
                ChangeScenery(_settings.DayScenerySettings);
            }
            else if (phase == DayPhase.Night)
            {
                ChangeScenery(_settings.NightScenerySettings);
            }
        }

        private void ChangeScenery(SkyScenerySettings scenerySettings)
        {
            if (_currentSkyScenerySettings == scenerySettings)
            {
                return;
            }
            
            if (_currentSkyScenerySettings != null)
            {
                _currentSkyScenerySettings.CelestialGameObject.SetActive(false);
                _currentSkyScenerySettings.CelestialLight.enabled = false;
            }
            
            RenderSettings.skybox = scenerySettings.SkyboxMaterial;
            RenderSettings.fogColor = scenerySettings.FogColor;
            RenderSettings.fogDensity = scenerySettings.FogDensity;
            RenderSettings.ambientLight = scenerySettings.AmbientColor;
            RenderSettings.ambientIntensity = scenerySettings.AmbientIntensity;
            RenderSettings.reflectionIntensity = scenerySettings.ReflectionIntensity;
            
            var grassMaterial = _settings.GrassMaterial;
            grassMaterial.SetColor("_Subsurface_Scattering_Color", scenerySettings.GrassSubsurfaceColor);
            grassMaterial.SetFloat("Distance_Fade_Start", scenerySettings.GrassDistanceFadeStart);
            grassMaterial.SetFloat("Distance_Fade_End", scenerySettings.GrassDistanceFadeEnd);
            
            _currentSkyScenerySettings = scenerySettings;
            _currentSkyScenerySettings.CelestialGameObject.SetActive(true);
            _currentSkyScenerySettings.CelestialLight.enabled = true;
        }
    }
}
