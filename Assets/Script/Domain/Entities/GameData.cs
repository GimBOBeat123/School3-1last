namespace Domain.Entities
{
    /// <summary>
    /// 도메인
    /// 게임 저장 시 필요한 데이터
    /// </summary>
    [System.Serializable]
    public class GameData
    {
        /// <summary>
        /// 현재 공격력
        /// </summary>
        public int Attack;

        /// <summary>
        /// 현재 골드
        /// </summary>
        public int Gold;

        /// <summary>
        /// 현재 라운드
        /// </summary>
        public int Round;
    }
}