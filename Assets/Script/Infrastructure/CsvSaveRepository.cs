using System.IO;
using Domain.Entities;
using Domain.Interfaces;
using UnityEngine;

namespace Infrastructure
{
    /// <summary>
    /// 인프라 CSV 저장
    /// 게임 데이터를 CSV 형식으로 저장 및 로드
    /// DI를 통해 JsonSaveRepository와 교체 가능
    /// </summary>
    public class CsvSaveRepository : ISaveRepository
    {
        private readonly string path;

        /// <summary>
        /// CSV 저장소 생성
        /// 저장 경로 설정
        /// </summary>
        public CsvSaveRepository()
        {
            path = UnityEngine.Application.persistentDataPath + "/save.csv";
        }

        /// <summary>
        /// 게임 데이터를 CSV로 저장
        /// 형식: 공격력,골드,라운드
        /// </summary>
        public void Save(GameData data)
        {
            string csv =
                $"{data.Attack},{data.Gold},{data.Round}";

            File.WriteAllText(path, csv);
        }

        /// <summary>
        /// CSV 파일에서 게임 데이터 로드
        /// 파일이 없으면 기본값 반환
        /// </summary>
        public GameData Load()
        {
            if (!File.Exists(path))
                return new GameData();

            string csv = File.ReadAllText(path);

            string[] split = csv.Split(',');

            return new GameData
            {
                Attack = int.Parse(split[0]),
                Gold = int.Parse(split[1]),
                Round = int.Parse(split[2])
            };
        }
    }
}