(function() {"use strict";var __module = CC_EDITOR ? module : {exports:{}};var __filename = 'preview-scripts/assets/Script/ConnectionManager.js';var __require = CC_EDITOR ? function (request) {return cc.require(request, require);} : function (request) {return cc.require(request, __filename);};function __define (exports, require, module) {"use strict";
cc._RF.push(module, '9cf07sV71xOMoCTgGD0Fp9b', 'ConnectionManager', __filename);
// Script/ConnectionManager.ts

Object.defineProperty(exports, "__esModule", { value: true });
var signalR = require("@aspnet/signalr");
var ConnectionManager = /** @class */ (function () {
    function ConnectionManager(serverConfig, securityConfig) {
        this.serverConfig = serverConfig;
        this.securityConfig = securityConfig;
        this.connectTryCount = 0;
    }
    ConnectionManager.getInstance = function () {
        return ConnectionManager.conn;
    };
    ConnectionManager.prototype.connect = function () {
        return __awaiter(this, void 0, Promise, function () {
            var builder;
            var _this = this;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        if (!this.connection) {
                            builder = new signalR.HubConnectionBuilder()
                                .configureLogging(signalR.LogLevel.Information)
                                .withUrl(this.serverConfig.gameServerUrl, {
                                accessTokenFactory: function () {
                                    return _this.securityConfig.currentToken.access_token;
                                }
                            });
                            this.connection = builder.build();
                            this.connection.onclose(function (err) {
                                _this.dispatch(ConnectionManager.ON_CONNECTION_CLOSED, err);
                            });
                            this.connection
                                .on(ConnectionManager.CREATE_ROOM, function (message) {
                                cc.log(ConnectionManager.CREATE_ROOM + " received: " + message);
                                _this.dispatch(ConnectionManager.ON_CREATE_ROOM, message);
                            });
                            this.connection
                                .on(ConnectionManager.JOIN_GAME_MESSAGE, function (message) {
                                cc.log(ConnectionManager.JOIN_GAME_MESSAGE + " received: " + message);
                                _this.dispatch(ConnectionManager.ON_JOIN_GAME_MESSAGE, message);
                            });
                            this.connection
                                .on(ConnectionManager.START_GAME_MESSAGE, function (message) {
                                cc.log(ConnectionManager.START_GAME_MESSAGE + " received: " + message);
                                _this.dispatch(ConnectionManager.ON_START_GAME_MESSAGE, message);
                            });
                            this.connection
                                .on(ConnectionManager.POP_CARD_MESSAGE, function (message) {
                                cc.log(ConnectionManager.POP_CARD_MESSAGE + " received: " + message);
                                _this.dispatch(ConnectionManager.ON_POP_CARD_MESSAGE, message);
                            });
                            this.connection
                                .on(ConnectionManager.SNAP_MESSAGE, function (message) {
                                cc.log(ConnectionManager.SNAP_MESSAGE + " received: " + message);
                                _this.dispatch(ConnectionManager.ON_SNAP_MESSAGE, message);
                            });
                        }
                        if (this.connection.state === signalR.HubConnectionState.Connected) {
                            return [2 /*return*/, new Promise(function () {
                                    cc.log("You are already connected");
                                    return;
                                })];
                        }
                        cc.log("Connecting");
                        this.dispatch(ConnectionManager.ON_CONNECTING, {});
                        this.connectTryCount++;
                        if (this.connectTryCount > 10) {
                            throw "maxReconnectFailed";
                        }
                        return [4 /*yield*/, this.connection.start()];
                    case 1:
                        _a.sent();
                        this.connectTryCount = 0;
                        this.dispatch(ConnectionManager.ON_CONNECTED, {});
                        cc.log("Connection stablished!");
                        return [2 /*return*/];
                }
            });
        });
    };
    ConnectionManager.prototype.createRoom = function () {
        this.validateConnected();
        return this.connection.send(ConnectionManager.CREATE_ROOM).then(function (data) {
            cc.log(data);
        }).catch(function (e) {
            cc.error(e);
            throw e;
        });
    };
    ConnectionManager.prototype.startGame = function (roomId) {
        this.validateConnected();
        return this.connection.send(ConnectionManager.START_GAME_MESSAGE, roomId).then(function (data) {
            cc.log("Message " + ConnectionManager.START_GAME_MESSAGE + " sent");
        }).catch(function (err) {
            cc.error(err);
            throw err;
        });
    };
    ConnectionManager.prototype.joinGame = function (roomId, isViewer) {
        if (isViewer === void 0) { isViewer = false; }
        this.validateConnected();
        return this.connection.send(ConnectionManager.JOIN_GAME_MESSAGE, roomId, isViewer)
            .then(function (data) {
            cc.log("Message " + ConnectionManager.JOIN_GAME_MESSAGE + " sent");
        }).catch(function (err) {
            cc.error(err);
            throw err;
        });
    };
    ConnectionManager.prototype.popCard = function (gameId) {
        this.validateConnected();
        return this.connection.send(ConnectionManager.POP_CARD_MESSAGE, gameId).then(function (data) {
            cc.log("Message " + ConnectionManager.POP_CARD_MESSAGE + " sent");
        }).catch(function (err) {
            cc.error(err);
            throw err;
        });
    };
    ConnectionManager.prototype.snap = function (gameId) {
        this.validateConnected();
        return this.connection.send(ConnectionManager.SNAP_MESSAGE, gameId).then(function (data) {
            cc.log("Message " + ConnectionManager.POP_CARD_MESSAGE + " sent");
        }).catch(function (err) {
            cc.error(err);
            throw err;
        });
    };
    ConnectionManager.prototype.validateConnected = function () {
        if (this.connection.state === signalR.HubConnectionState.Disconnected) {
            cc.error("You should be connected to create a room");
            throw "You are discconected";
        }
        return;
    };
    ConnectionManager.prototype.dispatch = function (type, data) {
        var e = new cc.Event.EventCustom(type, true);
        e.target = this;
        e.setUserData(data);
        cc.director.dispatchEvent(e);
        cc.systemEvent.dispatchEvent(e);
    };
    ConnectionManager.conn = new ConnectionManager();
    ConnectionManager.ON_CONNECTING = "onConnecting";
    ConnectionManager.ON_CONNECTION_CLOSED = "onConnectionClosed";
    ConnectionManager.ON_CONNECTED = "onConnected";
    ConnectionManager.CREATE_ROOM = "CreateRoom";
    ConnectionManager.JOIN_GAME_MESSAGE = "JoinGame";
    ConnectionManager.START_GAME_MESSAGE = "StartGame";
    ConnectionManager.POP_CARD_MESSAGE = "PopCard";
    ConnectionManager.SNAP_MESSAGE = "Snap";
    ConnectionManager.ON_CREATE_ROOM = "OnCreateRoom";
    ConnectionManager.ON_POP_CARD_MESSAGE = "OnJoinGame";
    ConnectionManager.ON_START_GAME_MESSAGE = "OnStartGame";
    ConnectionManager.ON_JOIN_GAME_MESSAGE = "OnPopCard";
    ConnectionManager.ON_SNAP_MESSAGE = "OnPopCard";
    return ConnectionManager;
}());
exports.ConnectionManager = ConnectionManager;
exports.default = ConnectionManager;

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
        //# sourceMappingURL=ConnectionManager.js.map
        