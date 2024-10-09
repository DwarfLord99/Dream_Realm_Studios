using UnityEditor;
using UnityEngine;

public class BulkTextureCompressor : EditorWindow
{
    [MenuItem("Tools/Compress All Textures")]
    public static void CompressAllTextures()
    {
        string[] texturePaths = AssetDatabase.FindAssets("t:Texture2D");
        foreach (string texturePath in texturePaths)
        {
            string path = AssetDatabase.GUIDToAssetPath(texturePath);
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

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
