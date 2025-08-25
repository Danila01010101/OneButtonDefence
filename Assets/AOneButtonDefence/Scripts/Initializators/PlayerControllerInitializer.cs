using System;
using System.Collections;
using AOneButtonDefence.Scripts.Data;
using UnityEngine;

namespace AOneButtonDefence.Scripts.Initializators
{
    public class PlayerControllerInitializer : IGameInitializerStep
    {        
        public CharacterVisibilityToggler CharacterVisibilityToggler { get; private set; }
        
        private readonly PlayerControllerInitializerData data;

        public PlayerControllerInitializer(PlayerControllerInitializerData data)
        {
            this.data = data;
        }

        public IEnumerator Initialize()
        {
            PlayerController playerControllerInstance = UnityEngine.Object.Instantiate(data.PlayerController);
            playerControllerInstance.Initialize(data.PlayerData, data.CameraTransform);
            CharacterVisibilityToggler = new CharacterVisibilityToggler(data.PlayerController, 
                data.CharacterStartPosition, data.BattleStarted, data.BattleEnded );
            yield break;
        }

        public class PlayerControllerInitializerData
        {
            public readonly PlayerController PlayerController;
            public readonly PlayerControllerData PlayerData;
            public readonly Transform CameraTransform;
            public readonly Vector3 CharacterStartPosition;
            public readonly Action BattleStarted;
            public readonly Action BattleEnded;

            public PlayerControllerInitializerData(PlayerController playerController, PlayerControllerData playerData, Transform cameraTransform, Vector3 characterStartPosition, Action battleStarted, Action battleEnded)
            {
                PlayerController = playerController;
                PlayerData = playerData;
                CameraTransform = cameraTransform;
                CharacterStartPosition = characterStartPosition;
                BattleStarted = battleStarted;
                BattleEnded = battleEnded;
            }
        }
    }
}