using System.Collections;
using Unbowed.Gameplay.Characters.Modules;
using Unbowed.Utility.WaitFor;
using UnityEngine;

namespace Unbowed.Gameplay.OnDeath {
    [RequireComponent(typeof(Health))]
    public class DestroyOnDeath : MonoBehaviour {
        [SerializeField] private AnimationClip optionalAnimationDelay;

        private void Start() {
            GetComponent<Health>().Died += (data) => StartCoroutine(DestroyCoroutine());
        }

        private IEnumerator DestroyCoroutine() {
            yield return new WaitForOptionalAnimation(optionalAnimationDelay);
            Destroy(gameObject);
        }
    }
}