(function() {"use strict";var __module = CC_EDITOR ? module : {exports:{}};var __filename = 'preview-scripts/assets/Script/api/clients/GameRoomClient.js';var __require = CC_EDITOR ? function (request) {return cc.require(request, require);} : function (request) {return cc.require(request, __filename);};function __define (exports, require, module) {"use strict";
cc._RF.push(module, '8e740EaJklPRoAdalYYCS9W', 'GameRoomClient', __filename);
// Script/api/clients/GameRoomClient.ts

Object.defineProperty(exports, "__esModule", { value: true });
var GameRoom_1 = require("../models/GameRoom");
var SwaggerException_1 = require("./SwaggerException");
var GameRoomClient = /** @class */ (function () {
    function GameRoomClient(baseUrl, http) {
        this.jsonParseReviver = undefined;
        this.http = http ? http : window;
        this.baseUrl = baseUrl ? baseUrl : "https://localhost:44378";
    }
    GameRoomClient.prototype.getAll = function () {
        var _this = this;
        var url_ = (this.baseUrl + "/api/GameRoom").replace(/[?&]$/, "");
        var options_ = {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };
        return this.http.fetch(url_, options_).then(function (_response) {
            return _this.processGetAll(_response);
        });
    };
    GameRoomClient.prototype.processGetAll = function (response) {
        var _this = this;
        var status = response.status;
        var _headers = {};
        if (response.headers && response.headers.forEach) {
            response.headers.forEach(function (v, k) { return _headers[k] = v; });
        }
        if (status === 200) {
            return response.text().then(function (_responseText) {
                var result200 = null;
                var resultData200 = _responseText === "" ? null : JSON.parse(_responseText, _this.jsonParseReviver);
                if (resultData200 && resultData200.constructor === Array) {
                    result200 = [];
                    for (var _i = 0, resultData200_1 = resultData200; _i < resultData200_1.length; _i++) {
                        var item = resultData200_1[_i];
                        result200.push(GameRoom_1.GameRoom.fromJS(item));
                    }
                }
                return result200;
            });
        }
        else if (status !== 200 && status !== 204) {
            return response.text().then(function (_responseText) {
                return SwaggerException_1.throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve(null);
    };
    GameRoomClient.prototype.get = function (id) {
        var _this = this;
        var url_ = this.baseUrl + "/api/GameRoom/{id}";
        if (id === undefined || id === null) {
            throw new Error("The parameter 'id' must be defined.");
        }
        url_ = url_.replace("{id}", encodeURIComponent("" + id));
        url_ = url_.replace(/[?&]$/, "");
        var options_ = {
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };
        return this.http.fetch(url_, options_).then(function (_response) {
            return _this.processGet(_response);
        });
    };
    GameRoomClient.prototype.processGet = function (response) {
        var _this = this;
        var status = response.status;
        var _headers = {};
        if (response.headers && response.headers.forEach) {
            response.headers.forEach(function (v, k) { return _headers[k] = v; });
        }
        if (status === 200) {
            return response.text().then(function (_responseText) {
                var result200 = null;
                var resultData200 = _responseText === "" ? null : JSON.parse(_responseText, _this.jsonParseReviver);
                result200 = resultData200 ? GameRoom_1.GameRoom.fromJS(resultData200) : null;
                return result200;
            });
        }
        else if (status !== 200 && status !== 204) {
            return response.text().then(function (_responseText) {
                return SwaggerException_1.throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve(null);
    };
    return GameRoomClient;
}());
exports.GameRoomClient = GameRoomClient;

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
        //# sourceMappingURL=GameRoomClient.js.map
        