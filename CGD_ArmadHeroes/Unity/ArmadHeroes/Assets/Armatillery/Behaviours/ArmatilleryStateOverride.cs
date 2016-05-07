/// <summary>
/// ArmatilleryStateOverride.cs
/// Created and implemented by Daniel Weston 29/04/2016
/// </summary>
using UnityEngine;
using System.Collections;
using ArmadHeroes;

namespace Armatillery
{
    public class ArmatilleryStateOverride : ArmadHeroes.BaseStateOverride
    {
        protected override void Start()
        {
            ArmadHeroes.GameManager.instance.m_gameOver = GameOver;
        }

        protected override void GameOver()
        {
            foreach (ArmaPlayer _player in ArmaPlayerManager.instance.m_players)
	        {
                GlobalPlayerManager.instance.SetDebriefStats(_player.playerNumber, _player.chevron_score, _player.accolade_timesShot, _player.accolade_distance, _player.accolade_distance,
                    _player.accolade_shotsFired, _player.accolade_unique);
	        }

        }

        protected override void Pause()
        {
            ArmaPlayerManager.instance.VibratePlayers(0, 0);
        }

        protected override void Play()
        {
            throw new System.NotImplementedException();
        }
    }
}