(function() {"use strict";var __module = CC_EDITOR ? module : {exports:{}};var __filename = 'preview-scripts/assets/Script/ConnectionManager.js';var __require = CC_EDITOR ? function (request) {return cc.require(request, require);} : function (request) {return cc.require(request, __filename);};function __define (exports, require, module) {"use strict";
cc._RF.push(module, '9cf07sV71xOMoCTgGD0Fp9b', 'ConnectionManager', __filename);
// Script/ConnectionManager.ts

Object.defineProperty(exports, "__esModule", { value: true });
var signalR = require("@aspnet/signalr");
var jso_1 = require("jso");
var ConnectionManager = /** @class */ (function () {
    function ConnectionManager() {
        this.connectTryCount = 0;
        this._player = null;
        this.client = new jso_1.JSO({
            providerID: "snapGameOauth",
            client_id: "snapGameApiDevSwagger",
            authorization: "https://localhost:52365/connect/authorize",
            token: "https://localhost:52365/connect/token",
            redirect_uri: "http://localhost:7456",
            debug: true,
            scopes: {
                request: ["snapgame"],
            },
            response_type: "token",
            request: {
                // tslint:disable-next-line:max-line-length
                nonce: "636864884858396406.MWI4ZjNkYWItOGU1My00YmFiLTg1MTAtMWQzOTY2OTM4YzRkOGFhOGI1OGItODc0YS00NGEyLWI3NzgtYzU0YmJiMzk5NWY0"
            }
        });
    }
    ConnectionManager.getInstance = function () {
        return ConnectionManager.conn;
    };
    Object.defineProperty(ConnectionManager.prototype, "player", {
        get: function () {
            return this._player;
        },
        set: function (player) {
            this._player = player;
            var eventType = this._player ? ConnectionManager.ON_PLAYER_LOGIN_EVENT :
                ConnectionManager.ON_PLAYER_LOGOUT;
            this.dispatch(eventType, this._player);
        },
        enumerable: true,
        configurable: true
    });
    ConnectionManager.prototype.refresh = function () {
        var _this = this;
        return this.login().then(function () {
            return _this.connect().catch(function (e) {
                cc.error(e);
                throw e;
            });
        });
    };
    ConnectionManager.prototype.login = function () {
        var _this = this;
        this.client.callback();
        cc.log("Trying to loggin");
        this.dispatch(ConnectionManager.ON_CHALLENGING, {});
        return this.client.getToken()
            .then(function (token) {
            _this.token = token;
            cc.log("I got the token: ", _this.token);
            var fetcher = new jso_1.Fetcher(_this.client);
            var url = "https://localhost:44378/api/Player/me";
            fetcher.fetch(url, {})
                .then(function (data) {
                return data.json();
            })
                .then(function (data) {
                cc.log("Player ifno recieved from the server " + data);
                _this.player = data;
            })
                .catch(function (err) {
                cc.error("Error getting user data: ", err);
                throw err;
            });
        })
            .catch(function (err) {
            cc.error("Error from getToken: ", err);
            throw err;
        });
    };
    ConnectionManager.prototype.logout = function () {
        this.client.wipeTokens();
        this.player = null;
    };
    ConnectionManager.prototype.connect = function () {
        var _this = this;
        if (!this.connection) {
            var builder = new signalR.HubConnectionBuilder()
                .configureLogging(signalR.LogLevel.Information)
                .withUrl(ConnectionManager.ENPOINT, {
                accessTokenFactory: function () {
                    return !_this.token ? "" : _this.token.access_token;
                }
            });
            this.connection = builder.build();
            this.connection.onclose(function (err) {
                _this.dispatch(ConnectionManager.ON_CONNECTION_CLOSED, err);
                // this.connect();
                // this.connectionStatusLbl.string = "Disconnected";
                // this.reconnectBtn.node.getChildByName("Label").getComponent(cc.Label).string = "Reconnect";
                // cc.error(err);
            });
        }
        if (this.connection.state === signalR.HubConnectionState.Connected) {
            return new Promise(function () {
                cc.log("You are already connected");
                return;
            });
        }
        cc.log("Connecting");
        this.dispatch(ConnectionManager.ON_CONNECTING, {});
        this.connectTryCount++;
        if (this.connectTryCount > 10) {
            throw "maxReconnectFailed";
        }
        return this.connection.start().then(function (c) {
            _this.connectTryCount = 0;
            _this.connection
                .on(ConnectionManager.CREATE_ROOM, function (message) {
                cc.log(ConnectionManager.CREATE_ROOM + " received: " + message);
                _this.dispatch(ConnectionManager.ON_CREATE_ROOM, message);
            });
            _this.connection
                .on(ConnectionManager.JOIN_GAME_MESSAGE, function (message) {
                cc.log(ConnectionManager.JOIN_GAME_MESSAGE + " received: " + message);
                _this.dispatch(ConnectionManager.ON_JOIN_GAME_MESSAGE, message);
            });
            _this.connection
                .on(ConnectionManager.START_GAME_MESSAGE, function (message) {
                cc.log(ConnectionManager.START_GAME_MESSAGE + " received: " + message);
                _this.dispatch(ConnectionManager.ON_START_GAME_MESSAGE, message);
            });
            _this.connection
                .on(ConnectionManager.POP_CARD_MESSAGE, function (message) {
                cc.log(ConnectionManager.POP_CARD_MESSAGE + " received: " + message);
                _this.dispatch(ConnectionManager.ON_POP_CARD_MESSAGE, message);
            });
            _this.connection
                .on(ConnectionManager.SNAP_MESSAGE, function (message) {
                cc.log(ConnectionManager.SNAP_MESSAGE + " received: " + message);
                _this.dispatch(ConnectionManager.ON_SNAP_MESSAGE, message);
            });
            _this.dispatch(ConnectionManager.ON_CONNECTED, {});
            cc.log("Connection stablished!");
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
    ConnectionManager.ON_CHALLENGING = "onChallenging";
    ConnectionManager.ON_PLAYER_LOGIN_EVENT = "onPlayerLoginEvent";
    ConnectionManager.ON_PLAYER_LOGOUT = "onPlayerLogoutEvent";
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
    ConnectionManager.ENPOINT = "https://localhost:44378/game_notifications";
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
        