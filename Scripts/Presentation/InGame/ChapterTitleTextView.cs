using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Unity1week202312.InGame
{
    public class ChapterTitleTextView : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private TextMeshProUGUI _chapterText;

        private void Awake()
        {
            _canvasGroup.alpha = 0f;
        }

        public void SetChapterText(string text) => _chapterText.text = text;

        public async UniTask ShowAsync(CancellationToken cancellationToken)
        {
            await _canvasGroup.DOFade(1, 0.2f).WithCancellation(cancellationToken);
        }

        public async UniTask HideAsync(CancellationToken cancellationToken)
        {
            await _canvasGroup.DOFade(0, 0.2f).WithCancellation(cancellationToken);
        }
    }
}