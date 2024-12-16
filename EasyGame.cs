using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;


namespace EasyMonoGame
{
    /// <summary>
    /// This class represents the game. It is a singleton class.
    /// </summary>
    public class EasyGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private World activeWorld;
        private static EasyGame instance;
        private bool hasLoadedContent = false;
        private bool isPaused = false;
        private Random random;
        

        private EasyGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            IsMouseVisible = true;
            instance = this;
            random = new Random();
        }
        /// <summary>
        /// Get the instance of this class.
        /// </summary>
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
        
        internal bool HasLoadedContent
        {
            get { return hasLoadedContent; }
        }
        /// <summary>
        /// Pause the game, or restart the game.
        /// 
        /// Press P to unpause the game.
        /// </summary>
        public bool IsPaused
        {
            get { return isPaused; }
            set { isPaused = value; }
        }
        /// <summary>
        /// Get or set the active world.
        /// </summary>
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
        /// <summary>
        /// Get the random number generator for this game.
        /// </summary>
        public Random Random
        {
            get { return random; }
        }
        /// <summary>
        /// Used by MonoGame to initialize the game.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
            
        }
        /// <summary>
        /// Used by MonoGame to load content.
        /// </summary>
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
        /// <summary>
        /// Used by MonoGame to update the game each frame.
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.P))
            {
                isPaused = false;
            }
            if (isPaused)
            {
                return;
            }

            // TODO: Add your update logic here
            if (activeWorld != null)
            {
                activeWorld.Update(gameTime);
            }
                

            base.Update(gameTime);
        }
        /// <summary>
        /// Used by MonoGame to draw the game each frame.
        /// </summary>
        /// <param name="gameTime"></param>
        protected override void Draw(GameTime gameTime)
        {
            if (activeWorld == null)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);
            }
            else
            {
                GraphicsDevice.Clear(activeWorld.BackgroundColor);
            }

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            if (activeWorld != null)
                activeWorld.Draw(gameTime, spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
