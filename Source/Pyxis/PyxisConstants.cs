namespace Pyxis
{
    public static class PyxisConstants
    {
        public static string DummyImage => "https://placehold.jp/1x1.png";

        public static string DummyIcon => "http://placehold.jp/24/cccccc/999999/48x48.png?text=A";

        public static string ApplicationKey => "8B2D7393-617C-4836-AA0E-68B502D9B1C9";

        public static string DownloadFoilderName => "Pyxis";

        public static class ConfigurationKeys
        {
            #region Search

            /// <summary>
            ///     検索モード (Partial or Exact)
            /// </summary>
            public static string DefaultSearchMode => "Configuration.Search.DefaultMode";

            #endregion

            #region Cache
            // TODO: This section is reserved for feature.

            /// <summary>
            ///     キャッシュ最大サイズ
            /// </summary>
            public static string MaximumCacheSize => "Configuration.Cache.MaxSize";

            /// <summary>
            ///     キャッシュ自動削除ルール
            /// </summary>
            public static string AutomaticClearRule => "Configuration.Cache.ClrarRule";

            #endregion

            #region Appearance

            /// <summary>
            ///     Related Items の表示 (for zh-CN)
            /// </summary>
            public static string IsShowRelatedItems => "Configuration.Appearance.RelatedItems";

            /// <summary>
            ///     テーマ (Dark, Light or Windows)
            /// </summary>
            public static string Theme => "Configuration.Appearance.Theme";

            #endregion
        }
    }
}