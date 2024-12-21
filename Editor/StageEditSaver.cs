using Kiro.Data.EditMode;
using UnityEditor;

namespace Kiro.Editor
{
    public static class StageEditSaver
    {
        public static void Execute()
        {
            var stageDataHolder = UIToolkitUtil.GetAssetByType<AllWorldsDataSO>();
            EditorUtility.SetDirty(stageDataHolder);
            AssetDatabase.SaveAssets();
        }
    }
}