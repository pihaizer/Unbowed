using System.Collections;
using System.Linq;
using UnityEngine;

namespace Unbowed.Gameplay {
    public class DamageZone : MonoBehaviour {
        [SerializeField] private int damage = 1;
        [SerializeField] private float radius = 3f;
        [SerializeField] private float timeBetweenAttacks = 1f;
        private Coroutine _hitCoroutine;

        private void OnEnable() {
            _hitCoroutine = StartCoroutine(HitCoroutine());
        }

        private void OnDisable() {
            StopCoroutine(_hitCoroutine);
        }

        private IEnumerator HitCoroutine() {
            while (true) {
                yield return new WaitForSeconds(timeBetweenAttacks);
                var colliders = Physics.OverlapSphere(transform.position, radius);
                var targets = colliders
                    .Where(col=>col.GetComponentInParent<IHittable>() != null)
                    .Select(col=>col.GetComponentInParent<IHittable>());
                foreach (var target in targets) {
                    TryHit(target);
                }
            }
        }

        private void TryHit(IHittable target) {
            if (target.CanBeHit()) {
                target.Hit(damage, null);
            }
        }
    }
}