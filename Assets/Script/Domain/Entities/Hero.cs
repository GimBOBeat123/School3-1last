using UniRx;

namespace Domain.Entities
{
    /// <summary>
    /// 도메인
    /// 플레이어가 조작하는 캐릭터
    /// </summary>
    public class Hero
    {
        /// <summary>
        /// 현재 공격력
        /// </summary>
        public ReactiveProperty<int> Attack = new(1);

        /// <summary>
        /// 현재 골드
        /// </summary>
        public ReactiveProperty<int> Gold = new(0);

        /// <summary>
        /// 공격력 증가
        /// </summary>
        public void IncreaseAttack(int value)
        {
            Attack.Value += value;
        }

        /// <summary>
        /// 골드 증가
        /// </summary>
        public void AddGold(int value)
        {
            Gold.Value += value;
        }

        /// <summary>
        /// 골드 소모
        /// 골드가 충분하면 소모하고 참 반환
        /// </summary>
        public bool SpendGold(int value)
        {
            if (Gold.Value < value)
                return false;

            Gold.Value -= value;
            return true;
        }

        /// <summary>
        /// 공격력 설정
        /// </summary>
        public void SetAttack(int value)
        {
            Attack.Value = value;
        }

        /// <summary>
        /// 골드 설정
        /// </summary>
        public void SetGold(int value)
        {
            Gold.Value = value;
        }
    }
}