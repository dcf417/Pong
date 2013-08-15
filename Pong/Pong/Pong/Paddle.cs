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
    public class Paddle : DrawableGameComponent
    {
        private Texture2D _texture;
        private Vector2 _position;
        private int _width;
        private int _height;
        private float _speed;
        private Vector2 _center;
        private float _minY;
        private float _maxY;
        private Keys _upKey;
        private Keys _downKey;
        private Color[] _textureData;

        public Paddle(Game game, Vector2 position, Keys upKey, Keys downKey)
            : base(game)
        {
            _position = position;
            _upKey = upKey;
            _downKey = downKey;

            Game.Components.Add(this);
        }

        public Color[] TextureData
        {
            get
            {
                return _textureData;
            }
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            _speed = 0.5f;  

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _texture = Game.Content.Load<Texture2D>(@"Images\paddle");
            _width = _texture.Width;
            _height = _texture.Height;
            _center = new Vector2(_width / 2, _height / 2);

            _textureData = new Color[_width * _height];
            _texture.GetData(_textureData);

            SetBoundaries();

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            float timeLapse = (float)gameTime.ElapsedGameTime.Milliseconds;

            if (Keyboard.GetState().IsKeyDown(_upKey))
            {
                _position.Y -= _speed * timeLapse;
            }
            else if (Keyboard.GetState().IsKeyDown(_downKey))
            {
                _position.Y += _speed * timeLapse;
            }

            _position.Y = MathHelper.Clamp(_position.Y, _minY, _maxY);
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sb = ((PongGame)Game).SpriteBatch;
            sb.Draw(_texture, _position, null, Color.White, 0.0f, _center, 1.0f, SpriteEffects.None, 0.0f);

            base.Draw(gameTime);
        }

        public Rectangle GetBoundingRectangle()
        {
            return new Rectangle((int)_position.X, (int)_position.Y, _width, _height);
        }

        private Rectangle TitleSafeRegion(int spriteWidth, int spriteHeight)
        {
            // show entire region on the PC or Zune
            return new Rectangle(0, 0, Game.Window.ClientBounds.Width,
                                       Game.Window.ClientBounds.Height - spriteHeight - 5);
        }

        private void SetBoundaries()
        {
            Rectangle playArea = ((PongGame)Game).Arena.GetPlayArea();
            _minY = _height / 2 + playArea.Y + 5;
            _maxY = playArea.Height - (_height / 2) - 5;
        }
    }
}
