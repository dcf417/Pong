using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pong.Managers;

namespace Pong
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class PongGame : Game
    {
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        ScoreKeeper _scoreKeeper;
        Paddle[] _paddles;
        Ball _ball;
        Arena _arena;

        public PongGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferHeight = 800;
            _graphics.PreferredBackBufferWidth = 1200;
            Content.RootDirectory = "Content";

            Services.AddService(typeof(IInputManager), new InputManager(this));
        }

        public SpriteBatch SpriteBatch
        {
            get { return _spriteBatch; }
        }

        public Paddle[] Paddles
        {
            get { return _paddles; }
        }

        public Arena Arena
        {
            get { return _arena; }
        }

        public ScoreKeeper ScoreKeeper
        {
            get { return _scoreKeeper; }
        }

        public bool GameInProgress
        {
            get { return _scoreKeeper.KeepPlaying; }
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _scoreKeeper = new ScoreKeeper(this);

            _arena = new Arena(this);

            _paddles = new Paddle[2];
            _paddles[0] = new Paddle(this, new Vector2(20, Window.ClientBounds.Height / 2), Player.One);
            _paddles[1] = new Paddle(this, new Vector2(Window.ClientBounds.Width - 40, Window.ClientBounds.Height / 2), Player.Two);

            _ball = new Ball(this);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            base.Draw(gameTime);
            _spriteBatch.End();
        }
    }
}
