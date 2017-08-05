/// <reference path="types-gt-mp/index.d.ts" />
var ModeEnum;
(function (ModeEnum) {
    ModeEnum[ModeEnum["Move"] = 0] = "Move";
    ModeEnum[ModeEnum["Rotate"] = 1] = "Rotate";
})(ModeEnum || (ModeEnum = {}));
var currentMode = ModeEnum.Move;
API.onKeyDown.connect(function (sender, event) {
    if (event.KeyCode === Keys.M) {
        if (currentMode === ModeEnum.Move) {
            currentMode = ModeEnum.Rotate;
        }
        else {
            currentMode = ModeEnum.Move;
        }
        API.sendChatMessage("Switched mode to " + (currentMode === 0 ? "MOVE" : "ROTATE"));
    }
    if (currentMode === ModeEnum.Move) {
        if (event.KeyCode === Keys.PageUp) {
            API.triggerServerEvent("move_up");
        }
        else if (event.KeyCode === Keys.PageDown) {
            API.triggerServerEvent("move_down");
        }
        else if (event.KeyCode === Keys.Up) {
            API.triggerServerEvent("move_forward");
        }
        else if (event.KeyCode === Keys.Down) {
            API.triggerServerEvent("move_backwards");
        }
        else if (event.KeyCode === Keys.Right) {
            API.triggerServerEvent("move_right");
        }
        else if (event.KeyCode === Keys.Left) {
            API.triggerServerEvent("move_left");
        }
    }
    if (currentMode === ModeEnum.Rotate) {
        if (event.KeyCode === Keys.Right) {
            API.triggerServerEvent("rotate_right");
        }
        else if (event.KeyCode === Keys.Left) {
            API.triggerServerEvent("rotate_left");
        }
        else if (event.KeyCode === Keys.PageUp) {
            API.triggerServerEvent("rotate_up");
        }
        else if (event.KeyCode === Keys.PageDown) {
            API.triggerServerEvent("rotate_down");
        }
        else if (event.KeyCode === Keys.Up) {
            API.triggerServerEvent("rotate_forward");
        }
        else if (event.KeyCode === Keys.Down) {
            API.triggerServerEvent("rotate_backwards");
        }
    }
});
