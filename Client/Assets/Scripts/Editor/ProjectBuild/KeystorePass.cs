
using UnityEditor;

[InitializeOnLoad]
public class KeystorePass
{
    static KeystorePass()
    {
        PlayerSettings.Android.keystorePass = "ps12369";
        PlayerSettings.Android.keyaliasName = "jdqw";
        PlayerSettings.Android.keyaliasPass = "ps12369";
    }
}