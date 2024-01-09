using Kameffee.Audio;
using Kameffee.License;
using Unity1week202312.Title;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Unity1week202312.Installer
{
    public class TitleLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private LicenseView _licenseViewPrefab;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<TitleCanvasView>();
            builder.RegisterEntryPoint<TitleEntryPoint>(Lifetime.Singleton);
            builder.RegisterComponentInHierarchy<StartPerformer>();

            builder.Register<GetLicenseTextUseCase>(Lifetime.Scoped).WithParameter("License");
            builder.Register<LicensePresenter>(Lifetime.Scoped).AsImplementedInterfaces().AsSelf();
            builder.RegisterComponentInNewPrefab<LicenseView>(_licenseViewPrefab, Lifetime.Scoped);

            builder.RegisterEntryPoint<AudioSettingsPresenter>();
            builder.RegisterComponentInHierarchy<AudioSettingView>();
        }
    }
}