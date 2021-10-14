using System;
using System.Collections;
using Sirenix.OdinInspector;
using Unbowed.Managers.Saves;
using Unbowed.SO;
using UnityEngine;

namespace Unbowed.Gameplay {
    public class GameController : MonoBehaviour {
        [SerializeField] SceneConfig startingLocationConfig;
        SaveFile _save;

        void Start() {
            _save = SaveController.Load();
            var operation = ScenesConfig.Instance.Load(new SceneChangeRequest(startingLocationConfig) {
                useLoadingScreen = true
            });
            if(operation != null) operation.completed += OnLoadCompleted;
            else OnLoadCompleted(null);
        }

        void OnLoadCompleted(AsyncOperation obj) {
            StartCoroutine(InitPlayer());
        }

        IEnumerator InitPlayer() {
            if (_save.characters.Count == 0) yield break;
            yield return new WaitUntil(() => ActivePlayer.Exists && ActivePlayer.Get().IsStarted);
            var player = ActivePlayer.Get();
            _save.characters[0].ApplyToCharacter(player);
        }

        [Button]
        public void Save() {
            var save = new SaveFile();
            var playerSave = CharacterSave.FromCharacter(ActivePlayer.Get());
            save.characters.Add(playerSave);
            SaveController.Save(save);
        }

        void OnApplicationQuit() {
            Save();
        }
    }
}