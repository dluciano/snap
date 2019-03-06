(function() {"use strict";var __module = CC_EDITOR ? module : {exports:{}};var __filename = 'preview-scripts/assets/Script/oauth/GameConfiguration.js';var __require = CC_EDITOR ? function (request) {return cc.require(request, require);} : function (request) {return cc.require(request, __filename);};function __define (exports, require, module) {"use strict";
cc._RF.push(module, '2d2fdQ8l7pOCLG41dLm+RUu', 'GameConfiguration', __filename);
// Script/oauth/GameConfiguration.ts

"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var GameConfiguration = /** @class */ (function () {
    function GameConfiguration() {
        this.playersEnpoint = "/api/Player";
        this._player = null;
    }
    Object.defineProperty(GameConfiguration.prototype, "oauthConfiguration", {
        get: function () {
            return this._oauthConfiguration;
        },
        set: function (config) {
            this._oauthConfiguration = config;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GameConfiguration.prototype, "apiUrl", {
        get: function () {
            return this._apiUrl;
        },
        set: function (baseUrl) {
            this._apiUrl = baseUrl;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GameConfiguration.prototype, "gameServerUrl", {
        get: function () {
            return this._serverUrl;
        },
        set: function (_serverUrl) {
            this._serverUrl = _serverUrl;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GameConfiguration.prototype, "currentToken", {
        get: function () {
            return this._token;
        },
        set: function (token) {
            this._token = token;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GameConfiguration.prototype, "currentPlayer", {
        get: function () {
            return this._player;
        },
        set: function (player) {
            this._player = player;
            this.dispatch(GameConfiguration.ON_PLAYER_DATA_UPDATED, this._player);
        },
        enumerable: true,
        configurable: true
    });
    GameConfiguration.prototype.dispatch = function (type, data) {
        var e = new cc.Event.EventCustom(type, true);
        e.target = this;
        e.setUserData(data);
        cc.systemEvent.dispatchEvent(e);
    };
    GameConfiguration.ON_PLAYER_DATA_UPDATED = "onPlayerDataUpdated";
    return GameConfiguration;
}());
exports.GameConfiguration = GameConfiguration;

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
        //# sourceMappingURL=GameConfiguration.js.map
        