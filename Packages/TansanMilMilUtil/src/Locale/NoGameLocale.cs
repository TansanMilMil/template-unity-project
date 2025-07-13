using System;
using Cysharp.Threading.Tasks;
using TansanMilMil.Util;
using R3;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace TansanMilMil.Util
{
    [DefaultExecutionOrder(-10)]
    public class NoGameLocale : GameLocaleBase
    {
        protected override UniTask InitializeAvailableLocalesAsync()
        {
            return UniTask.CompletedTask;
        }

        protected override void LoadLocaleFromConfigSaveData()
        {
            return;
        }

        protected override void SetLocaleFromConfigSaveData()
        {
            return;
        }

        protected override void SetLocaleToConfig()
        {
            return;
        }

        protected override Locale GetLocale(string cultureInfoName)
        {
            return null;
        }

        protected override Locale GetCurrentLocale()
        {
            return null;
        }

        protected override void SetLocale(Locale locale)
        {
            return;
        }

        public override void SetLocale(string cultureInfoName)
        {
            return;
        }

        /// <summary>
        /// 改行コードを画面上に反映させたい場合は<see cref="GetEntryValueReplacedAsync"/>を使用してください。
        /// </summary>
        public override async UniTask<string> GetEntryValueAsync(string entryName, string tableReference)
        {
            if (string.IsNullOrWhiteSpace(entryName)) return "";

            return await UniTask.FromResult(entryName);
        }
    }
}