using UniRx;

namespace Domain.Entities
{
    /// <summary>
    /// 도메인
    /// 플레이어가 처치하는 적
    /// </summary>
    public class Monster
    {
        /// <summary>
        /// 최대 체력
        /// </summary>
        public ReactiveProperty<int> MaxHp = new();

        /// <summary>
        /// 현재 체력
        /// </summary>
        public ReactiveProperty<int> CurrentHp = new();

        /// <summary>
        /// 몬스터 생성
        /// </summary>
        public Monster(int hp)
        {
            MaxHp.Value = hp;
            CurrentHp.Value = hp;
        }

        /// <summary>
        /// 피해 받기
        /// 체력이 0 이하면 죽음
        /// </summary>
        public bool TakeDamage(int damage)
        {
            CurrentHp.Value -= damage;

            if (CurrentHp.Value <= 0)
            {
                CurrentHp.Value = 0;
                return true;
            }

            return false;
        }
    }
}