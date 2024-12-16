using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace EasyMonoGame
{
    /// <summary>
    /// Class used to load game art.
    /// </summary>
    public class GameArt
    {
        private static Dictionary<string, Texture2D> images  = new Dictionary<string, Texture2D>();
        private static ContentManager content;
        private static HashSet<string> fileNames = new HashSet<string>(); 
        private static SpriteFont font;

        /// <summary>
        /// Set contentmanager. 
        /// </summary>
        /// <param name="content"></param>
        internal static void SetContentManager(ContentManager content)
        {
            GameArt.content = content;
            
        }
        /// <summary>
        /// Use added file names to load Texture2D objects.
        /// </summary>
        /// <exception cref="Exception"></exception>
        internal static void Load()
        {
            if (content == null)
            {
                throw new Exception("ContentManager not set to a value. Call SetContentManager.");
            }
            foreach (string name in fileNames)
            {
                images[name] = content.Load<Texture2D>(name);
            }
            fileNames.Clear();
        }
        /// <summary>
        /// Add an image file. Use the file name.
        /// </summary>
        /// <param name="name"></param>
        public static void Add(string name)
        {

            //images[name] = content.Load<Texture2D>(name);
            fileNames.Add(name);
        }
        /// <summary>
        /// Add several images. Use filenames stored in an array.
        /// </summary>
        /// <param name="names"></param>
        public static void Add(string[] names)
        {
            foreach (string name in names)
            {
                Add(name);
            }

        }
        /// <summary>
        /// Check if an image is loaded.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool Contains(string name)
        {
            return images.ContainsKey(name);
        }
        /// <summary>
        /// Get an image.
        /// </summary>
        /// <param name="name">The image name.</param>
        /// <returns></returns>
        public static Texture2D Get(string name) 
        { 
            return images[name]; 
        }
        internal static SpriteFont GetFont()
        {
            return font;
        }
        internal static void SetFont(SpriteFont font)
        {
            GameArt.font = font;
        }
    }
}
