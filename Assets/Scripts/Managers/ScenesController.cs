using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Unbowed.Managers.Saves;
using Unbowed.Signals;
using Unbowed.SO;
using UnityEngine.SceneManagement;
using Zenject;

namespace Unbowed.Managers
{
    public class ScenesController : IScenesController, IInitializable
    {
        public List<SceneConfig> LoadedScenes { get; } = new();
        
        [Inject] private SignalBus _bus;
        [Inject] private ScenesConfig _scenesConfig;
        
        public void Initialize()
        {
            for (int i = 0; i < SceneManager.sceneCount; i++) {
                Scene scene = SceneManager.GetSceneAt(i);
                SceneConfig config = _scenesConfig.SceneConfigs.Find(sceneConfig => sceneConfig.sceneName == scene.name);
                if (config != null) LoadedScenes.Add(config);
            }
        }

        public async Task Load(SceneChangeRequest request)
        {
            
            if (LoadedScenes.Contains(request.SceneConfig)) return;

            if (request.UseLoadingScreen) {
                _bus.Fire(new LoadingScreenRequestSignal(true));
            }

            if (request.UnloadOther) {
                for (int i = LoadedScenes.Count - 1; i >= 0; i--) {
                    await Unload(LoadedScenes[i]);
                }
            }

            LoadedScenes.Add(request.SceneConfig);

            await SceneManager.LoadSceneAsync(request.SceneConfig.sceneName, LoadSceneMode.Additive);

            if (request.SetActive) {
                Scene scene = SceneManager.GetSceneByName(request.SceneConfig.sceneName);
                SceneManager.SetActiveScene(scene);
            }

            if (request.UseLoadingScreen) {
                _bus.Fire(new LoadingScreenRequestSignal(false));
            }
        }

        public async Task Unload(SceneConfig sceneConfig)
        {
            if (!LoadedScenes.Contains(sceneConfig)) return;

            LoadedScenes.Remove(sceneConfig);
            await SceneManager.UnloadSceneAsync(sceneConfig.sceneName);
        }
    }
}