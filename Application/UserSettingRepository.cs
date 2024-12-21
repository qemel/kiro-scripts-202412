using UnityEngine;

namespace Kiro.Application
{
    /// <summary>
    ///     ユーザー設定を保存するリポジトリ
    /// </summary>
    public sealed class UserSettingRepository
    {
        public float GetBgmVolume() => PlayerPrefs.GetFloat("BgmVolume", 0.3f);

        public void SetBgmVolume(float volume) => PlayerPrefs.SetFloat("BgmVolume", volume);

        public float GetSfxVolume() => PlayerPrefs.GetFloat("SfxVolume", 0.3f);

        public void SetSfxVolume(float volume) => PlayerPrefs.SetFloat("SfxVolume", volume);
    }
}