using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Health
{
    public class EnemyHealthController : EntityHealthController
    {
        private string _enemyName;
        private PlayerController _player;
        
        public override float CurrentHealth
        {
            get => base.CurrentHealth;
            set
            {
                if (value <= 0) 
                    AddScoreAndKillCount();
                base.CurrentHealth = value;
            }
        }

        protected override void Start()
        {
            base.Start();
            
            _enemyName = name.Split(' ')[0];
            _player = GameObject.Find("Player").GetComponent<PlayerController>();
        }

        private void AddScoreAndKillCount()
        {
            switch (_enemyName)
            {
                case "Enemy1":
                    _player.Score += 30;
                    break;
                case "Enemy2":
                    _player.Score += 10;
                    break;
                case "Miniboss":
                    _player.Score += 1000;
                    break;
            }
            _player.EnemyKillCounter++;
        }
    }
}