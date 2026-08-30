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

            // IDisposable 로도 바인딩해 컨테이너가 구독 해제를 보장
            Container.BindInterfacesAndSelfTo<EquipmentService>()
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

            // 프레젠테이션 계층 - 프레젠터 / 러너 바인딩 (일반 클래스)
            // IInitializable / IDisposable 로 바인딩되어 컨테이너가 생명주기를 관리
            // (Initialize 호출, 컨텍스트 파괴 시 Dispose 호출)
            Container.BindInterfacesAndSelfTo<GamePresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<UpgradePresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<SavePresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<ClearPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<InventoryPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<EquipmentPresenter>().AsSingle();
            Container.BindInterfacesAndSelfTo<AutoAttackRunner>().AsSingle();

            Debug.Log("========== MVP 게임 설치 성공 ==========");
        }
    }
}