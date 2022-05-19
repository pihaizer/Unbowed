using UnityEngine;
using Zenject;

namespace Unbowed.UI
{
    public class PopupFactory : MonoBehaviour
    {
        [Inject] private DiContainer _container;
        
        public T Create<T>(T popupPrefab) where T : Component
        {
            return _container.InstantiatePrefab(popupPrefab, transform).GetComponent<T>();
        }
    }
}