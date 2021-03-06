(function() {"use strict";var __module = CC_EDITOR ? module : {exports:{}};var __filename = 'preview-scripts/assets/Script/api/models/GameRoomPlayer.js';var __require = CC_EDITOR ? function (request) {return cc.require(request, require);} : function (request) {return cc.require(request, __filename);};function __define (exports, require, module) {"use strict";
cc._RF.push(module, '9d400YyC5BCbbNl4Gk8DOPx', 'GameRoomPlayer', __filename);
// Script/api/models/GameRoomPlayer.ts

Object.defineProperty(exports, "__esModule", { value: true });
var GameRoom_1 = require("./GameRoom");
var Player_1 = require("./Player");
var GameRoomPlayer = /** @class */ (function () {
    function GameRoomPlayer(data) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property)) {
                    this[property] = data[property];
                }
            }
        }
    }
    GameRoomPlayer.prototype.init = function (data) {
        if (data) {
            this.player = data.player ? Player_1.Player.fromJS(data.player) : undefined;
            this.isViewer = data.isViewer;
            this.gameRoom = data.gameRoom ? GameRoom_1.GameRoom.fromJS(data.gameRoom) : undefined;
            this.id = data.id;
            this.playerId = data.playerId;
            this.roomId = data.roomId;
        }
    };
    GameRoomPlayer.fromJS = function (data) {
        data = typeof data === "object" ? data : {};
        var result = new GameRoomPlayer();
        result.init(data);
        return result;
    };
    GameRoomPlayer.prototype.toJSON = function (data) {
        data = typeof data === "object" ? data : {};
        data.player = this.player ? this.player.toJSON() : undefined;
        data.isViewer = this.isViewer;
        data.gameRoom = this.gameRoom ? this.gameRoom.toJSON() : undefined;
        data.id = this.id;
        data.playerId = this.playerId;
        data.roomId = this.roomId;
        return data;
    };
    return GameRoomPlayer;
}());
exports.GameRoomPlayer = GameRoomPlayer;

cc._RF.pop();
        }
        if (CC_EDITOR) {
            __define(__module.exports, __require, __module);
        }
        else {
            cc.registerModuleFunc(__filename, function () {
                __define(__module.exports, __require, __module);
            });
        }
        })();
        //# sourceMappingURL=GameRoomPlayer.js.map
        