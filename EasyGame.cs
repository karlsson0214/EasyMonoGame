﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace EasyMonoGame
{
    public class EasyGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private World activeWorld;
        private static EasyGame instance;
        private bool hasLoadedContent = false;

        private EasyGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsMouseVisible = true;
            instance = this;
            
        }

        public static EasyGame Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EasyGame();
                }
                return instance;
            }
        }
        public bool HasLoadedContent
        {
            get { return hasLoadedContent; }
        }
        public World ActiveWorld
        {
            get 
            { 
                return activeWorld; 
            }
            set 
            { 
                activeWorld = value;
                graphics.PreferredBackBufferWidth = activeWorld.Width;
                graphics.PreferredBackBufferHeight = activeWorld.Height;
                graphics.ApplyChanges();

                if (hasLoadedContent)
                {
                    // Methodes below are called in method LoadContent,
                    // the first time the game is loaded.
                    GameArt.Load(); // use file names to load Texture2D
                    activeWorld.LoadContent();
                }
                    
            }
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load content for EasyGame.
            GameArt.SetContentManager(Content);
            GameArt.Load();
            activeWorld.LoadContent();
            // TODO load font from proper location.
            SpriteFont font = Content.Load<SpriteFont>("Arial32");

            //https://docs.monogame.net/articles/getting_to_know/howto/content_pipeline/HowTo_LoadContentLibrary.html
            /*
            ResourceContentManager resxContent;
            resxContent = new ResourceContentManager(Services, ResourceFont.ResourceManager);
            SpriteFont font = resxContent.Load<SpriteFont>("all"); // ERROR resource is not binary
            */
            GameArt.SetFont(font);
            hasLoadedContent = true;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            // TODO: Add your update logic here
            if (activeWorld != null)
            {
                activeWorld.Update(gameTime);
            }
                

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            if (activeWorld != null)
                activeWorld.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
