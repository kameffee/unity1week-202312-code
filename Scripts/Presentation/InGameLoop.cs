using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Kameffee.Extensions;
using Kameffee.Scenes;
using Unity1week202312.ChapterTitle;
using Unity1week202312.InGame;
using Unity1week202312.Stage;
using Unity1week202312.PlaceBoard.History;
using Unity1week202312.PlaceObjects;
using Unity1week202312.Result;
using UnityEngine;
using VContainer.Unity;

namespace Unity1week202312
{
    public class InGameLoop : Presenter, IInitializable, IAsyncStartable
    {
        private readonly InGameSetupUseCase _inGameSetupUseCase;
        private readonly NextPlaceObjectUseCase _nextPlaceObjectUseCase;
        private readonly PlaceUseCase _placeUseCase;
        private readonly PackingBoxPerformer _packingBoxPerformer;
        private readonly InGameUIView _inGameUIView;
        private readonly PlaceObjectHolderView _placeObjectHolderView;
        private readonly PlaceBoardRecordUseCase _placeBoardRecordUseCase;
        private readonly SceneLoader _sceneLoader;
        private readonly IChapterTitlePerformer _chapterTitlePerformer;
        private readonly ResultCalculator _resultCalculator;
        private readonly SceneDataContainer _sceneDataContainer;
        private readonly PlaceBoardHistory _placeBoardHistory;
        private readonly ChapterTitleTextPresenter _chapterTitleTextPresenter;
        private readonly IGuidancePerformer _guidancePerformer;
        private readonly InGameUserState _inGameUserState;

        public InGameLoop(
            InGameSetupUseCase inGameSetupUseCase,
            NextPlaceObjectUseCase nextPlaceObjectUseCase,
            PlaceUseCase placeUseCase,
            PackingBoxPerformer packingBoxPerformer,
            InGameUIView inGameUIView,
            PlaceObjectHolderView placeObjectHolderView,
            PlaceBoardRecordUseCase placeBoardRecordUseCase,
            SceneLoader sceneLoader,
            IChapterTitlePerformer chapterTitlePerformer,
            ResultCalculator resultCalculator,
            SceneDataContainer sceneDataContainer,
            PlaceBoardHistory placeBoardHistory,
            ChapterTitleTextPresenter chapterTitleTextPresenter,
            IGuidancePerformer guidancePerformer,
            InGameUserState inGameUserState)
        {
            _inGameSetupUseCase = inGameSetupUseCase;
            _nextPlaceObjectUseCase = nextPlaceObjectUseCase;
            _placeUseCase = placeUseCase;
            _packingBoxPerformer = packingBoxPerformer;
            _inGameUIView = inGameUIView;
            _placeObjectHolderView = placeObjectHolderView;
            _placeBoardRecordUseCase = placeBoardRecordUseCase;
            _sceneLoader = sceneLoader;
            _chapterTitlePerformer = chapterTitlePerformer;
            _resultCalculator = resultCalculator;
            _sceneDataContainer = sceneDataContainer;
            _placeBoardHistory = placeBoardHistory;
            _chapterTitleTextPresenter = chapterTitleTextPresenter;
            _guidancePerformer = guidancePerformer;
            _inGameUserState = inGameUserState;
        }

        public void Initialize()
        {
        }

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(2f), cancellationToken: cancellation);

            var stageId = StageId.First;

            for (var i = 0; i < 3; i++)
            {
                await GameStart(stageId, cancellation);

                stageId = stageId.Next();

                // 次への演出
                await UniTask.Delay(TimeSpan.FromSeconds(1f), cancellationToken: cancellation);
            }

            // ゲームクリア
            CalculateResult();
            _sceneLoader.LoadAsync(SceneDefine.Result).Forget();
        }

        private async UniTask GameStart(StageId stageId, CancellationToken cancellation)
        {
            Debug.Log($"[GameStart] stageId: {stageId}");

            await _chapterTitlePerformer.PlayAsync(stageId, cancellation);

            _inGameSetupUseCase.Setup(stageId);

            await _chapterTitleTextPresenter.ShowAsync(stageId, cancellation);

            await _packingBoxPerformer.ShowAsync(cancellation);

            if (!_placeObjectHolderView.IsShow)
            {
                await _placeObjectHolderView.ShowAsync(cancellation);
            }

            _nextPlaceObjectUseCase.SetAll();

            // ガイドを表示
            await _guidancePerformer.ShowAsync(GuidanceView.StateType.NotSelected, cancellation);

            // Packingボタン押下時 or ダンボールがいっぱいになったら
            await UniTask.WhenAny(
                _placeUseCase.OnFullPackingAsObservable().ToUniTask(true, cancellationToken: cancellation),
                _inGameUIView.OnClickPackingButtonAsObservable().ToUniTask(true, cancellationToken: cancellation)
            );

            _inGameUIView.HidePackingButton();

            Debug.Log("Clear");

            // 記録
            _placeBoardRecordUseCase.Record(stageId);

            await UniTask.WhenAny(
                _guidancePerformer.HideAsync(cancellation),
                UniTask.Delay(TimeSpan.FromSeconds(0.3f), cancellationToken: cancellation)
            );

            _inGameUserState.SetState(InGameUserStateType.Packing);

            // ダンボールを閉じる演出
            await _packingBoxPerformer.PackingAsync(cancellation);

            await _chapterTitleTextPresenter.HideAsync(cancellation);

            await UniTask.Delay(TimeSpan.FromSeconds(0.1f), cancellationToken: cancellation);

            // 退場
            await _packingBoxPerformer.HideAsync(cancellation);

            // ホルダーを閉じる
            await _placeObjectHolderView.HideAsync(cancellation);

            _inGameUserState.SetState(InGameUserStateType.Ready);
        }

        private void CalculateResult()
        {
            var resultData = _resultCalculator.Calculate(_placeBoardHistory);
            _sceneDataContainer.Set(new ResultSceneData(resultData.Type));
        }
    }
}