using System;
using Unbowed.Gameplay;
using UnityEngine;

namespace Unbowed.Animation
{
    public class ChestAnimator : MonoBehaviour
    {
        [SerializeField] private Chest _chest;
        [SerializeField] private Animator _animator;
        
        private static readonly int _isOpened = Animator.StringToHash("isOpened");

        private void Awake()
        {
            _chest.IsOpened.Changed += OnChestOpenedChanged;
        }

        private void OnChestOpenedChanged(bool value)
        {
            _animator.SetBool(_isOpened, value);
        }
    }
}