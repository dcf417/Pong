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
    public class Ball : DrawableGameComponent
    {
        private Texture2D _texture;
        private Vector2 _startPosition;
        private Vector2 _position;
        private Vector2 _speed;
        private int _width;
        private int _height;
        private Vector2 _center;
        private int _minY;
        private int _maxY;
        private int _minX;
        private int _maxX;
        private SoundEffect _bounce;
        private SoundEffect _score;
        private Color[] _textureData;

        public Ball(Game game)
            : base(game)
        {
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
            _startPosition = _position = new Vector2(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2);
            _speed = new Vector2(0.5f, 0.5f);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _bounce = Game.Content.Load<SoundEffect>(@"Sounds\bounce");
            _score = Game.Content.Load<SoundEffect>(@"Sounds\score");

            _texture = Game.Content.Load<Texture2D>(@"Images\ball");
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

            _position += _speed * timeLapse;

            _position.X = MathHelper.Clamp(_position.X, _minX, _maxX);
            _position.Y = MathHelper.Clamp(_position.Y, _minY, _maxY);

            if (_position.Y == _minY || _position.Y == _maxY)
            {
                _bounce.Play();
                _speed.Y *= -1;
            }

            if (_position.X == _minX)
            {
                _score.Play();
                ((PongGame)Game).Scores[(int) Players.Two]++;
                _position = _startPosition;
                _speed.X *= -1;
            }

            if (_position.X == _maxX)
            {
                _score.Play();
                ((PongGame)Game).Scores[(int)Players.One]++;
                _position = _startPosition;
                _speed.X *= -1;
            }

            PaddleCollision collision = DidHitPaddle();
            switch (collision)
            {
                case PaddleCollision.Left:
                case PaddleCollision.Right:
                    _bounce.Play();
                    _speed.X *= -1;
                    break;
                case PaddleCollision.Top:
                case PaddleCollision.Bottom:
                    _bounce.Play();
                    _speed.Y *= -1;
                    break;
                default:
                    break;
            }
            
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch sb = ((PongGame)Game).SpriteBatch;
            sb.Draw(_texture, _position, Color.White);

            base.Draw(gameTime);
        }

        private PaddleCollision DidHitPaddle()
        {
            Paddle[] paddles = ((PongGame)Game).Paddles;
            Rectangle ballRectangle = new Rectangle((int)_position.X, (int)_position.Y, _width, _height);

            for (int i = 0; i < paddles.Length; i++)
            {
                var paddleRectangle = paddles[i].GetBoundingRectangle();
                var intersect = Rectangle.Intersect(ballRectangle, paddleRectangle);

                if (intersect != Rectangle.Empty)
                    if (DidPixelCollide(ballRectangle, TextureData, paddleRectangle, paddles[i].TextureData, intersect))
                        return GetCollisionDirection(intersect, paddleRectangle);
            }

            return PaddleCollision.None;
        }

        private bool DidPixelCollide(Rectangle rectangleA, Color[] dataA, Rectangle rectangleB, Color[] dataB, Rectangle intersect)
        {
            var paddleCollision = GetCollisionDirection(intersect, rectangleB);

            int top = intersect.Top;
            int bottom = intersect.Bottom;
            int left = intersect.Left;
            int right = intersect.Right;

            // Check every point within the intersection bounds
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    // Get the color of both pixels at this point
                    Color colorA = dataA[(x - rectangleA.Left) +
                                (y - rectangleA.Top) * rectangleA.Width];
                    Color colorB = dataB[(x - rectangleB.Left) +
                                (y - rectangleB.Top) * rectangleB.Width];

                    // If both pixels are not completely transparent,
                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        // then an intersection has been found
                        return true;
                    }
                }
            }

            // No intersection found
            return false;
        }

        private PaddleCollision GetCollisionDirection(Rectangle intersection, Rectangle paddleRectangle)
        {
            if (_speed.X > 0)
            {
                if (intersection.X >= paddleRectangle.Left)
                    return PaddleCollision.Left;
            }
            else
            {
                if (intersection.X <= paddleRectangle.Right)
                    return PaddleCollision.Right;
            }

            if (_speed.Y > 0)
            {
                if (intersection.Y >= paddleRectangle.Top)
                    return PaddleCollision.Top;
            }
            else
            {
                if (intersection.Y <= paddleRectangle.Bottom)
                    return PaddleCollision.Bottom;
            }

            return PaddleCollision.None;
        }

        private void SetBoundaries()
        {
            Rectangle playArea = ((PongGame)Game).Arena.GetPlayArea();
            _minX = playArea.X - _width;
            _maxX = playArea.Width + _width;
            _minY = playArea.Y;
            _maxY = playArea.Height - _height;
        }
    }
}
