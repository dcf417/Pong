using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Pong
{
    public class ScoreKeeper
    {
        private IDictionary<Player, int> _scores;
        private Player? _winner;
        private SoundEffect _scoreEffect;

        public ScoreKeeper(Game game)
        {
            KeepPlaying = true;
            _winner = null;
            _scores = new Dictionary<Player, int>();
            _scores.Add(Player.One, 0);
            _scores.Add(Player.Two, 0);

            _scoreEffect = game.Content.Load<SoundEffect>(@"Sounds\score");
        }

        public void Score(Player player)
        {
            _scores[player] += 1;
            _scoreEffect.Play();

            if (_scores[player] == 5)
            {
                _winner = player;
                KeepPlaying = false;
            }
        }

        public bool KeepPlaying { get; private set; }

        public int PlayerOneScore { get { return _scores[Player.One]; } }
        public int PlayerTwoScore { get { return _scores[Player.Two]; } }

        public string GetWinner()
        {
            return string.Format("Player {0}", _winner == Player.One ? "One" : "Two"); 
        }
    }
}
