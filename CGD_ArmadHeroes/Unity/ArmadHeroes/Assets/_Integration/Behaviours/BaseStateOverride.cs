/// <summary>
/// BaseStateOverride.cs
/// Created and implemented by Daniel Weston 29/04/2016
/// </summary>
using UnityEngine;
using System.Collections;
using ArmadHeroes;

namespace ArmadHeroes
{
    public abstract class BaseStateOverride : MonoBehaviour
    {
        protected virtual void Start()
        {
            ArmadHeroes.GameManager.instance.m_pauseGame = Pause;
            ArmadHeroes.GameManager.instance.m_playGame = Play;
            ArmadHeroes.GameManager.instance.m_gameOver = GameOver;
       }

        protected abstract void Pause();

        protected abstract void Play();

        protected abstract void GameOver();
    }
}