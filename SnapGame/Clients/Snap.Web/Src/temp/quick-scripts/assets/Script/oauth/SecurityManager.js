(function() {"use strict";var __module = CC_EDITOR ? module : {exports:{}};var __filename = 'preview-scripts/assets/Script/oauth/SecurityManager.js';var __require = CC_EDITOR ? function (request) {return cc.require(request, require);} : function (request) {return cc.require(request, __filename);};function __define (exports, require, module) {"use strict";
cc._RF.push(module, '1bdb2Q7gxVEkJbua46sZKZW', 'SecurityManager', __filename);
// Script/oauth/SecurityManager.ts

"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var jso_1 = require("jso");
var JsoSecurityManager = /** @class */ (function () {
    function JsoSecurityManager(playerClient, securityConfig) {
        var _this = this;
        this.playerClient = playerClient;
        this.securityConfig = securityConfig;
        this.logout = function () {
            if (!_this.client) {
                return;
            }
            _this.client.wipeTokens();
            _this.securityConfig.currentPlayer = null;
        };
    }
    JsoSecurityManager.prototype.login = function () {
        return __awaiter(this, void 0, Promise, function () {
            var oautConfig, _a, _b, error_1, error_2;
            return __generator(this, function (_c) {
                switch (_c.label) {
                    case 0:
                        oautConfig = this.securityConfig.oauthConfiguration;
                        this.client = new jso_1.JSO({
                            providerID: oautConfig.providerID,
                            client_id: oautConfig.client_id,
                            authorization: oautConfig.authorization,
                            token: oautConfig.token,
                            redirect_uri: oautConfig.redirect_uri,
                            debug: true,
                            scopes: oautConfig.scopes,
                            response_type: oautConfig.response_type,
                            request: {
                                nonce: oautConfig.nonce
                            }
                        });
                        return [4 /*yield*/, this.client.callback()];
                    case 1:
                        _c.sent();
                        cc.log("Loggin started");
                        this.dispatch(JsoSecurityManager.ON_CHALLENGING, {});
                        _c.label = 2;
                    case 2:
                        _c.trys.push([2, 8, , 9]);
                        _a = this.securityConfig;
                        return [4 /*yield*/, this.client.getToken()];
                    case 3:
                        _a.currentToken = _c.sent();
                        cc.log("I got the token: ", this.securityConfig.currentToken);
                        _c.label = 4;
                    case 4:
                        _c.trys.push([4, 6, , 7]);
                        _b = this.securityConfig;
                        return [4 /*yield*/, this.playerClient.post()];
                    case 5: return [2 /*return*/, _b.currentPlayer = _c.sent()];
                    case 6:
                        error_1 = _c.sent();
                        cc.log("Error recieving Player info: " + error_1);
                        throw error_1;
                    case 7: return [3 /*break*/, 9];
                    case 8:
                        error_2 = _c.sent();
                        cc.error("Error from getToken: ", error_2);
                        throw error_2;
                    case 9: return [2 /*return*/];
                }
            });
        });
    };
    JsoSecurityManager.prototype.fetch = function (input, init) {
        return this.client ? new jso_1.Fetcher(this.client).fetch(input, init) :
            window.fetch(input, init);
    };
    JsoSecurityManager.prototype.dispatch = function (type, data) {
        var e = new cc.Event.EventCustom(type, true);
        e.target = this;
        e.setUserData(data);
        cc.systemEvent.dispatchEvent(e);
    };
    JsoSecurityManager.ON_CHALLENGING = "onChallenging";
    return JsoSecurityManager;
}());
exports.JsoSecurityManager = JsoSecurityManager;
exports.default = JsoSecurityManager;

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
        //# sourceMappingURL=SecurityManager.js.map
        