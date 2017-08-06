using System;
using GrandTheftMultiplayer.Server.API;
using GrandTheftMultiplayer.Server.Elements;
using GrandTheftMultiplayer.Server.Managers;
using GrandTheftMultiplayer.Shared.Math;
using Object = GrandTheftMultiplayer.Server.Elements.Object;

namespace PropMover
{
    public class PropMover : Script
    {
        #region Fields

        private static float _moveUnit = 0.01f;
        private static float _rotateUnit = 5f;

        private int _model;
        private Vector3 _position;
        private Vector3 _rotation;
        private Object _attachedObject;
        private string _bone;

        #endregion

        #region Enums

        [Flags]
        public enum AnimationFlags
        {
            Loop = 1 << 0,
            StopOnLastFrame = 1 << 1,
            OnlyAnimateUpperBody = 1 << 4,
            AllowPlayerControl = 1 << 5,
            Cancellable = 1 << 7
        }

        #endregion

        #region Methods

        public PropMover()
        {
            _position = new Vector3();
            _rotation = new Vector3();
            _bone = "IK_R_Hand";

            API.onClientEventTrigger += OnOnClientEventTrigger;
        }

        #region Commands

        [Command("setprop")]
        public void SetProp(Client player, int model)
        {
            if (_attachedObject != null)
            {
                API.detachEntity(_attachedObject);
                API.deleteEntity(_attachedObject);
            }

            _model = model;
            _attachedObject = API.createObject(_model, new Vector3(), new Vector3());
            UpdateObject(player);

            API.sendChatMessageToPlayer(player, "Set prop to " + model);
        }

        [Command("setbone")]
        public void SetBone(Client player, string bone)
        {
            _bone = bone;
            UpdateObject(player);

            API.sendChatMessageToPlayer(player, "Set bone to " + bone);
        }

        [Command("setposition")]
        public void SetPosition(Client player, float x, float y, float z)
        {
            _position = new Vector3(x, y, z);
            UpdateObject(player);

            API.sendChatMessageToPlayer(player, "Set position to " + _position);
        }

        [Command("setrotation")]
        public void SetRotation(Client player, float x, float y, float z)
        {
            _rotation = new Vector3(x, y, z);
            UpdateObject(player);

            API.sendChatMessageToPlayer(player, "Set rotation to " + _rotation);
        }

        [Command("setmoveunit")]
        public void SetMoveUnit(Client player, float units)
        {
            _moveUnit = units;
            UpdateObject(player);

            API.sendChatMessageToPlayer(player, "Set move unit to " + units);
        }

        [Command("setrotateunit")]
        public void SetRotateUnit(Client player, float units)
        {
            _rotateUnit = units;
            UpdateObject(player);

            API.sendChatMessageToPlayer(player, "Set rotate unit to " + units);
        }

        [Command("resetposition")]
        public void ResetPosition(Client player)
        {
            _position = new Vector3();
            UpdateObject(player);

            API.sendChatMessageToPlayer(player, "Resetted position");
        }

        [Command("resetrotation")]
        public void ResetRotation(Client player)
        {
            _rotation = new Vector3();
            UpdateObject(player);

            API.sendChatMessageToPlayer(player, "Resetted rotation");
        }

        [Command("printprop")]
        public void PrintPropPositionAndRotation(Client player)
        {
            API.sendChatMessageToPlayer(player, "Prop model: " + _model);
            API.sendChatMessageToPlayer(player, "Bone: " + _bone);
            API.sendChatMessageToPlayer(player, "Position: " + _position);
            API.sendChatMessageToPlayer(player, "Rotation: " + _rotation);

            API.consoleOutput("Prop model: " + _model);
            API.consoleOutput("Bone: " + _bone);
            API.consoleOutput("Position: " + _position);
            API.consoleOutput("Rotation: " + _rotation);
        }

        [Command("playanimation")]
        public void PlayAnimation(Client player, string animDict, string animName)
        {
            API.playPlayerAnimation(player,
                (int) (AnimationFlags.AllowPlayerControl | AnimationFlags.Loop | AnimationFlags.OnlyAnimateUpperBody |
                       AnimationFlags.Cancellable), animDict, animName);

            API.sendChatMessageToPlayer(player, "Playing animation " + animDict + " - " + animName);
        }

        [Command("stopanimation")]
        public void StopAnimation(Client player)
        {
            API.stopPlayerAnimation(player);

            API.sendChatMessageToPlayer(player, "Stopping animation");
        }

        #endregion

        private void OnOnClientEventTrigger(Client sender, string eventName, params object[] arguments)
        {
            if (_attachedObject == null)
            {
                return;
            }

            switch (eventName)
            {
                case "move_up":
                    _position.X -= _moveUnit;
                    break;
                case "move_down":
                    _position.X += _moveUnit;
                    break;
                case "move_forward":
                    _position.Y += _moveUnit;
                    break;
                case "move_backwards":
                    _position.Y -= _moveUnit;
                    break;
                case "move_left":
                    _position.Z -= _moveUnit;
                    break;
                case "move_right":
                    _position.Z += _moveUnit;
                    break;

                case "rotate_up":
                    _rotation.X -= _rotateUnit;
                    break;
                case "rotate_down":
                    _rotation.X += _rotateUnit;
                    break;
                case "rotate_forward":
                    _rotation.Y += _rotateUnit;
                    break;
                case "rotate_backwards":
                    _rotation.Y -= _rotateUnit;
                    break;
                case "rotate_left":
                    _rotation.Z -= _rotateUnit;
                    break;
                case "rotate_right":
                    _rotation.Z += _rotateUnit;
                    break;

                default:
                    return;
            }

            UpdateObject(sender);
        }

        private void UpdateObject(Client player)
        {
            if (_attachedObject == null)
            {
                return;
            }

            if (API.isEntityAttachedToAnything(_attachedObject))
            {
                API.detachEntity(_attachedObject);
            }

            API.attachEntityToEntity(_attachedObject, player, _bone, _position, _rotation);
        }

        #endregion
    }
}