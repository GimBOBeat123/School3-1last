using Domain.Entities;
using UniRx;
using UnityEngine;

namespace Application
{
    /// <summary>
    /// 모델
    /// 라운드, 몬스터, 전투 로직 제어
    /// </summary>
    public class BattleService
    {
        /// <summary>
        /// 현재 라운드 번호
        /// </summary>
        public ReactiveProperty<int> CurrentRound = new(1);

        /// <summary>
        /// 현재 출현한 몬스터
        /// </summary>
        public ReactiveProperty<Monster> CurrentMonster = new();

        /// <summary>
        /// 게임 클리어 여부
        /// </summary>
        public ReactiveProperty<bool> IsGameClear = new(false);

        /// <summary>
        /// 영웅 정보
        /// </summary>
        public Hero Hero { get; }

        private ItemDropService itemDropService;

        /// <summary>
        /// 전투 서비스 생성
        /// </summary>
        public BattleService(Hero hero, ItemDropService itemDropService)
        {
            Debug.Log("BattleService 생성됨");
            Hero = hero;
            this.itemDropService = itemDropService;

            SpawnMonster();
        }

        /// <summary>
        /// 몬스터에게 공격 실행
        /// 몬스터 처치 시 라운드 증가 및 보상 처리
        /// </summary>
        public void Attack()
        {
            Debug.Log("공격!");

            if (CurrentMonster.Value == null)
                return;

            bool dead =
                CurrentMonster.Value.TakeDamage(
                    Hero.Attack.Value);

            if (dead)
            {
                if (CurrentRound.Value == 50)
                {
                    IsGameClear.Value = true;
                    return;
                }

                int goldReward = CurrentRound.Value * 5;
                Hero.AddGold(goldReward);

                // 아이템 드롭
                itemDropService?.DropRandomWeaponOnMonsterKill(Vector3.zero);

                CurrentRound.Value++;

                SpawnMonster();
            }
        }

        /// <summary>
        /// 새로운 몬스터 생성
        /// 라운드에 따라 체력 결정
        /// </summary>
        private void SpawnMonster()
        {
            int hp;

            if (CurrentRound.Value == 50)
            {
                hp = 5000;
            }
            else
            {
                hp = CurrentRound.Value *
                     CurrentRound.Value *
                     10;
            }
            Debug.Log($"몬스터 생성 체력={hp}");
            CurrentMonster.Value =
                new Monster(hp);
        }

        /// <summary>
        /// 저장된 게임 데이터로 복구
        /// 라운드, 골드, 공격력 복구
        /// </summary>
        public void Restore(GameData data)
        {
            Hero.SetAttack(data.Attack);
            Hero.SetGold(data.Gold);

            CurrentRound.Value = data.Round;

            SpawnMonster();
        }
    }
}