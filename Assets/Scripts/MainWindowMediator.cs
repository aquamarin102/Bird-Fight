using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BirdFight
{
    internal class MainWindowMediator : MonoBehaviour
    {
        [SerializeField] private int _maxCrimeRate = 2;

        [Header("Player Stats")]
        [SerializeField] private TMP_Text _countMoneyText;
        [SerializeField] private TMP_Text _countHealthText;
        [SerializeField] private TMP_Text _countPowerText;
        [SerializeField] private TMP_Text _countCrimeRateText;

        [Header("Enemy Stats")]
        [SerializeField] private TMP_Text _countPowerEnemyText;

        [Header("Money Buttons")]
        [SerializeField] private Button _plusMoneyButton;
        [SerializeField] private Button _minusMoneyButton;

        [Header("Health Buttons")]
        [SerializeField] private Button _plusHealthButton;
        [SerializeField] private Button _minusHealthButton;

        [Header("Power Buttons")]
        [SerializeField] private Button _plusPowerButton;
        [SerializeField] private Button _minusPowerButton;

        [Header("Crime Rate Buttons")]
        [SerializeField] private Button _plusCrimeRateButton;
        [SerializeField] private Button _minusCrimeRateButton;

        [Header("Other Buttons")]
        [SerializeField] private Button _fightButton;
        [SerializeField] private Button _skipFightButton;

        private int _allCountMoneyPlayer;
        private int _allCountHealthPlayer;
        private int _allCountPowerPlayer;
        private int _allCountCrimeRatePlayer;

        private PlayerData _money;
        private PlayerData _heath;
        private PlayerData _power;
        private PlayerData _crimeRate;

        private Enemy _enemy;

        private void Start()
        {
            _enemy = new Enemy("Enemy Flappy");

            _money = CreatePlayerData(DataType.Money);
            _heath = CreatePlayerData(DataType.Health);
            _power = CreatePlayerData(DataType.Power);
            _crimeRate = CreatePlayerData(DataType.CrimeRate);

            Subscribe();

            UpdateVisibleSkipFightButton();
        }
        
        private void OnDestroy()
        {
            DisposePlayerData(ref _money);
            DisposePlayerData(ref _heath);
            DisposePlayerData(ref _power);
            DisposePlayerData(ref _crimeRate);

            Unsubscribe();
        }
        
        private PlayerData CreatePlayerData(DataType dataType)
        {
            PlayerData playerData = new PlayerData(dataType);
            playerData.Attach(_enemy);

            return playerData;
        }
        
        private void  DisposePlayerData (ref PlayerData playerData)
        {
            playerData.Detach(_enemy);
            playerData = null;
        }
        
        private void Subscribe()
        {
            _plusMoneyButton.onClick.AddListener(IncreaseMoney);
            _minusMoneyButton.onClick.AddListener(DecreaseMoney);

            _plusHealthButton.onClick.AddListener(IncreaseHealth);
            _minusHealthButton.onClick.AddListener(DecreaseHealth);

            _plusPowerButton.onClick.AddListener(IncreasePower);
            _minusPowerButton.onClick.AddListener(DecreasePower);

            _plusCrimeRateButton.onClick.AddListener(IncreaseCrimeRate);
            _minusCrimeRateButton.onClick.AddListener(DecreaseCrimeRate);

            _fightButton.onClick.AddListener(Fight);
            _skipFightButton.onClick.AddListener(SkipBattle);
        }

        private void Unsubscribe()
        {
            _plusMoneyButton.onClick.RemoveAllListeners();
            _minusMoneyButton.onClick.RemoveAllListeners();

            _plusHealthButton.onClick.RemoveAllListeners();
            _minusHealthButton.onClick.RemoveAllListeners();

            _plusPowerButton.onClick.RemoveAllListeners();
            _minusPowerButton.onClick.RemoveAllListeners();

            _plusCrimeRateButton.onClick.RemoveAllListeners();
            _minusCrimeRateButton.onClick.RemoveAllListeners();

            _fightButton.onClick.RemoveAllListeners();
            _skipFightButton.onClick.RemoveAllListeners();
        }

        private void IncreaseMoney() => IncreaseValue(ref _allCountMoneyPlayer, DataType.Money);
        private void DecreaseMoney() => DecreaseValue(ref _allCountMoneyPlayer, DataType.Money);

        private void IncreaseHealth() => IncreaseValue(ref _allCountHealthPlayer, DataType.Health);
        private void DecreaseHealth() => DecreaseValue(ref _allCountHealthPlayer, DataType.Health);
        
        private void IncreasePower() => IncreaseValue(ref _allCountPowerPlayer, DataType.Power);
        private void DecreasePower() => DecreaseValue(ref _allCountPowerPlayer, DataType.Power);

        private void IncreaseCrimeRate() => IncreaseValue(ref _allCountCrimeRatePlayer, DataType.CrimeRate);
        private void DecreaseCrimeRate() => DecreaseValue(ref _allCountCrimeRatePlayer, DataType.CrimeRate);

        private void IncreaseValue(ref int value, DataType dataType) => AddToValue(ref value, 1, dataType);
        private void DecreaseValue(ref int value, DataType dataType) => AddToValue(ref value, -1, dataType);

        private void AddToValue(ref int value, int addition, DataType dataType)
        {
            value += addition;
            ChangeDataWindow(value, dataType);
        }

        private void ChangeDataWindow(int countChangeData, DataType dataType)
        {
            PlayerData playerData = GetPlayerData(dataType);
            TMP_Text textComponent = GetTextComponent(dataType);
            string dataTypeString = GetDataTypeString(dataType);
            string text = $"Player {dataTypeString:F}: {countChangeData}";

            playerData.Value = countChangeData;
            textComponent.text = text;

            int enemyPower = _enemy.CalculationPower();
            _countPowerEnemyText.text = $"Enemy Power {enemyPower}";
            
            if(dataType == DataType.CrimeRate)
                UpdateVisibleSkipFightButton();
        }
        
        private PlayerData GetPlayerData(DataType dataType) =>
            dataType switch
            {
                DataType.Money => _money,
                DataType.Health => _heath,
                DataType.Power => _power,
                DataType.CrimeRate => _crimeRate,
                _ => throw new ArgumentException($"Wrong{nameof(DataType)}")
            };
        
        private TMP_Text GetTextComponent(DataType dataType) =>
            dataType switch
            {
                DataType.Money => _countMoneyText,
                DataType.Health => _countHealthText,
                DataType.Power => _countPowerText,
                DataType.CrimeRate => _countCrimeRateText,
                _ => throw new ArgumentException($"Wrong {nameof(DataType)}")
            };
        
        private string GetDataTypeString(DataType dataType) =>
            dataType switch
            {
                DataType.Money => "Money",
                DataType.Health => "Health",
                DataType.Power => "Power",
                DataType.CrimeRate => "Crime Rate",
                _ => throw new ArgumentException($"Wrong {nameof(DataType)}")
            };

        private void UpdateVisibleSkipFightButton()
        {
            PlayerData playerCrimeRateData = GetPlayerData(DataType.CrimeRate);
            bool buttonActive = playerCrimeRateData.Value <= _maxCrimeRate;
            _skipFightButton.gameObject.SetActive(buttonActive);
        }

        private void Fight()
        {
            int enemyPower = _enemy.CalculationPower();
            bool isVictory = _allCountPowerPlayer >= enemyPower;
            
            string color = isVictory? "#07FF00" : "#FF0000";
            string message = isVictory ? "Win" : "Lose";

            Debug.Log($"<color={color}>{message}!!!</color>");
        }
        
        private void SkipBattle()
        {
            Debug.Log("Battle was skipped");
        }
    }
}