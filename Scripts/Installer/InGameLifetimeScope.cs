using System.Collections.Generic;
using Unity1week202312.GridCell;
using Unity1week202312.InGame;
using Unity1week202312.PlaceBoard;
using Unity1week202312.PlaceBoard.History;
using Unity1week202312.PlaceObjects;
using Unity1week202312.Result;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Unity1week202312.Installer
{
    public class InGameLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private GridCellView _gridCellViewPrefab;

        [SerializeField]
        private Transform _gridCellViewParent;

        [SerializeField]
        private Transform _placeObjectViewParent;

        [SerializeField]
        private List<PlaceObjectView> _placeObjectViewPrefabs;

        [SerializeField]
        private Transform _placedObjectViewParent;

        [SerializeField]
        private Transform _graggingObjectViewParent;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<InGameLoop>();
            builder.Register<InGameSetupUseCase>(Lifetime.Scoped);
            builder.RegisterComponentInHierarchy<InGameUIView>();
            builder.Register<StageService>(Lifetime.Scoped);

            builder.RegisterComponentInHierarchy<ChapterTitleTextView>();
            builder.Register<ChapterTitleTextPresenter>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();

            builder.Register<PlaceBoardHistory>(Lifetime.Scoped);
            builder.Register<PlaceBoardRecordUseCase>(Lifetime.Scoped);

            builder.RegisterInstance<IReadOnlyList<PlaceObjectView>>(_placeObjectViewPrefabs);

            builder.RegisterComponent<GridCellView>(_gridCellViewPrefab);
            builder.Register<GridCellViewFactory>(Lifetime.Scoped).WithParameter(_gridCellViewParent);
            builder.Register<GridPresenter>(Lifetime.Scoped).AsSelf().AsImplementedInterfaces();

            builder.Register<PlaceObjectViewFactory>(Lifetime.Scoped)
                .WithParameter(_placeObjectViewParent);
            builder.RegisterComponentInHierarchy<PlaceObjectHolderView>();
            builder.RegisterEntryPoint<PlaceObjectPresenter>();
            builder.Register<PlaceObjectFactory>(Lifetime.Scoped);

            builder.Register<PlaceObjectMasterDataRepository>(Lifetime.Scoped);
            builder.Register<DraggingPlaceObjectFactory>(Lifetime.Scoped)
                .WithParameter(_graggingObjectViewParent);

            builder.Register<SelectPlaceObjectUseCase>(Lifetime.Scoped);
            builder.Register<PlaceableObjectContainer>(Lifetime.Scoped);
            builder.Register<NextPlaceObjectUseCase>(Lifetime.Scoped);
            builder.Register<PlaceObjectLotteryer>(Lifetime.Scoped);

            builder.Register<PlaceUseCase>(Lifetime.Scoped);
            builder.Register<PlaceBoardSetupUseCase>(Lifetime.Scoped);
            builder.Register<PlaceBoardFactory>(Lifetime.Scoped);
            builder.Register<PlaceBoardService>(Lifetime.Scoped);
            builder.Register<PlaceableMap>(Lifetime.Scoped);

            builder.Register<ResultCalculator>(Lifetime.Scoped);

            builder.RegisterComponentInHierarchy<PackingBoxPerformer>();

            builder.Register<PlaceableSpaceCalculator>(Lifetime.Scoped);

            builder.Register<PlaceBoardPresenter>(Lifetime.Scoped)
                .WithParameter(_placedObjectViewParent)
                .AsImplementedInterfaces()
                .AsSelf();

            builder.RegisterEntryPoint<InGameUpdater>(Lifetime.Scoped);

            // Guidance
            builder.RegisterComponentInHierarchy<GuidanceView>();
            builder.Register<InGameUserState>(Lifetime.Scoped);
            builder.Register<GuidancePresenter>(Lifetime.Scoped).AsImplementedInterfaces();
        }
    }
}