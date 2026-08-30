using Application;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure;
using Presentation;
using Presentation.Presenters;
using Presentation.Views;
using UnityEngine;
using Zenject;

namespace Installers
{
    /// <summary>
    /// 인프라 게임 설치 관리
    /// Zenject DI 컨테이너 설정
    /// MVP 패턴 준수
    /// </summary>
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private HUDView hudView;
        [SerializeField] private UpgradeView upgradeView;
        [SerializeField] private GameControlView gameControlView;
        [SerializeField] private InventoryView inventoryView;
        [SerializeField] private EquipmentView equipmentView;

        /// <summary>
        /// Zenject 바인딩 설정
        /// 모든 서비스, 뷰, 프레젠터 등록
        /// </summary>
        public override void InstallBindings()
        {
            Debug.Log("========== MVP 게임 설치 시작 ==========");

            // 도메인 계층 - 모델 바인딩
            Container.Bind<Hero>()
                .AsSingle();

            // 애플리케이션 계층 - 서비스 바인딩
            Container.Bind<BattleService>()
                .AsSingle();

            Container.Bind<UpgradeService>()
                .AsSingle();

            Container.Bind<InventoryService>()
                .AsSingle();

            Container.Bind<EquipmentService>()
                .AsSingle();

            Container.Bind<ItemDropService>()
                .AsSingle();

            // 인프라 계층 - 저장소 바인딩
            // JsonSaveRepository 사용 (CsvSaveRepository로 교체 가능)
            Container.Bind<ISaveRepository>()
                .To<JsonSaveRepository>()
                .AsSingle();

            Container.Bind<IInventoryRepository>()
                .To<InventoryRepository>()
                .AsSingle();

            // 프레젠테이션 계층 - 뷰 바인딩 (MonoBehaviour)
            Container.Bind<HUDView>()
                .FromInstance(hudView)
                .AsSingle();

            Container.Bind<UpgradeView>()
                .FromInstance(upgradeView)
                .AsSingle();

            Container.Bind<GameControlView>()
                .FromInstance(gameControlView)
                .AsSingle();

            Container.Bind<ClearView>()
                .FromComponentInHierarchy()
                .AsSingle();

            Container.Bind<InventoryView>()
                .FromInstance(inventoryView)
                .AsSingle();

            Container.Bind<EquipmentView>()
                .FromInstance(equipmentView)
                .AsSingle();

            Container.Bind<SettingsView>()
                .FromComponentInHierarchy()
                .AsSingle();

            Container.Bind<SaveService>()
                .AsSingle();

            // 프레젠테이션 계층 - 프레젠터 바인딩 (일반 클래스)
            // 모든 프레젠터는 Zenject가 자동 생성 및 주입
            Container.Bind<GamePresenter>()
                .AsSingle()
                .NonLazy();

            Container.Bind<UpgradePresenter>()
                .AsSingle()
                .NonLazy();

            Container.Bind<SavePresenter>()
                .AsSingle()
                .NonLazy();

            Container.Bind<ClearPresenter>()
                .AsSingle()
                .NonLazy();

            Container.Bind<InventoryPresenter>()
                .AsSingle()
                .NonLazy();

            Container.Bind<EquipmentPresenter>()
                .AsSingle()
                .NonLazy();

            // 프레젠테이션 계층 - 러너 바인딩 (일반 클래스)
            Container.Bind<AutoAttackRunner>()
                .AsSingle()
                .NonLazy();

            Debug.Log("========== MVP 게임 설치 성공 ==========");
        }
    }
}