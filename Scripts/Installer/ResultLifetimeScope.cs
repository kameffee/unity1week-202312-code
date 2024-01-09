using Kameffee.Scenes;
using Unity1week202312.Result;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Unity1week202312.Installer
{
    public class ResultLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private ResultSceneData _debugSceneData;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<ResultEntryPoint>();
            builder.RegisterComponentInHierarchy<ResultCanvasView>();

            builder.Register<CreateResultContentViewModelUseCase>(Lifetime.Scoped);

            builder.RegisterBuildCallback(resolver =>
            {
                var container = resolver.Resolve<SceneDataContainer>();
                if (!container.TryGet<ResultSceneData>(out _))
                {
                    container.Set(_debugSceneData);
                }
            });
        }
    }
}