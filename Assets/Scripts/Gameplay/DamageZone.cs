using System.Collections;
using System.Linq;
using UnityEngine;

namespace Unbowed.Gameplay {
    public class DamageZone : MonoBehaviour {
        [SerializeField] int damage = 1;
        [SerializeField] float radius = 3f;
        [SerializeField] float timeBetweenAttacks = 1f;
        Coroutine _hitCoroutine;

        void OnEnable() {
            _hitCoroutine = StartCoroutine(HitCoroutine());
        }

        void OnDisable() {
            StopCoroutine(_hitCoroutine);
        }

        IEnumerator HitCoroutine() {
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
    
        void TryHit(IHittable target) {
            if (target.CanBeHit()) {
                target.Hit(damage, null);
            }
        }
    }
}