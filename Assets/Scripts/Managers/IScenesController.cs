using System.Collections.Generic;
using System.Threading.Tasks;
using Unbowed.SO;

namespace Unbowed.Managers
{
    public interface IScenesController
    {
        public List<SceneConfig> LoadedScenes { get; }
        public Task Load(SceneChangeRequest request);
        public Task Unload(SceneConfig sceneConfig);
    }
}