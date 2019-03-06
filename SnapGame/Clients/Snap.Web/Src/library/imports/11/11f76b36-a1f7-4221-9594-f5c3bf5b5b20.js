"use strict";
cc._RF.push(module, '11f76s2ofdCIZWU9cO/W1sg', 'GameRoom');
// Script/api/models/GameRoom.ts

Object.defineProperty(exports, "__esModule", { value: true });
var GameData_1 = require("./GameData");
var Player_1 = require("./Player");
var GameRoomPlayer_1 = require("./GameRoomPlayer");
var GameRoom = /** @class */ (function () {
    function GameRoom(data) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property)) {
                    this[property] = data[property];
                }
            }
        }
    }
    GameRoom.prototype.init = function (data) {
        if (data) {
            if (data.roomPlayers && data.roomPlayers.constructor === Array) {
                this.roomPlayers = [];
                for (var _i = 0, _a = data.roomPlayers; _i < _a.length; _i++) {
                    var item = _a[_i];
                    this.roomPlayers.push(GameRoomPlayer_1.GameRoomPlayer.fromJS(item));
                }
            }
            this.id = data.id;
            this.canJoin = data.canJoin;
            this.gameIdentifier = data.gameIdentifier;
            this.gamesData = data.gamesData ? GameData_1.GameData.fromJS(data.gamesData) : undefined;
            this.createdBy = data.createdBy ? Player_1.Player.fromJS(data.createdBy) : undefined;
        }
    };
    GameRoom.fromJS = function (data) {
        data = typeof data === "object" ? data : {};
        var result = new GameRoom();
        result.init(data);
        return result;
    };
    GameRoom.prototype.toJSON = function (data) {
        data = typeof data === "object" ? data : {};
        if (this.roomPlayers && this.roomPlayers.constructor === Array) {
            data.roomPlayers = [];
            for (var _i = 0, _a = this.roomPlayers; _i < _a.length; _i++) {
                var item = _a[_i];
                data.roomPlayers.push(item.toJSON());
            }
        }
        data.id = this.id;
        data.canJoin = this.canJoin;
        data.gameIdentifier = this.gameIdentifier;
        data.gamesData = this.gamesData ? this.gamesData.toJSON() : undefined;
        data.createdBy = this.createdBy ? this.createdBy.toJSON() : undefined;
        return data;
    };
    return GameRoom;
}());
exports.GameRoom = GameRoom;

cc._RF.pop();