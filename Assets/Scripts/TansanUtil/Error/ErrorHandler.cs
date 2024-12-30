// using NCMB;
// using UnityEngine;

// /// <summary>
// /// ゲーム中のエラーをハンドリングするクラス
// /// </summary>
// namespace TansanMilMil.Util
// {
//     public class ErrorLogModel
//     {
//         public string message { get; set; }
//         public string innerException { get; set; }
//         public string stackTrace { get; set; }
//     };

//     [DefaultExecutionOrder(-200)]
//     public class ErrorHandler : MonoBehaviour
//     {
//         public enum ERROR_LEVEL
//         {
//             NOTICE = 0,
//             WARNING = 1,
//             DANGER = 2,
//         }

//         /// <summary>NCMB系エラーはこのメソッドへ投げる</summary>
//         public static void ThrowNCMBError(NCMBException e, string stackTrace = null, ERROR_LEVEL errorLevel = ERROR_LEVEL.DANGER)
//         {
//             Debug.LogError(e);
//             if (!IsException(e))
//             {
//                 return;
//             }
//         }

//         private static bool IsException(NCMBException e)
//         {
//             string errorCode = e.ErrorCode;
//             switch (errorCode)
//             {
//                 case "E429001":
//                     return true;
//                 case "E502001":
//                     return true;
//                 case "E502003":
//                     return true;
//                 default:
//                     return true;
//             }
//         }

//         private static string GetNCMBExceptionText(NCMBException e)
//         {
//             string text = "";
//             string errorCode = e.ErrorCode;
//             switch (errorCode)
//             {
//                 case "E429001":
//                     text = "サーバ容量超過の為、通信できませんでした。";
//                     break;
//                 case "E502001":
//                     text = "サーバでエラーが発生した為、通信できませんでした。";
//                     break;
//                 case "E502003":
//                     text = "サーバでエラーが発生した為、通信できませんでした。";
//                     break;
//                 default:
//                     text = "通信中に予期せぬエラーが発生しました。";
//                     break;
//             }
//             return text;
//         }

//         private static string GetPlayerPrefsExceptionText(PlayerPrefsException e)
//         {
//             string text = "";
//             text = "セーブデータの読込または書込に失敗しました。";
//             return text;
//         }
//     }
// }
