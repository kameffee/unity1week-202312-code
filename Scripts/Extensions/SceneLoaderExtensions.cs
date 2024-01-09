using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Kameffee.Scenes;

namespace Unity1week202312.Extensions
{
    public static class SceneLoaderExtensions
    {
        public static async UniTask LoadAsync(
            this SceneLoader sceneLoader,
            SceneDefine nextScene,
            CancellationToken cancellationToken = default)
        {
            if (!Enum.IsDefined(typeof(SceneDefine), nextScene))
            {
                throw new OperationCanceledException();
            }

            await sceneLoader.LoadAsync((int)nextScene, cancellationToken);
        }
    }
}