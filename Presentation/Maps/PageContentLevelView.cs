using Kiro.Application;
using Kiro.Domain;
using R3;
using UnityEngine;

namespace Kiro.Presentation
{
    /// <summary>
    ///     ページ内に配置する、ステージ選択ボタンのView
    /// </summary>
    [RequireComponent(typeof(SpriteButtonView))]
    public sealed class PageContentLevelView : MonoBehaviour
    {
        [SerializeField] int _world;
        [SerializeField] int _level;

        SpriteButtonView _spriteButtonView;

        public Observable<StageId> OnClickAsObservable =>
            _spriteButtonView.OnClickAsObservable.Select(
                _ => new StageId(new WorldId(_world), new LevelInWorldId(_level))
            );

        void Awake()
        {
            _spriteButtonView = GetComponent<SpriteButtonView>();
        }

        void Start()
        {
            // Log
            OnClickAsObservable.Subscribe(stageId => GameLog.Execute(stageId.ToString(), this)).AddTo(this);
        }
    }
}