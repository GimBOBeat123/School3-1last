using System.IO;
using Domain.Entities;
using Domain.Interfaces;
using UnityEngine;

namespace Infrastructure
{
    /// <summary>
    /// 인프라 JSON 저장
    /// 게임 데이터를 JSON 형식으로 저장 및 로드
    /// </summary>
    public class JsonSaveRepository : ISaveRepository
    {
        private readonly string path;

        /// <summary>
        /// JSON 저장소 생성
        /// 저장 경로 설정
        /// </summary>
        public JsonSaveRepository()
        {
            path = UnityEngine.Application.persistentDataPath + "/save.json";
        }

        /// <summary>
        /// 게임 데이터를 JSON으로 저장
        /// </summary>
        public void Save(GameData data)
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(path, json);
            Debug.Log($"저장 완료\n{json}");
        }

        /// <summary>
        /// JSON 파일에서 게임 데이터 로드
        /// 파일이 없으면 기본값 반환
        /// </summary>
        public GameData Load()
        {
            if (!File.Exists(path))
            {
                Debug.Log("저장 파일 없음");
                return new GameData();
            }
            string json = File.ReadAllText(path);
            Debug.Log($"로드 완료\n{json}");

            return JsonUtility.FromJson<GameData>(json);
        }
    }
}