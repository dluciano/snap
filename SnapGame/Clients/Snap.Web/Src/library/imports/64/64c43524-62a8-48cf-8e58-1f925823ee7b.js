"use strict";
cc._RF.push(module, '64c43UkYqhIz45YH5JYI+57', 'Player');
// Script/api/models/Player.ts

Object.defineProperty(exports, "__esModule", { value: true });
var GameRoom_1 = require("./GameRoom");
var GameRoomPlayer_1 = require("./GameRoomPlayer");
var PlayerTurn_1 = require("./PlayerTurn");
var Player = /** @class */ (function () {
    function Player(data) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property)) {
                    this[property] = data[property];
                }
            }
        }
    }
    Player.prototype.init = function (data) {
        if (data) {
            this.username = data.username;
            if (data.gameRoomPlayers && data.gameRoomPlayers.constructor === Array) {
                this.gameRoomPlayers = [];
                for (var _i = 0, _a = data.gameRoomPlayers; _i < _a.length; _i++) {
                    var item = _a[_i];
                    this.gameRoomPlayers.push(GameRoomPlayer_1.GameRoomPlayer.fromJS(item));
                }
            }
            if (data.playerTurns && data.playerTurns.constructor === Array) {
                this.playerTurns = [];
                for (var _b = 0, _c = data.playerTurns; _b < _c.length; _b++) {
                    var item = _c[_b];
                    this.playerTurns.push(PlayerTurn_1.PlayerTurn.fromJS(item));
                }
            }
            this.id = data.id;
            if (data.createdRooms && data.createdRooms.constructor === Array) {
                this.createdRooms = [];
                for (var _d = 0, _e = data.createdRooms; _d < _e.length; _d++) {
                    var item = _e[_d];
                    this.createdRooms.push(GameRoom_1.GameRoom.fromJS(item));
                }
            }
        }
        return this;
    };
    Player.prototype.toJSON = function (data) {
        data = typeof data === "object" ? data : {};
        data.username = this.username;
        if (this.gameRoomPlayers && this.gameRoomPlayers.constructor === Array) {
            data.gameRoomPlayers = [];
            for (var _i = 0, _a = this.gameRoomPlayers; _i < _a.length; _i++) {
                var item = _a[_i];
                data.gameRoomPlayers.push(item.toJSON());
            }
        }
        if (this.playerTurns && this.playerTurns.constructor === Array) {
            data.playerTurns = [];
            for (var _b = 0, _c = this.playerTurns; _b < _c.length; _b++) {
                var item = _c[_b];
                data.playerTurns.push(item.toJSON());
            }
        }
        data.id = this.id;
        if (this.createdRooms && this.createdRooms.constructor === Array) {
            data.createdRooms = [];
            for (var _d = 0, _e = this.createdRooms; _d < _e.length; _d++) {
                var item = _e[_d];
                data.createdRooms.push(item.toJSON());
            }
        }
        return data;
    };
    return Player;
}());
exports.Player = Player;

cc._RF.pop();