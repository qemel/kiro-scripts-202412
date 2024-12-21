using System.Runtime.CompilerServices;
using UnityEngine;

namespace Kiro.Application
{
    public class GameLog
    {
        int _frame = -1;
        string _namespace;
        string _className;
        string _methodName;
        string _message;

        /// <summary>
        ///     初期値はちょっと白
        /// </summary>
        Color _color = new(0.9f, 0.9f, 0.9f);

        public static GameLog Create() => new();

        public static void Execute(
            string message, object classType, Color color = default, [CallerMemberName] string methodName = ""
        ) =>
            new GameLog()
                .WithMessage(message)
                .WithFrame()
                .WithLocation(classType, methodName)
                .WithColor(color == default ? new Color(0.9f, 0.9f, 0.9f) : color)
                .Log();

        public static void ExecuteWarning(
            string message, object classType, Color color = default, [CallerMemberName] string methodName = ""
        ) =>
            new GameLog()
                .WithMessage(message)
                .WithFrame()
                .WithLocation(classType, methodName)
                .WithColor(color == default ? new Color(0.9f, 0.9f, 0.9f) : color)
                .LogWarning();

        public GameLog WithMessage(string message)
        {
            _message = message;
            return this;
        }

        public GameLog WithFrame()
        {
            _frame = Time.frameCount;
            return this;
        }

        public GameLog WithLocation(object classType, [CallerMemberName] string methodName = "")
        {
            _namespace = classType.GetType().Namespace;
            _className = classType.GetType().Name;
            _methodName = string.IsNullOrEmpty(methodName) ? null : methodName;
            return this;
        }

        public GameLog WithColor(Color color)
        {
            _color = color;
            return this;
        }

        public void Log()
        {
#if UNITY_EDITOR
            Debug.Log(ToString());
#endif
        }

        public void LogWarning()
        {
#if UNITY_EDITOR
            Debug.LogWarning(ToString());
#endif
        }

        public void LogError()
        {
#if UNITY_EDITOR
            Debug.LogError(ToString());
#endif
        }

        public override string ToString()
        {
            // ないならその部分は表示しない
            var namespaceStr = string.IsNullOrEmpty(_namespace) ? "" : $"{_namespace}.";
            var classNameStr = string.IsNullOrEmpty(_className) ? "" : $"{_className}.";
            var methodNameStr = string.IsNullOrEmpty(_methodName) ? "" : $"{_methodName}";
            var frameStr = _frame == -1 ? "" : $"[Frame: {_frame}] ";

            if (string.IsNullOrEmpty(namespaceStr) && string.IsNullOrEmpty(classNameStr) &&
                string.IsNullOrEmpty(methodNameStr))
                return $"{frameStr}{_message}";

            return
                $"{frameStr}<{namespaceStr}{classNameStr}{methodNameStr}> <color=#{ColorUtility.ToHtmlStringRGB(_color)}>{_message}</color>";
        }
    }
}