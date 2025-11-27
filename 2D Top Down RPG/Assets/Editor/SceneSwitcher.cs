using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SceneSwitcher : MonoBehaviour
{
    // --- BURAYI KENDÝ SAHNE ÝSÝMLERÝNE GÖRE DÜZENLE ---

    // Menüde "Scenes" -> "Ana Menü" seçeneðini oluþturur
    [MenuItem("Scenes/Ana Menü")]
    public static void OpenMainMenu()
    {
        OpenScene("MainMenu"); // Sahne dosyanýn adý neyse onu yaz
    }

    // Menüde "Scenes" -> "Level 1" seçeneðini oluþturur
    [MenuItem("Scenes/Level 1")]
    public static void OpenLevel1()
    {
        // Görüntüdeki sahne adýna istinaden:
        OpenScene("Scene1");
    }

    // Menüde "Scenes" -> "Level 2" seçeneðini oluþturur
    [MenuItem("Scenes/Level 2")]
    public static void OpenLevel2()
    {
        // Görüntüdeki sahne adýna istinaden:
        OpenScene("Scene2");
    }

    // --- BU KISIM STANDART FONKSÝYON (DOKUNMANA GEREK YOK) ---
    static void OpenScene(string sceneName)
    {
        // Önce mevcut sahneyi kaydetmek ister misin diye sorar (veri kaybýný önler)
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            // Sahne dosyasýný bulur ve açar
            // Not: Sahnelerin "Assets/Scenes/" klasöründe olduðunu varsayar.
            // Farklý yerdeyse yolu tam yazman gerekebilir (örn: "Assets/Maps/Level1.unity")
            string path = "Assets/Scenes/" + sceneName + ".unity";
            EditorSceneManager.OpenScene(path);
        }
    }
}