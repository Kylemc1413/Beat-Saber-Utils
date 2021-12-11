using System;
using System.Linq;
using UnityEngine;
using System.Reflection;
using System.IO;

namespace BS_Utils.Utilities
{
    // TODO: Mark class as internal at some point, and remove unused methods (only a few are used internally by the GetUserInfo class)
    [Obsolete("The UIUtilities class will be marked as internal in a future update. Please use BSML's Utilities class instead.")]
    public static class UIUtilities
    {
        public static Texture2D LoadTextureRaw(byte[] file)
        {
            if (file.Any())
            {
                Texture2D tex2D = new Texture2D(2, 2);
                if (tex2D.LoadImage(file))
                    return tex2D;
            }
            return null;
        }

        public static Texture2D LoadTextureFromFile(string filePath)
        {
            if (File.Exists(filePath))
                return LoadTextureRaw(File.ReadAllBytes(filePath));

            return null;
        }

        public static Texture2D LoadTextureFromResources(string resourcePath)
        {
            return LoadTextureRaw(GetResource(Assembly.GetCallingAssembly(), resourcePath));
        }

        public static Sprite LoadSpriteRaw(byte[] image, float pixelsPerUnit = 100.0f)
        {
            return LoadSpriteFromTexture(LoadTextureRaw(image), pixelsPerUnit);
        }

        public static Sprite LoadSpriteFromTexture(Texture2D spriteTexture, float pixelsPerUnit = 100.0f)
        {
            if (spriteTexture)
                return Sprite.Create(spriteTexture, new Rect(0, 0, spriteTexture.width, spriteTexture.height), new Vector2(0, 0), pixelsPerUnit);
            return null;
        }

        public static Sprite LoadSpriteFromFile(string filePath, float pixelsPerUnit = 100.0f)
        {
            return LoadSpriteFromTexture(LoadTextureFromFile(filePath), pixelsPerUnit);
        }

        public static Sprite LoadSpriteFromResources(string resourcePath, float pixelsPerUnit = 100.0f)
        {
            return LoadSpriteRaw(GetResource(Assembly.GetCallingAssembly(), resourcePath), pixelsPerUnit);
        }

        public static byte[] GetResource(Assembly asm, string resourceName)
        {
            using Stream stream = asm.GetManifestResourceStream(resourceName);
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, (int)stream.Length);
            return data;
        }

        public static void PrintHierarchy(Transform transform, string spacing = "|-> ")
        {
            spacing = spacing.Insert(1, "  ");
            var tempList = transform.Cast<Transform>().ToList();
            foreach (var child in tempList)
            {
                Console.WriteLine($"{spacing}{child.name}");
                PrintHierarchy(child, "|" + spacing);
            }
        }
    }
}
