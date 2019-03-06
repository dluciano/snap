"use strict";
cc._RF.push(module, '4a09fwjIB9Fer1pcqIIHgoJ', 'MainMenuManager');
// Script/MainMenuManager.ts

"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ItemRoomScrollView_1 = require("../Prefabs/ItemRoomScrollView");
var ConnectionManager_1 = require("./ConnectionManager");
var SnapGame_1 = require("./SnapGame");
var WaitingRoomManager_1 = require("./WaitingRoomManager");
var SecurityManager_1 = require("./oauth/SecurityManager");
var api_1 = require("./api");
var GameConfiguration_1 = require("./oauth/GameConfiguration");
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
        _this.gameConfig = new GameConfiguration_1.GameConfiguration();
        _this.start = function () { return __awaiter(_this, void 0, Promise, function () {
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.refresh()];
                    case 1:
                        _a.sent();
                        return [2 /*return*/];
                }
            });
        }); };
        _this.createRoom = function () { return __awaiter(_this, void 0, Promise, function () {
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.connection.createRoom()];
                    case 1:
                        _a.sent();
                        return [2 /*return*/];
                }
            });
        }); };
        _this.connect = function () { return __awaiter(_this, void 0, Promise, function () {
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.connection.connect()];
                    case 1:
                        _a.sent();
                        return [2 /*return*/];
                }
            });
        }); };
        _this.refresh = function () { return __awaiter(_this, void 0, Promise, function () {
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.oauth.login()];
                    case 1:
                        _a.sent();
                        return [4 /*yield*/, this.connection.connect()];
                    case 2:
                        _a.sent();
                        return [2 /*return*/];
                }
            });
        }); };
        _this.login = function () { return __awaiter(_this, void 0, Promise, function () {
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.oauth.login()];
                    case 1:
                        _a.sent();
                        return [2 /*return*/];
                }
            });
        }); };
        return _this;
    }
    /**
     *
     */
    MainMenuManager.prototype.onLoad = function () {
        var _this = this;
        cc.systemEvent
            .on(ConnectionManager_1.ConnectionManager.ON_CONNECTED, this.onConnected, this);
        cc.systemEvent
            .on(ConnectionManager_1.ConnectionManager.ON_CONNECTION_CLOSED, this.onDisconnected, this);
        cc.systemEvent
            .on(GameConfiguration_1.GameConfiguration.ON_PLAYER_DATA_UPDATED, this.onPlayerDataUpdated, this);
        cc.systemEvent
            .on(ConnectionManager_1.ConnectionManager.ON_CREATE_ROOM, this.onRoomCreated, this);
        cc.systemEvent
            .on(ConnectionManager_1.ConnectionManager.ON_JOIN_GAME_MESSAGE, this.onJoinGame, this);
        this.gameConfig.oauthConfiguration = {
            providerID: "snapGameOauth",
            client_id: "snapGameApiDevSwagger",
            authorization: "https://localhost:52365/connect/authorize",
            token: "https://localhost:52365/connect/token",
            redirect_uri: "http://localhost:7456",
            scopes: {
                request: ["snapgame"],
            },
            response_type: "token",
            nonce: "636864884858396406.MWI4ZjNkYWItOGU1My00YmFiLTg1MTAtMWQzOTY2OTM4YzRkOGFhOGI1OGItODc0YS00NGEyLWI3NzgtYzU0YmJiMzk5NWY0"
        };
        this.gameConfig.apiUrl = "https://localhost:44378";
        this.gameConfig.gameServerUrl = "https://localhost:44378/game_notifications";
        var fetcher = {
            fetch: function (input, init) {
                return _this.oauth ? _this.oauth.fetch(input, init) : window.fetch(input, init);
            }
        };
        this.playerClient = new api_1.PlayerClient(this.gameConfig, this.gameConfig, fetcher);
        this.oauth = new SecurityManager_1.default(this.playerClient, this.gameConfig);
        this.connection = new ConnectionManager_1.ConnectionManager(this.gameConfig, this.gameConfig);
    };
    MainMenuManager.prototype.onDestroy = function () {
        cc.systemEvent
            .off(ConnectionManager_1.ConnectionManager.ON_CONNECTED, this.onConnected, this);
        cc.systemEvent
            .off(ConnectionManager_1.ConnectionManager.ON_CONNECTION_CLOSED, this.onDisconnected, this);
        cc.systemEvent
            .off(GameConfiguration_1.GameConfiguration.ON_PLAYER_DATA_UPDATED, this.onPlayerDataUpdated, this);
        cc.systemEvent
            .off(ConnectionManager_1.ConnectionManager.ON_CREATE_ROOM, this.onRoomCreated, this);
        cc.systemEvent
            .off(ConnectionManager_1.ConnectionManager.ON_JOIN_GAME_MESSAGE, this.onJoinGame, this);
    };
    MainMenuManager.prototype.onJoinGame = function (e) {
        var joinedUser = e.getUserData().username;
        var roomId = e.getUserData().roomId;
        if (joinedUser === this.gameConfig.currentPlayer.username) {
            cc.director.loadScene(this.roomSceneName, function () {
                var scene = cc.director.getScene();
                var game = scene.getChildByName("Game").getComponent(SnapGame_1.default);
                var sceneManager = scene.getChildByName("WaitingRoomManager").getComponent(WaitingRoomManager_1.default);
                game.roomId = roomId;
                sceneManager.loadData();
            });
        }
    };
    MainMenuManager.prototype.onRoomCreated = function (e) {
        var roomId = e.getUserData().roomId;
        var createdBy = e.getUserData().createdBy;
        if (createdBy === this.gameConfig.currentPlayer.username) {
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
    MainMenuManager.prototype.onPlayerDataUpdated = function (e) {
        var player = e.getUserData();
        this.loginNameLbl.string = player && player.username ? player.username : "Not singed in";
    };
    MainMenuManager.prototype.logout = function () {
        this.oauth.logout();
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