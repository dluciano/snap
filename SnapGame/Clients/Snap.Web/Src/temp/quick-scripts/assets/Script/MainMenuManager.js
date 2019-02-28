(function() {"use strict";var __module = CC_EDITOR ? module : {exports:{}};var __filename = 'preview-scripts/assets/Script/MainMenuManager.js';var __require = CC_EDITOR ? function (request) {return cc.require(request, require);} : function (request) {return cc.require(request, __filename);};function __define (exports, require, module) {"use strict";
cc._RF.push(module, '4a09fwjIB9Fer1pcqIIHgoJ', 'MainMenuManager', __filename);
// Script/MainMenuManager.ts

"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ItemRoomScrollView_1 = require("../Prefabs/ItemRoomScrollView");
var ConnectionManager_1 = require("./ConnectionManager");
var SnapGame_1 = require("./SnapGame");
var WaitingRoomManager_1 = require("./WaitingRoomManager");
var _a = cc._decorator, ccclass = _a.ccclass, property = _a.property;
var MainMenuManager = /** @class */ (function (_super) {
    __extends(MainMenuManager, _super);
    function MainMenuManager() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.loginNameLbl = null;
        _this.connectionStatusLbl = null;
        _this.loginBtn = null;
        _this.logoutBtn = null;
        _this.reconnectBtn = null;
        _this.itemPrefab = null;
        _this.roomScrollView = null;
        _this.roomSceneName = "";
        _this.connection = ConnectionManager_1.ConnectionManager.getInstance();
        return _this;
    }
    MainMenuManager.prototype.onLoad = function () {
        cc.systemEvent
            .on(ConnectionManager_1.ConnectionManager.ON_CONNECTED, this.onConnected, this);
        cc.systemEvent
            .on(ConnectionManager_1.ConnectionManager.ON_CONNECTION_CLOSED, this.onDisconnected, this);
        cc.systemEvent
            .on(ConnectionManager_1.ConnectionManager.ON_PLAYER_LOGIN_EVENT, this.onLogin, this);
        cc.systemEvent
            .on(ConnectionManager_1.ConnectionManager.ON_CREATE_ROOM, this.onRoomCreated, this);
        cc.systemEvent
            .on(ConnectionManager_1.ConnectionManager.ON_JOIN_GAME_MESSAGE, this.onJoinGame, this);
        this.refresh();
    };
    MainMenuManager.prototype.onDestroy = function () {
        cc.systemEvent
            .off(ConnectionManager_1.ConnectionManager.ON_CONNECTED, this.onConnected, this);
        cc.systemEvent
            .off(ConnectionManager_1.ConnectionManager.ON_CONNECTION_CLOSED, this.onDisconnected, this);
        cc.systemEvent
            .off(ConnectionManager_1.ConnectionManager.ON_PLAYER_LOGIN_EVENT, this.onLogin, this);
        cc.systemEvent
            .off(ConnectionManager_1.ConnectionManager.ON_CREATE_ROOM, this.onRoomCreated, this);
        cc.systemEvent
            .off(ConnectionManager_1.ConnectionManager.ON_JOIN_GAME_MESSAGE, this.onJoinGame, this);
    };
    MainMenuManager.prototype.onJoinGame = function (e) {
        var joinedUser = e.getUserData().username;
        var roomId = e.getUserData().roomId;
        if (joinedUser === this.connection.player.username) {
            cc.director.loadScene(this.roomSceneName, function () {
                var scene = cc.director.getScene();
                var game = scene.getChildByName("Game").getComponent(SnapGame_1.default);
                var sceneManager = scene.getChildByName("WaitingRoomManager").getComponent(WaitingRoomManager_1.default);
                game.roomId = roomId;
                sceneManager.loadData();
            });
        }
    };
    MainMenuManager.prototype.createRoom = function () {
        this.connection.createRoom();
    };
    MainMenuManager.prototype.onRoomCreated = function (e) {
        var roomId = e.getUserData().roomId;
        var createdBy = e.getUserData().createdBy;
        if (createdBy === this.connection.player.username) {
            cc.director.loadScene(this.roomSceneName, function () {
                var scene = cc.director.getScene();
                var game = scene.getChildByName("Game").getComponent(SnapGame_1.default);
                var sceneManager = scene.getChildByName("WaitingRoomManager").getComponent(WaitingRoomManager_1.default);
                game.roomId = roomId;
                game.createdBy = createdBy;
                sceneManager.loadData();
            });
            return;
        }
        this.addRoomItem(roomId);
    };
    MainMenuManager.prototype.addRoomItem = function (roomId) {
        cc.log("Game created received: " + roomId);
        var node = cc.instantiate(this.itemPrefab);
        var item = node.getComponent(ItemRoomScrollView_1.default);
        item.id = roomId;
        var lbl = node.getComponentInChildren(cc.Label);
        lbl.string = "Room #" + roomId;
        var itemCount = this.roomScrollView.content.childrenCount;
        var h = node.height + 5;
        var y = (itemCount * h);
        this.roomScrollView.content.addChild(node);
        node.setPosition(0, -y);
        this.roomScrollView.content.height += h;
    };
    MainMenuManager.prototype.onConnected = function (e) {
        this.connectionStatusLbl.string = "Connected";
    };
    MainMenuManager.prototype.onDisconnected = function (e) {
        this.connectionStatusLbl.string = "Disconnected";
    };
    MainMenuManager.prototype.onLogin = function (e) {
        this.loginNameLbl.string = this.connection.player.username;
    };
    MainMenuManager.prototype.connect = function () {
        this.connection.connect();
    };
    MainMenuManager.prototype.refresh = function () {
        var _this = this;
        this.connection.refresh();
        fetch("https://localhost:44378/api/GameRoom")
            .then(function (data) {
            return data.json();
        }).then(function (rooms) {
            rooms.forEach(function (room) {
                _this.addRoomItem(room.id);
            });
        });
    };
    MainMenuManager.prototype.login = function () {
        this.connection.login();
    };
    MainMenuManager.prototype.logout = function () {
        this.connection.logout();
    };
    __decorate([
        property(cc.Label)
    ], MainMenuManager.prototype, "loginNameLbl", void 0);
    __decorate([
        property(cc.Label)
    ], MainMenuManager.prototype, "connectionStatusLbl", void 0);
    __decorate([
        property(cc.Button)
    ], MainMenuManager.prototype, "loginBtn", void 0);
    __decorate([
        property(cc.Button)
    ], MainMenuManager.prototype, "logoutBtn", void 0);
    __decorate([
        property(cc.Button)
    ], MainMenuManager.prototype, "reconnectBtn", void 0);
    __decorate([
        property(cc.Prefab)
    ], MainMenuManager.prototype, "itemPrefab", void 0);
    __decorate([
        property(cc.ScrollView)
    ], MainMenuManager.prototype, "roomScrollView", void 0);
    __decorate([
        property()
    ], MainMenuManager.prototype, "roomSceneName", void 0);
    MainMenuManager = __decorate([
        ccclass
    ], MainMenuManager);
    return MainMenuManager;
}(cc.Component));
exports.default = MainMenuManager;

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
        //# sourceMappingURL=MainMenuManager.js.map
        