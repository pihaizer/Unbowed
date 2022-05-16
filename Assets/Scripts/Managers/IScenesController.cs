using System.Threading.Tasks;
using Unbowed.SO;

namespace Unbowed.Managers
{
    public interface IScenesController
    {
        public Task Load(SceneConfig sceneConfig);
    }
}