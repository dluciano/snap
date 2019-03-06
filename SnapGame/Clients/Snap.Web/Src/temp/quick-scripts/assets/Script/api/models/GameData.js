(function() {"use strict";var __module = CC_EDITOR ? module : {exports:{}};var __filename = 'preview-scripts/assets/Script/api/models/GameData.js';var __require = CC_EDITOR ? function (request) {return cc.require(request, require);} : function (request) {return cc.require(request, __filename);};function __define (exports, require, module) {"use strict";
cc._RF.push(module, 'c6d64CYIzBBkJq4flqYJDPn', 'GameData', __filename);
// Script/api/models/GameData.ts

Object.defineProperty(exports, "__esModule", { value: true });
var PlayerTurn_1 = require("./PlayerTurn");
var GameData = /** @class */ (function () {
    function GameData(data) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property)) {
                    this[property] = data[property];
                }
            }
        }
    }
    GameData.prototype.init = function (data) {
        if (data) {
            this.firstPlayer = data.firstPlayer ? PlayerTurn_1.PlayerTurn.fromJS(data.firstPlayer) : undefined;
            this.currentTurn = data.currentTurn ? PlayerTurn_1.PlayerTurn.fromJS(data.currentTurn) : undefined;
            if (data.turns && data.turns.constructor === Array) {
                this.turns = [];
                for (var _i = 0, _a = data.turns; _i < _a.length; _i++) {
                    var item = _a[_i];
                    this.turns.push(PlayerTurn_1.PlayerTurn.fromJS(item));
                }
            }
            this.id = data.id;
            this.currentState = data.currentState;
        }
    };
    GameData.fromJS = function (data) {
        data = typeof data === "object" ? data : {};
        throw new Error("The abstract class 'GameData' cannot be instantiated.");
    };
    GameData.prototype.toJSON = function (data) {
        data = typeof data === "object" ? data : {};
        data.firstPlayer = this.firstPlayer ? this.firstPlayer.toJSON() : undefined;
        data.currentTurn = this.currentTurn ? this.currentTurn.toJSON() : undefined;
        if (this.turns && this.turns.constructor === Array) {
            data.turns = [];
            for (var _i = 0, _a = this.turns; _i < _a.length; _i++) {
                var item = _a[_i];
                data.turns.push(item.toJSON());
            }
        }
        data.id = this.id;
        data.currentState = this.currentState;
        return data;
    };
    return GameData;
}());
exports.GameData = GameData;

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
        //# sourceMappingURL=GameData.js.map
        