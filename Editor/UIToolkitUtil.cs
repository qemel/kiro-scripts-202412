using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Kiro.Editor
{
    /// <summary>
    ///     UI Toolkitのユーティリティクラス
    /// </summary>
    public static class UIToolkitUtil
    {
        public static VisualTreeAsset GetVisualTree(string name) => Resources.Load<VisualTreeAsset>(name);

        /// <summary>
        ///     Elementのコンストラクタでのボイラープレートを削減するためのメソッド
        /// </summary>
        public static void AddElement(VisualElement element)
        {
            var baseTree = GetVisualTree(element.GetType().Name);
            var baseElement = baseTree.Instantiate();
            element.Add(baseElement);
        }

        /// <summary>
        ///     指定の型のアセットを取得する便利メソッド
        /// </summary>
        /// <param name="searchInFolders"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetAssetByType<T>(string[] searchInFolders = null) where T : Object
        {
            var guids = AssetDatabase.FindAssets($"t:{typeof(T)}", searchInFolders);
            if (guids == null || !guids.Any()) return null;

            var guid = guids.First();
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var component = AssetDatabase.LoadAssetAtPath<T>(path);
            return component;
        }
    }
}