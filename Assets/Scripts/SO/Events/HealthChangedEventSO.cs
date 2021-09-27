using Gameplay;
using UnityEngine;

namespace SO.Events {
    [CreateAssetMenu(fileName = "Health Changed Event", menuName = "SO/Events/Health Changed Event", order = 0)]
    public class HealthChangedEventSO : ParameterEventSO<HealthChangeData> { }
}