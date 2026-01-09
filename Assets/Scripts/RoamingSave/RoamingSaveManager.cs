using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RoamingSave
{
    public class RoamingSaveManager : MonoBehaviour
    {
        [Header("Config")]
        public string cloudSlotId = "deviceA";

        [Header("UI")]
        public TMP_Text pointsText;
        public ConflictPopup conflictPopup;
        public Button syncNowButton;
        public Button clearLocalButton;

        private LocalSaveStorage _local;
        private CloudSaveSimulator _cloud;

        private SaveData _current;

        private void Awake()
        {
            _local = new LocalSaveStorage();
            _cloud = new CloudSaveSimulator(cloudSlotId);
            
            syncNowButton.onClick.AddListener(SyncUpload);
            clearLocalButton.onClick.AddListener(ClearLocalSave);
        }

        private void Start()
        {
            SyncAtStartup();
        }
        
        private void SyncAtStartup()
        {
            var localData = _local.Load();
            var cloudData = _cloud.Download();
            
            if (localData == null && cloudData == null)
            {
                _current = SaveData.New(0);
                _local.Save(_current);
                UpdateUI();
                return;
            }

            if (localData == null && cloudData != null)
            {
                _current = cloudData;
                _local.Save(_current);
                UpdateUI();
                return;
            }
            
            if (localData != null && cloudData == null)
            {
                _current = localData;
                UpdateUI();
                return;
            }
            
            bool conflict = localData.totalPoints != cloudData.totalPoints;

            if (!conflict)
            {
                _current = (localData.unixTimeUtc >= cloudData.unixTimeUtc) ? localData : cloudData;
                _local.Save(_current);
                UpdateUI();
                return;
            }

            var msg =
                $"Wykryto konflikt zapisu!\n\n" +
                $"LOCAL: points={localData.totalPoints}, time={localData.unixTimeUtc}\n" +
                $"CLOUD: points={cloudData.totalPoints}, time={cloudData.unixTimeUtc}\n\n" +
                $"Wybierz, który zapis zachować.";

            conflictPopup.Show(
                msg,
                onUseLocal: () =>
                {
                    _current = localData;
                    _cloud.Upload(_current);
                    UpdateUI();
                },
                onUseCloud: () =>
                {
                    _current = cloudData;
                    _local.Save(_current);
                    UpdateUI();
                });
        }

        public void AddPoints(int amount)
        {
            if (_current == null) _current = SaveData.New(0);

            _current.totalPoints += amount;
            _current.unixTimeUtc = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            _current.version++;
            
            _local.Save(_current);
            UpdateUI();
        }

        public void SyncUpload()
        {
            if (_current == null) return;
            _cloud.Upload(_current);
        }

        private void ClearLocalSave()
        {
            _local.Clear();
            _current = SaveData.New(0);
            _local.Save(_current);
            UpdateUI();
        }

        private void UpdateUI()
        {
            pointsText.text = $"Points: {_current?.totalPoints ?? 0}";
        }
    }
}
