using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Shiroi.FX.Effects.BuiltIn;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Shiroi.FX.Editor {
    public static class ShiroiFXEditor {
        [MenuItem(ShiroiFXEditorResources.BaseCreatePath + "Composite Effect", false, 5)]
        public static void CreateCompositeEffect() {
            CreateEffect<CompositeEffect>();
        }

        [MenuItem(ShiroiFXEditorResources.BaseCreatePath + "Particle Effect", false, 5)]
        public static void CreateParticleEffect() {
            CreateEffect<ParticleEffect>();
        }

        [MenuItem(ShiroiFXEditorResources.BaseCreatePath + "Freeze Frame Effect", false, 5)]
        public static void CreateFreezeFrameEffect() {
            CreateEffect<FreezeFrameEffect>();
        }

        [MenuItem(ShiroiFXEditorResources.BaseCreatePath + "Object Shake Effect", false, 5)]
        public static void CreateObjectShakeEffect() {
            CreateEffect<ObjectShakeEffect>();
        }


        [MenuItem(ShiroiFXEditorResources.BaseCreatePath + "Audio Effect", false, 5)]
        public static void CreateAudioEffect() {
            CreateEffect<AudioEffect>();
        }

        [MenuItem(ShiroiFXEditorResources.BaseCreatePath + "Field Of View Effect", false, 5)]
        public static void CreateFOVEffect() {
            CreateEffect<FOVEffect>();
        }

        private static void CreateEffect<T>() where T : ScriptableObject {
            var effect = ScriptableObject.CreateInstance<T>();
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (path == "") {
                path = "Assets";
            } else if (Path.GetExtension(path) != "") {
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }

            var assetPathAndName =
                AssetDatabase.GenerateUniqueAssetPath(string.Format("{0}/{1}.asset", path, typeof(T).Name));
            AssetDatabase.CreateAsset(effect, assetPathAndName);

            AssetDatabase.SaveAssets();
        }

        /// <summary>
        /// Read all bytes in this stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>All bytes in the stream.</returns>
        public static byte[] ReadAllBytes(this Stream stream) {
            long originalPosition = 0;

            if (stream.CanSeek) {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try {
                var readBuffer = new byte[4096];

                var totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0) {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead != readBuffer.Length) {
                        continue;
                    }

                    var nextByte = stream.ReadByte();
                    if (nextByte == -1) {
                        continue;
                    }

                    var temp = new byte[readBuffer.Length * 2];
                    Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                    Buffer.SetByte(temp, totalBytesRead, (byte) nextByte);
                    readBuffer = temp;
                    totalBytesRead++;
                }

                var buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead) {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }

                return buffer;
            } finally {
                if (stream.CanSeek) {
                    stream.Position = originalPosition;
                }
            }
        }

        private static readonly Dictionary<string, Texture2D> _embeddedIcons = new Dictionary<string, Texture2D>();

        /// <summary>
        /// Get the embedded icon with the given resource name.
        /// </summary>
        /// <param name="resourceName">The resource name.</param>
        /// <returns>The embedded icon with the given resource name.</returns>
        public static Texture2D GetEmbeddedIcon(string resourceName) {
            var assembly = Assembly.GetExecutingAssembly();

            Texture2D icon;
            if (!_embeddedIcons.TryGetValue(resourceName, out icon) || icon == null) {
                byte[] iconBytes;
                using (var stream = assembly.GetManifestResourceStream(resourceName))
                    iconBytes = stream.ReadAllBytes();
                icon = new Texture2D(128, 128);
                icon.LoadImage(iconBytes);
                icon.name = resourceName;

                _embeddedIcons[resourceName] = icon;
            }

            return icon;
        }

        /// <summary>
        /// Set the icon for this object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="texture">The icon.</param>
        public static void SetIcon(this Object obj, Texture2D texture) {
            var ty = typeof(EditorGUIUtility);
            var mi = ty.GetMethod("SetIconForObject", BindingFlags.NonPublic | BindingFlags.Static);
            mi.Invoke(null, new object[] {obj, texture});
        }

        /// <summary>
        /// Set the icon for this object from an embedded resource.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="texture">The icon.</param>
        public static void SetIcon(this Object obj, string resourceName) {
            SetIcon(obj, GetEmbeddedIcon(resourceName));
        }

        /// <summary>
        /// Get the icon for this object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The icon for this object.</returns>
        public static Texture2D GetIcon(this Object obj) {
            var ty = typeof(EditorGUIUtility);
            var mi = ty.GetMethod("GetIconForObject", BindingFlags.NonPublic | BindingFlags.Static);
            return mi.Invoke(null, new object[] {obj}) as Texture2D;
        }

        /// <summary>
        /// Remove this icon's object.
        /// </summary>
        /// <param name="obj">The object.</param>
        public static void RemoveIcon(this Object obj) {
            SetIcon(obj, (Texture2D) null);
        }
    }
}