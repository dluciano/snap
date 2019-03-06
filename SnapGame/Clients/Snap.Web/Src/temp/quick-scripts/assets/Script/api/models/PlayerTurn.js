(function() {"use strict";var __module = CC_EDITOR ? module : {exports:{}};var __filename = 'preview-scripts/assets/Script/api/models/PlayerTurn.js';var __require = CC_EDITOR ? function (request) {return cc.require(request, require);} : function (request) {return cc.require(request, __filename);};function __define (exports, require, module) {"use strict";
cc._RF.push(module, '219b1aaBNZLK4dEUdSyuLlJ', 'PlayerTurn', __filename);
// Script/api/models/PlayerTurn.ts

Object.defineProperty(exports, "__esModule", { value: true });
var Player_1 = require("./Player");
var GameData_1 = require("./GameData");
var PlayerTurn = /** @class */ (function () {
    function PlayerTurn(data) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property)) {
                    this[property] = data[property];
                }
            }
        }
    }
    PlayerTurn.prototype.init = function (data) {
        if (data) {
            this.player = data.player ? Player_1.Player.fromJS(data.player) : undefined;
            this.next = data.next ? PlayerTurn.fromJS(data.next) : undefined;
            if (data.firstPlayers && data.firstPlayers.constructor === Array) {
                this.firstPlayers = [];
                for (var _i = 0, _a = data.firstPlayers; _i < _a.length; _i++) {
                    var item = _a[_i];
                    this.firstPlayers.push(GameData_1.GameData.fromJS(item));
                }
            }
            if (data.currentTurns && data.currentTurns.constructor === Array) {
                this.currentTurns = [];
                for (var _b = 0, _c = data.currentTurns; _b < _c.length; _b++) {
                    var item = _c[_b];
                    this.currentTurns.push(GameData_1.GameData.fromJS(item));
                }
            }
            this.id = data.id;
        }
    };
    PlayerTurn.fromJS = function (data) {
        data = typeof data === "object" ? data : {};
        var result = new PlayerTurn();
        result.init(data);
        return result;
    };
    PlayerTurn.prototype.toJSON = function (data) {
        data = typeof data === "object" ? data : {};
        data.player = this.player ? this.player.toJSON() : undefined;
        data.next = this.next ? this.next.toJSON() : undefined;
        if (this.firstPlayers && this.firstPlayers.constructor === Array) {
            data.firstPlayers = [];
            for (var _i = 0, _a = this.firstPlayers; _i < _a.length; _i++) {
                var item = _a[_i];
                data.firstPlayers.push(item.toJSON());
            }
        }
        if (this.currentTurns && this.currentTurns.constructor === Array) {
            data.currentTurns = [];
            for (var _b = 0, _c = this.currentTurns; _b < _c.length; _b++) {
                var item = _c[_b];
                data.currentTurns.push(item.toJSON());
            }
        }
        data.id = this.id;
        return data;
    };
    return PlayerTurn;
}());
exports.PlayerTurn = PlayerTurn;

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
        //# sourceMappingURL=PlayerTurn.js.map
        