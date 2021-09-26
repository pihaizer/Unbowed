using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gameplay;
using UnityEngine;
using Utility;

public class DamageZone : MonoBehaviour {
    [SerializeField] float damage = 1f;
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
            target.Hit(damage, gameObject);
        }
    }
}