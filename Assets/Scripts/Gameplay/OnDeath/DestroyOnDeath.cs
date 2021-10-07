using System.Collections;
using Unbowed.Gameplay.Characters.Modules;
using Unbowed.Utility.WaitFor;
using UnityEngine;

namespace Unbowed.Gameplay.OnDeath {
    [RequireComponent(typeof(Health))]
    public class DestroyOnDeath : MonoBehaviour {
        [SerializeField] AnimationClip optionalAnimationDelay;

        void Start() {
            GetComponent<Health>().Died += () => StartCoroutine(DestroyCoroutine());
        }

        IEnumerator DestroyCoroutine() {
            yield return new WaitForOptionalAnimation(optionalAnimationDelay);
            Destroy(gameObject);
        }
    }
}