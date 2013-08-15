using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Pong
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Arena : DrawableGameComponent
    {
        private Vector2 _position;
        private Texture2D _background;
        private Texture2D _wall;
        private Vector2 _topWallPosition;
        private Vector2 _bottomWallPosition;
        private SpriteFont _spriteFont;
        private Vector2 _playerOneScorePos;
        private Vector2 _playerTwoScorePos;
        private int _playerOneScore;
        private int _playerTwoScore;

        public Arena(Game game)
            : base(game)
        {
            Game.Components.Add(this);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            _position = new Vector2(0, 0);
            _topWallPosition = new Vector2(-10, 0);
            _bottomWallPosition = new Vector2(-10, 780);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _background = Game.Content.Load<Texture2D>(@"Images\background");
            _wall = Game.Content.Load<Texture2D>(@"Images\wall");
            _spriteFont = Game.Content.Load<SpriteFont>(@"Fonts\Quartz MS");
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            _playerOneScore = ((PongGame)Game).Scores[0];
            _playerTwoScore = ((PongGame)Game).Scores[1];

            Vector2 fontVector = _spriteFont.MeasureString(_playerOneScore.ToString());
            _playerOneScorePos = new Vector2(Game.Window.ClientBounds.Width / 2 - 42 - fontVector.X, 40);
            _playerTwoScorePos = new Vector2(Game.Window.ClientBounds.Width / 2 + 40, 40);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sb = ((PongGame)Game).SpriteBatch;
            sb.Draw(_background, _position, null, Color.White);
            sb.Draw(_wall, _topWallPosition, null, Color.White);
            sb.Draw(_wall, _bottomWallPosition, null, Color.White);

            sb.DrawString(_spriteFont, _playerOneScore.ToString(), _playerOneScorePos, Color.White);
            sb.DrawString(_spriteFont, _playerTwoScore.ToString(), _playerTwoScorePos, Color.White);

            base.Draw(gameTime);
        }

        public Rectangle GetPlayArea()
        {
            return new Rectangle(0, _wall.Height, Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height - _wall.Height);
        }
    }
}