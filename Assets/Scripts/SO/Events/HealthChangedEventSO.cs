using Unbowed.Gameplay;
using UnityEngine;

namespace Unbowed.SO.Events {
    [CreateAssetMenu(fileName = "Health Changed Event", menuName = "SO/Events/Health Changed Event", order = 0)]
    public class HealthChangedEventSO : ParameterEventSO<HealthChangeData> { }
}