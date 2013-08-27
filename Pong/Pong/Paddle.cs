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
using Pong.Managers;


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
        private Vector2 _speed;
        private float _minY;
        private float _maxY;
        private Color[] _textureData;
        private Player _player;
        private IInputManager _inputManager;

        public Paddle(Game game, Vector2 position, Player player)
            : base(game)
        {
            _position = position;
            _player = player;

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
            _speed = new Vector2(0, 0.5f);
            _inputManager = Game.Services.GetService(typeof(IInputManager)) as IInputManager;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _texture = Game.Content.Load<Texture2D>(@"Images\paddle");
            _width = _texture.Width;
            _height = _texture.Height;

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
            if (!((PongGame)Game).GameInProgress)
                return;        

            float timeLapse = (float)gameTime.ElapsedGameTime.Milliseconds;
            MoveState move = _inputManager.GetPlayerMove(_player);

            if (move == MoveState.Up)
            {
                _position -= _speed * timeLapse;
            }
            else if (move == MoveState.Down)
            {
                _position += _speed * timeLapse;
            }

            _position.Y = MathHelper.Clamp(_position.Y, _minY, _maxY);
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sb = ((PongGame)Game).SpriteBatch;
            sb.Draw(_texture, _position, Color.White);

            base.Draw(gameTime);
        }

        public Rectangle GetBoundingRectangle()
        {
            return new Rectangle((int)_position.X, (int)_position.Y, _width, _height);
        }

        private void SetBoundaries()
        {
            Rectangle playArea = ((PongGame)Game).Arena.GetPlayArea();
            _minY = playArea.Y + 5;
            _maxY = playArea.Height - _height - 5;
        }
    }
}
