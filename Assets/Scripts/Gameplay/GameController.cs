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
        [Inject] private ISaveController _saveController;

        private const string _saveKey = "save";
        private SaveFile _save;

        private async void Start() {
            _save = await _saveController.GetAsync<SaveFile>(_saveKey);
            AsyncOperation operation = ScenesConfig.Instance.Load(new SceneChangeRequest(startingLocationConfig) {
                UseLoadingScreen = true,
                SetActive = true
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
            CharacterSave playerSave = CharacterSave.FromCharacter(ActivePlayer.Get());
            save.characters.Add(playerSave);
            _saveController.SetAsync(_saveKey, save);
        }

        private void OnApplicationQuit() {
            Save();
        }
    }
}