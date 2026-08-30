using Domain.Entities;

namespace Domain.Interfaces
{
    /// <summary>
    /// 인터페이스
    /// 게임 데이터 저장 및 로드 기능 정의
    /// 구현체: JsonSaveRepository, CsvSaveRepository (DI로 교체 가능)
    /// </summary>
    public interface ISaveRepository
    {
        /// <summary>
        /// 게임 데이터 저장
        /// </summary>
        void Save(GameData data);

        /// <summary>
        /// 게임 데이터 로드
        /// 저장 파일이 없으면 기본값 반환
        /// </summary>
        GameData Load();
    }
}