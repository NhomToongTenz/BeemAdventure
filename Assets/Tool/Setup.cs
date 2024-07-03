
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DuAn1.Tools
{
    public static class Setup
    {
        [MenuItem("Tools/Setup/Create Default Folders")]
        public static void CreateDefaultFolder()
        {
            Folders.CreateDefault("_Project", "Animation", "Art", "Audio", "Materials", "Models", "Prefabs", "Scenes", "Scripts", "Shaders", "Sprites", "Textures");
            AssetDatabase.Refresh();
        }

       static class Folders
       {
           public static void CreateDefault(string root, params string[] folders)
           {
               var fullPath = Path.Combine(Application.dataPath, root);

                foreach (var folder in folders)
                {
                     var path = Path.Combine(fullPath, folder);
                     if (!Directory.Exists(path))
                     {
                          Directory.CreateDirectory(path);
                     }
                }
           }
       }
    }
}
