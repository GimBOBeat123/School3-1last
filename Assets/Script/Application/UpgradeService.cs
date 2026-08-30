using Domain.Entities;

namespace Application
{
    /// <summary>
    /// 모델
    /// 골드를 소모하여 공격력 증가
    /// </summary>
    public class UpgradeService
    {
        private readonly Hero hero;

        /// <summary>
        /// 업그레이드 서비스 생성
        /// </summary>
        public UpgradeService(Hero hero)
        {
            this.hero = hero;
        }

        /// <summary>
        /// 다음 업그레이드에 필요한 골드
        /// 현재 공격력 x 10
        /// </summary>
        public int UpgradeCost =>
            hero.Attack.Value * 10;

        /// <summary>
        /// 공격력 업그레이드 실행
        /// 골드가 충분하면 공격력 1 증가
        /// </summary>
        public bool Upgrade()
        {
            if (!hero.SpendGold(UpgradeCost))
                return false;

            hero.IncreaseAttack(1);

            return true;
        }
    }
}