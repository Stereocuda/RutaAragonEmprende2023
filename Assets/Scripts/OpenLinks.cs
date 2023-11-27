using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class OpenLinks : MonoBehaviour
{
    public static string DatosURL= "http://rutaaragonemprende.com/politica-datos.pdf";
    public static string BasesURL= "http://rutaaragonemprende.com/bases-legales-sorteo.pdf";

    public static void OpenURL(string url)
    {
        #if !UNITY_EDITOR && UNITY_WEBGL
        OpenTab(url);
        #endif
    }

    public static void OpenDatos()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        OpenTab(DatosURL);
#endif
    }

    public static void OpenBases()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        OpenTab(BasesURL);
#endif
    }




    [DllImport("__Internal")]
    private static extern void OpenTab(string url);
}
