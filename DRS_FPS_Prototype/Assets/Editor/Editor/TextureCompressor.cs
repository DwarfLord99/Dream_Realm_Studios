using UnityEditor;
using UnityEngine;

public class BulkTextureCompressor : EditorWindow
{
    [MenuItem("Tools/Compress All Textures")]  //creates the menu item like in Moye's lectures
    public static void CompressAllTextures()
    {
        string[] texturePaths = AssetDatabase.FindAssets("t:Texture2D");  // populates the array with the paths to all the textures
        foreach (string texturePath in texturePaths)
        {
            string path = AssetDatabase.GUIDToAssetPath(texturePath);
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter; // each asset is imported based on Unity's importers, you can set these manually in the UI

            if (importer != null)
            {
                importer.textureCompression = TextureImporterCompression.Compressed;
                importer.maxTextureSize = 1024; // Change based on your needs
                importer.SaveAndReimport();
            }
        }
        AssetDatabase.Refresh();
    }
}
