using System;
using UnityEngine;

namespace Unbowed.Gameplay
{
    public class SkyboxRotator : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 0.1f;
        
        private static readonly int _rotationId = Shader.PropertyToID("_Rotation");


        private void Update()
        {
            if (!RenderSettings.skybox.HasFloat(_rotationId)) return;
            RenderSettings.skybox.SetFloat(_rotationId, _rotationSpeed * Time.time);
        }
    }
}