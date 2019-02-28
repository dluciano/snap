(function() {"use strict";var __module = CC_EDITOR ? module : {exports:{}};var __filename = 'preview-scripts/assets/Script/WaitingRoomManager.js';var __require = CC_EDITOR ? function (request) {return cc.require(request, require);} : function (request) {return cc.require(request, __filename);};function __define (exports, require, module) {"use strict";
cc._RF.push(module, '761e34sVU9P5L936SCXovp9', 'WaitingRoomManager', __filename);
// Script/WaitingRoomManager.ts

"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ConnectionManager_1 = require("./ConnectionManager");
require("./ConnectionManager");
var SnapGame_1 = require("./SnapGame");
var MainGameManager_1 = require("./MainGameManager");
var _a = cc._decorator, ccclass = _a.ccclass, property = _a.property;
var WaitingRoomManager = /** @class */ (function (_super) {
    __extends(WaitingRoomManager, _super);
    function WaitingRoomManager() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.userPrefab = null;
        _this.menuScene = "";
        _this.mainGameScene = "";
        _this.titleLabel = null;
        _this.snapGameNode = null;
        _this.playersPanelNode = null;
        _this.game = null;
        _this.connection = ConnectionManager_1.default.getInstance();
        return _this;
    }
    WaitingRoomManager.prototype.onLoad = function () {
        cc.systemEvent
            .on(ConnectionManager_1.default.ON_START_GAME_MESSAGE, this.gameStarted, this);
        this.game = this.snapGameNode.getComponent(SnapGame_1.default);
        cc.systemEvent
            .on(ConnectionManager_1.default.ON_JOIN_GAME_MESSAGE, this.onJoinGame, this);
    };
    WaitingRoomManager.prototype.onDestroy = function () {
        cc.systemEvent
            .off(ConnectionManager_1.default.ON_START_GAME_MESSAGE, this.gameStarted, this);
        cc.systemEvent
            .off(ConnectionManager_1.default.ON_JOIN_GAME_MESSAGE, this.onJoinGame, this);
    };
    WaitingRoomManager.prototype.loadData = function () {
        var _this = this;
        this.titleLabel.string = "Room #" + this.game.roomId.toString();
        fetch("https://localhost:44378/api/GameRoom/" + this.game.roomId)
            .then(function (data) {
            return data.json();
        }).then(function (room) {
            room.roomPlayers.map(function (rp) { return rp.player; }).forEach(function (player) {
                _this.addPlayerItem(player.username);
            });
        });
    };
    WaitingRoomManager.prototype.onJoinGame = function (e) {
        var joinedUser = e.getUserData().username;
        this.addPlayerItem(joinedUser);
    };
    WaitingRoomManager.prototype.startGame = function () {
        ConnectionManager_1.default.getInstance().startGame(this.game.roomId);
    };
    WaitingRoomManager.prototype.gameStarted = function (e) {
        var data = e.getUserData();
        var gameId = data.gameId;
        var currentPlayer = data.currentPlayer;
        cc.director.loadScene(this.mainGameScene, function () {
            var scene = cc.director.getScene();
            var gameManager = scene
                .getChildByName("MainGameManager")
                .getComponent(MainGameManager_1.default);
            var game = scene
                .getChildByName("Game")
                .getComponent(SnapGame_1.default);
            game.gameId = gameId;
            game.currentPlayer = currentPlayer;
            game.playerData = data.playerData;
            gameManager.loadData();
        });
    };
    WaitingRoomManager.prototype.addPlayerItem = function (username) {
        cc.log("User: " + username + " joined the match");
        var node = cc.instantiate(this.userPrefab);
        var lbl = node.getChildByName("Label").getComponent(cc.Label);
        lbl.string = username.toString();
        this.playersPanelNode.addChild(node);
    };
    WaitingRoomManager.prototype.back = function () {
        cc.director.loadScene(this.menuScene);
    };
    WaitingRoomManager.prototype.deleteRoom = function () {
        throw "Not yet implemented";
    };
    __decorate([
        property(cc.Prefab)
    ], WaitingRoomManager.prototype, "userPrefab", void 0);
    __decorate([
        property()
    ], WaitingRoomManager.prototype, "menuScene", void 0);
    __decorate([
        property()
    ], WaitingRoomManager.prototype, "mainGameScene", void 0);
    __decorate([
        property(cc.Label)
    ], WaitingRoomManager.prototype, "titleLabel", void 0);
    __decorate([
        property(cc.Node)
    ], WaitingRoomManager.prototype, "snapGameNode", void 0);
    __decorate([
        property(cc.Node)
    ], WaitingRoomManager.prototype, "playersPanelNode", void 0);
    WaitingRoomManager = __decorate([
        ccclass
    ], WaitingRoomManager);
    return WaitingRoomManager;
}(cc.Component));
exports.default = WaitingRoomManager;

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
        //# sourceMappingURL=WaitingRoomManager.js.map
        