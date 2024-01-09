using System.Collections.Generic;
using Kameffee.Audio;
using Kameffee.Scenes;
using Unity1week202312.ChapterTitle;
using Unity1week202312.PlaceObjects;
using Unity1week202312.Result;
using Unity1week202312.Stage;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using VContainer;
using VContainer.Unity;

namespace Unity1week202312.Installer
{
    public class RootLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private Light2D _globalLight;

        [SerializeField]
        private StageMasterDataStoreSource _stageMasterDataStoreSource;

        [SerializeField]
        private PlaceObjectMasterDataStoreSource _placeObjectMasterDataStoreSource;

        [SerializeField]
        private ScenesLifetimeScope _scenesLifetimeScope;

        [SerializeField]
        private AudioLifetimeScope _audioLifetimeScope;

        [SerializeField]
        private ChapterTitleView _chapterTitleViewPrefab;

        [SerializeField]
        private List<ChapterTitleContent> _chapterTitleContents;

        [SerializeField]
        private List<ResultContent> _resultContents;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInNewPrefab(_globalLight, Lifetime.Singleton)
                .DontDestroyOnLoad();

            _scenesLifetimeScope.Configure(builder);
            _audioLifetimeScope.Configure(builder);

            builder.RegisterInstance(_stageMasterDataStoreSource);
            builder.Register<StageMasterDataRepository>(Lifetime.Singleton);

            builder.Register<PlaceObjectMasterDataRepository>(Lifetime.Singleton);
            builder.RegisterInstance(_placeObjectMasterDataStoreSource);

            builder.RegisterComponentOnNewGameObject<AudioListener>(Lifetime.Singleton).DontDestroyOnLoad();

            builder.RegisterComponentInNewPrefab(_chapterTitleViewPrefab, Lifetime.Singleton).DontDestroyOnLoad();
            builder.Register<ChapterTitlePresenter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.RegisterInstance<IReadOnlyList<ChapterTitleContent>>(_chapterTitleContents);
            builder.Register<ChapterTitleContentRepository>(Lifetime.Singleton);

            // Result
            builder.Register<ResultContentRepository>(Lifetime.Singleton);
            builder.RegisterInstance<IReadOnlyList<ResultContent>>(_resultContents);

            builder.RegisterBuildCallback(OnBuildCallback);
        }

        private void OnBuildCallback(IObjectResolver resolver)
        {
            resolver.Resolve<AudioListener>();
            resolver.Resolve<SceneLoader>();
            resolver.Resolve<Light2D>();
        }
    }
}