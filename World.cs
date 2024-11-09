using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;


namespace EasyMonoGame
{
    public class World
    {
        private Texture2D backgroundTile;
        private SortedSet<Type> updateOrder; // first is  updated first
        private SortedSet<Type> drawOrder; // first is drawn on top, that is drawn last
        internal Dictionary<Type, List<Actor>> actors; //TODO make private
        private Dictionary<Vector2, Text> texts;
        private bool isBounded = true;
        private int width;
        private int height;
        private string backgroundTileName;
        /// <summary>
        /// Inherit this class to create a world for your game.
        /// </summary>
        /// <param name="width">Width of world i pixels.</param>
        /// <param name="height">Height of world i pixels.</param>
        public World(int width, int height)
        {
            //graphics = new GraphicsDeviceManager(this);
            this.width =  width;
            this.height = height;
            
            //Content.RootDirectory = "Content";
            //IsMouseVisible = true;
            actors = new Dictionary<Type, List<Actor>>();
            texts = new Dictionary<Vector2, Text>();
        }
        /// <summary>
        /// Set or get the background image for this world. 
        /// If the image is smaller than the world the background wíll be tiled.
        /// </summary>
        public string BackgroundTileName
        {
            get { return backgroundTileName; }
            set { backgroundTileName = value; }
        }
            
        /// <summary>
        /// Returns the height of the world in pixels.
        /// </summary>
        public int Height
        {
            get
            {
                return height;
            }
        }
        /// <summary>
        /// Returns the width of the world in pixels.
        /// </summary>
        public int Width
        {
            get 
            { 
                return width; 
            }
        }


        /// <summary>
        /// Add an actor to this world.
        /// </summary>
        /// <param name="actor"></param>
        public void Add(Actor actor, string imageName, float x, float y)
        {
            actor.ImageName = imageName;
            actor.Position = new Vector2(x, y);
            // LoadContent in EasyGame has been called and actor.Image is null. => Set image from GameArt.
            if (EasyGame.Instance.HasLoadedContent && actor.Image == null)
            {
                actor.Image = GameArt.Get(actor.ImageName);
            }
            // if not has type => add type to dictionary and create list
            if (actors.TryGetValue(actor.GetType(), out List<Actor> actorsOfType))
            {
                actorsOfType.Add(actor);
                actor.World = this;
            }
            else
            {
                List<Actor> actorsOfNewType = new List<Actor>();
                actorsOfNewType.Add(actor);
                actors.Add(actor.GetType(), actorsOfNewType);
                actor.World = this;
            }

        }
        /// <summary>
        /// Show text at the specified coordinate. 
        /// 
        /// A new text at the same coordinate will replace the other text.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>


        public void ShowText(string text, int x, int y)
        {
            if (GameArt.GetFont() != null)
            { 
                Text textObject = new Text(
                    GameArt.GetFont(),
                    text,
                    new Vector2(x, y),
                    Color.White);
                texts[new Vector2(x, y)] = textObject;
            }
        }
        /// </summary>
        /// 
        /// <summary>
        /// <param name="gameTime"></param>
        /// 

        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            TileBackground(spriteBatch);

            foreach (var pair in actors)
            {
                foreach (var actor in pair.Value)
                {
                    actor.Draw(spriteBatch);
                }

            }
            foreach (var pair in texts)
            {
                pair.Value.Draw(spriteBatch);
            }
            


        }
        internal void LoadContent()
        {
            foreach (var pair in actors)
            {
                foreach (var actor in pair.Value)
                {
                    actor.Image = GameArt.Get(actor.ImageName);
                }

            }
        }




        public virtual void Update(GameTime gameTime)
        {
            foreach (var pair in actors)
            {
                foreach (var actor in pair.Value)
                {
                    actor.Update(gameTime);
                    // Keep actor inside of screen
                    if (isBounded)
                    {
                        // x-direciton
                        if (actor.Position.X < 0)
                        {
                            actor.SetX(0);
                        }
                        else if (this.Width < actor.Position.X)
                        {
                            actor.SetX(this.Width);
                        }
                        // y-direction
                        if (actor.Position.Y < 0)
                        {
                            actor.SetY(0);
                        }
                        else if (this.Height < actor.Position.Y)
                        {
                            actor.SetY(this.Height);
                        }
                    }
                }

            }


        }
        


        private void TileBackground(SpriteBatch spriteBatch)
        {
            if (backgroundTileName == null)
            {
                return;
            }
            if (backgroundTile == null)
            {
                backgroundTile = GameArt.Get(backgroundTileName);
            }
            for (int x = 0; x < Width; x += backgroundTile.Width)
            {
                for (int y = 0; y < Height; y += backgroundTile.Height)
                {
                    spriteBatch.Draw(backgroundTile, new Vector2(x, y), Color.White);
                }
            }
        }

        public void RemoveActor(Actor actor)
        {
            List<Actor> actorsOfType = actors[actor.GetType()];
            if (actorsOfType != null)
            {
                actorsOfType.Remove(actor);
            }
        }

    }
}
