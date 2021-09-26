using System.Collections;
using UnityEngine;
using Utility;

namespace Gameplay.OnDeath {
    [RequireComponent(typeof(Mortal))]
    public class DestroyOnDeath : MonoBehaviour {
        [SerializeField] AnimationClip optionalAnimationDelay;

        void Start() {
            GetComponent<Mortal>().isDead.Changed += value => {
                if (value) StartCoroutine(DestroyCoroutine());
            };
        }

        IEnumerator DestroyCoroutine() {
            yield return new WaitForOptionalAnimation(optionalAnimationDelay);
            Destroy(gameObject);
        }
    }
}