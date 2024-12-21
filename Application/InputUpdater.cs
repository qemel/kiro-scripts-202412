using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

namespace Kiro.Application
{
    /// <summary>
    ///     Inputを監視してInputStoreに通知する
    /// </summary>
    public sealed class InputUpdater : ITickable
    {
        readonly InputStore _inputStore;
        readonly PlayerInput _playerInput;

        public InputUpdater(InputStore inputStore, PlayerInput playerInput)
        {
            _inputStore = inputStore;
            _playerInput = playerInput;
        }

        public void Tick()
        {
            var position = _playerInput.actions["Position"].ReadValue<Vector2>();
            _inputStore.SetPosition(position);

            if (_playerInput.actions["Select"].triggered) _inputStore.PublishSelect();
            if (_playerInput.actions["Retry"].triggered) _inputStore.PublishRetry();
            if (_playerInput.actions["Undo"].triggered) _inputStore.PublishUndo();
            if (_playerInput.actions["Escape"].triggered) _inputStore.PublishEscape();
        }
    }
}