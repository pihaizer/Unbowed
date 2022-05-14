using System;
using System.Collections;
using Sirenix.OdinInspector;
using Unbowed.Managers.Saves;
using Unbowed.SO;
using UnityEngine;
using Zenject;

namespace Unbowed.Gameplay {
    public class GameController : MonoBehaviour {
        [SerializeField] private SceneConfig startingLocationConfig;
        [Inject] private SaveController _saveController;
        private SaveFile _save;

        private void Start() {
            _save = _saveController.Load();
            var operation = ScenesConfig.Instance.Load(new SceneChangeRequest(startingLocationConfig) {
                useLoadingScreen = true,
                setActive = true
            });
            if(operation != null) operation.completed += OnLoadCompleted;
            else OnLoadCompleted(null);
        }

        private void OnLoadCompleted(AsyncOperation obj) {
            StartCoroutine(InitPlayer());
        }

        private IEnumerator InitPlayer() {
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
            _saveController.Save(save);
        }

        private void OnApplicationQuit() {
            Save();
        }
    }
}