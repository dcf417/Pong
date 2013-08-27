using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Pong.Managers
{
    public interface IInputManager
    {
        MoveState GetPlayerMove(Player player);
    }

    public class InputManager : GameComponent, IInputManager
    {
        private static readonly Keys _playerOneUp = Keys.W;
        private static readonly Keys _playerOneDown = Keys.S;
        private static readonly Keys _playerTwoUp = Keys.Up;
        private static readonly Keys _playerTwoDown = Keys.Down;

        private static Dictionary<Player, MoveState> _playerMovement;

        public InputManager(Game game) : base(game)
        {
            _playerMovement = new Dictionary<Player, MoveState>();
            _playerMovement.Add(Player.One, MoveState.None);
            _playerMovement.Add(Player.Two, MoveState.None);

            game.Components.Add(this);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            KeyboardState state = Keyboard.GetState();
            if(state.IsKeyDown(_playerOneUp))
            {
                _playerMovement[Player.One] = MoveState.Up;
            }
            else if(state.IsKeyDown(_playerOneDown))
            {
                _playerMovement[Player.One] = MoveState.Down;
            }
            else
            {
                _playerMovement[Player.One] = MoveState.None;
            }

            if (state.IsKeyDown(_playerTwoUp))
            {
                _playerMovement[Player.Two] = MoveState.Up;
            }
            else if (state.IsKeyDown(_playerTwoDown))
            {
                _playerMovement[Player.Two] = MoveState.Down;
            }
            else
            {
                _playerMovement[Player.Two] = MoveState.None;
            }
        }

        public MoveState GetPlayerMove(Player player)
        {
            return _playerMovement[player];
        }
    }
}
