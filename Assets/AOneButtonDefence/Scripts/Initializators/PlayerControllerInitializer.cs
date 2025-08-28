using System.Collections;
using AOneButtonDefence.Scripts.Data;
using UnityEngine;

namespace AOneButtonDefence.Scripts
{
    public class PlayerControllerInitializer
    {
        private PlayerController playerControllerInstance;
        private CharacterVisibilityToggler visibilityToggler;

        private readonly PlayerControllerInitializerData data;

        public PlayerControllerInitializer(PlayerControllerInitializerData data)
        {
            this.data = data;
        }

        public IEnumerator Initialize()
        {
            playerControllerInstance = Object.Instantiate(
                data.PlayerController,
                data.CharacterStartPosition,
                Quaternion.identity);

            playerControllerInstance.Initialize(data.PlayerData, data.CameraTransform);

            visibilityToggler = new CharacterVisibilityToggler(
                playerControllerInstance,
                data.CharacterStartPosition,
                data.BattleEvents);

            yield break;
        }

        public void Dispose()
        {
            visibilityToggler?.Dispose();
            if (playerControllerInstance != null)
            {
                Object.Destroy(playerControllerInstance.gameObject);
            }
        }

        public class PlayerControllerInitializerData
        {
            public readonly PlayerController PlayerController;
            public readonly PlayerControllerData PlayerData;
            public readonly Camera CameraTransform;
            public readonly Vector3 CharacterStartPosition;
            public readonly BattleEvents BattleEvents;

            public PlayerControllerInitializerData(
                PlayerController playerController,
                PlayerControllerData playerData,
                Camera cameraTransform,
                Vector3 characterStartPosition,
                BattleEvents battleEvents)
            {
                PlayerController = playerController;
                PlayerData = playerData;
                CameraTransform = cameraTransform;
                CharacterStartPosition = characterStartPosition;
                BattleEvents = battleEvents;
            }
        }
    }
}
