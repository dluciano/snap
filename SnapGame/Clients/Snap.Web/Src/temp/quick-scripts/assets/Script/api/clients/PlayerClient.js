(function() {"use strict";var __module = CC_EDITOR ? module : {exports:{}};var __filename = 'preview-scripts/assets/Script/api/clients/PlayerClient.js';var __require = CC_EDITOR ? function (request) {return cc.require(request, require);} : function (request) {return cc.require(request, __filename);};function __define (exports, require, module) {"use strict";
cc._RF.push(module, '8d99chNBN9Hy50FLFCvJaXf', 'PlayerClient', __filename);
// Script/api/clients/PlayerClient.ts

"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Player_1 = require("../models/Player");
var _1 = require(".");
var PlayerClient = /** @class */ (function () {
    function PlayerClient(apiConfig, securityConfig, fetcher) {
        var _this = this;
        this.apiConfig = apiConfig;
        this.securityConfig = securityConfig;
        this.fetcher = fetcher;
        this.jsonParseReviver = undefined;
        this.get = function (action) { return __awaiter(_this, void 0, Promise, function () {
            var url_, options_;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        url_ = (this.apiConfig.apiUrl +
                            this.apiConfig.playersEnpoint +
                            action).replace(/[?&]$/, "");
                        options_ = {
                            method: "GET",
                            headers: {
                                "Accept": "application/json",
                                "Authentication": "Bearer " + this.securityConfig.currentToken.access_token
                            }
                        };
                        return [4 /*yield*/, this.fetcher.fetch(url_, options_)];
                    case 1: return [2 /*return*/, _a.sent()];
                }
            });
        }); };
        this.post_ = function () { return __awaiter(_this, void 0, Promise, function () {
            var url_, options_;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        url_ = (this.apiConfig.apiUrl +
                            this.apiConfig.playersEnpoint).replace(/[?&]$/, "");
                        options_ = {
                            method: "POST",
                            headers: {
                                "Accept": "application/json",
                                "Authentication": "Bearer " + this.securityConfig.currentToken.access_token
                            }
                        };
                        return [4 /*yield*/, this.fetcher.fetch(url_, options_)];
                    case 1: return [2 /*return*/, _a.sent()];
                }
            });
        }); };
        this.post = function () { return __awaiter(_this, void 0, Promise, function () {
            var response, player;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.post_()];
                    case 1:
                        response = _a.sent();
                        return [4 /*yield*/, this.responseToEntity(response)];
                    case 2:
                        player = _a.sent();
                        return [2 /*return*/, player];
                }
            });
        }); };
        this.responseToEntity = function (response) { return __awaiter(_this, void 0, Promise, function () {
            var status, _headers, _responseText, resultData200, data, entity, _responseText;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        status = response.status;
                        _headers = {};
                        if (response.headers && response.headers.forEach) {
                            response.headers.forEach(function (v, k) { return _headers[k] = v; });
                        }
                        if (!(status === 200)) return [3 /*break*/, 2];
                        return [4 /*yield*/, response.text()];
                    case 1:
                        _responseText = _a.sent();
                        resultData200 = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                        data = typeof resultData200 === "object" ? resultData200 : {};
                        entity = new Player_1.Player();
                        entity.init(data);
                        return [2 /*return*/, entity];
                    case 2:
                        if (!(status !== 200 && status !== 204)) return [3 /*break*/, 4];
                        return [4 /*yield*/, response.text()];
                    case 3:
                        _responseText = _a.sent();
                        return [2 /*return*/, _1.throwException("An unexpected server error occurred.", status, _responseText, _headers)];
                    case 4: return [2 /*return*/, Promise.resolve(null)];
                }
            });
        }); };
    }
    PlayerClient.prototype.me = function () {
        return __awaiter(this, void 0, Promise, function () {
            var _response;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0: return [4 /*yield*/, this.get("/me")];
                    case 1:
                        _response = _a.sent();
                        return [4 /*yield*/, this.responseToEntity(_response)];
                    case 2: return [2 /*return*/, _a.sent()];
                }
            });
        });
    };
    return PlayerClient;
}());
exports.PlayerClient = PlayerClient;

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
        //# sourceMappingURL=PlayerClient.js.map
        