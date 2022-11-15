using BirdFight.Interfaces;
using UnityEngine;

namespace BirdFight
{
    internal class Enemy : IEnemy
    {
        private const float KMoney = 5f;
        private const float KPower = 1.5f;
        private const float KCrimeRate = 3f;
        private const float MaxHeathPlayer = 20f;

        private readonly string _name;

        private int _moneyPlayer;
        private int _powerPlayer;
        private int _healthPlayer;
        private int _crimeRatePlayer;

        public Enemy(string name) => _name = name;
        
        public void Update(PlayerData playerData)
        {
            switch (playerData.DataType)
            {
                case DataType.Money:
                    _moneyPlayer = playerData.Value;
                    break;

                case DataType.Health:
                    _healthPlayer = playerData.Value;
                    break;

                case DataType.Power:
                    _powerPlayer = playerData.Value;
                    break;

                case DataType.CrimeRate:
                    _crimeRatePlayer = playerData.Value;
                    break;
            }

            Debug.Log($"Notified {_name} change to {playerData}");
        }

        public int CalculationPower()
        {
            int kHealth = CalculationHealth();
            float moneyRatio = _moneyPlayer / KMoney;
            float powerRatio = _powerPlayer / KPower;
            float crimeRateRatio = _crimeRatePlayer / KCrimeRate;

            return (int)(moneyRatio + kHealth + powerRatio + crimeRateRatio);
        }

        private int CalculationHealth() => _healthPlayer > MaxHeathPlayer ? 100 : 5;
    }
}