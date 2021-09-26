﻿using UnityEngine;

public class ParticlesBuilder {
    readonly ParticleSystem _particleSystem;
    Vector3 _spawnPosition;
    
    public ParticlesBuilder(ParticleSystem particleSystem) {
        _particleSystem = particleSystem;
    }

    public ParticleSystem Build() {
        var particles = Object.Instantiate(_particleSystem, _spawnPosition, Quaternion.identity);
        return particles;
    }

    public ParticlesBuilder SetSpawnPosition(Vector3 position) {
        _spawnPosition = position;
        return this;
    }

    public ParticlesBuilder SetSortingLayer(string name) {
        _particleSystem.GetComponent<ParticleSystemRenderer>().sortingLayerName = name;
        return this;
    }
}