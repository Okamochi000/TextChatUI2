using UnityEngine;

namespace UniSoftwareKeyboardArea
{
    /// <summary>
    /// ソフトウェアキーボードの表示領域を管理するクラス
    /// </summary>
    public static class SoftwareKeyboardArea
    {
        /// <summary>
        /// 高さを返します
        /// </summary>
        public static int GetHeight()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            //if (!TouchScreenKeyboard.visible) { return 0; }
            using (var unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                var currentActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
                var unityPlayer = currentActivity.Get<AndroidJavaObject>("mUnityPlayer");
                var view = unityPlayer.Call<AndroidJavaObject>("getView");
                
                if (view == null) return 0;

                int result = 0;

                using (var rect = new AndroidJavaObject("android.graphics.Rect"))
                {
                    view.Call("getWindowVisibleDisplayFrame", rect);
                    result = Screen.height - rect.Call<int>("height");
                }

                if (TouchScreenKeyboard.hideInput) return result;

                var softInputDialog = unityPlayer.Get<AndroidJavaObject>("mSoftInputDialog");
                var window = softInputDialog?.Call<AndroidJavaObject>("getWindow");
                var decorView = window?.Call<AndroidJavaObject>("getDecorView");

                if (decorView == null) return result;

                var decorHeight = decorView.Call<int>("getHeight");
                result += decorHeight;

                return result;
            }
#else
            var area = TouchScreenKeyboard.area;
            var height = Mathf.RoundToInt(area.height);
            return Screen.height <= height ? 0 : height;
#endif
        }

        /// <summary>
        /// 横向きにした時のホームエリアサイズを取得する
        /// </summary>
        public static int GetLandscapeHomeWidth()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            if (!TouchScreenKeyboard.visible) { return 0; }
            using (var unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                var currentActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
                var unityPlayer = currentActivity.Get<AndroidJavaObject>("mUnityPlayer");
                var view = unityPlayer.Call<AndroidJavaObject>("getView");
                
                if (view == null) return 0;

                int result = 0;

                using (var rect = new AndroidJavaObject("android.graphics.Rect"))
                {
                    view.Call("getWindowVisibleDisplayFrame", rect);
                    result = Screen.width - rect.Call<int>("width");
                }

                return result;
            }
#else
            var area = TouchScreenKeyboard.area;
            var width = Mathf.RoundToInt(area.width);
            return Screen.width <= width ? 0 : width;
#endif
        }
    }
}