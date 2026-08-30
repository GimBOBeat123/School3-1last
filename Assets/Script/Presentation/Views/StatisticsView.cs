using TMPro;
using UnityEngine;

namespace Presentation.Views
{
    /// <summary>
    /// 게임 진행 중 누적 통계를 표시하는 View
    /// - 총 처치 몬스터 수
    /// - 총 획득 골드
    /// </summary>
    public class StatisticsView : MonoBehaviour
    {
        [SerializeField] private TMP_Text totalMonsterKillText;
        [SerializeField] private TMP_Text totalGoldEarnedText;

        private int totalMonsterKilled = 0;
        private int totalGoldEarned = 0;

        private void Awake()
        {
            UpdateDisplay();
        }

        public void OnMonsterKilled()
        {
            totalMonsterKilled++;
            UpdateDisplay();
        }

        public void OnGoldEarned(int amount)
        {
            totalGoldEarned += amount;
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            if (totalMonsterKillText != null)
                totalMonsterKillText.text = $"Kill: {totalMonsterKilled}";

            if (totalGoldEarnedText != null)
                totalGoldEarnedText.text = $"Get: {totalGoldEarned}";
        }

        public void ResetStatistics()
        {
            totalMonsterKilled = 0;
            totalGoldEarned = 0;
            UpdateDisplay();
        }
    }
}
