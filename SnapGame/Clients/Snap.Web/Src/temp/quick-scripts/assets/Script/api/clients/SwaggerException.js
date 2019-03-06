(function() {"use strict";var __module = CC_EDITOR ? module : {exports:{}};var __filename = 'preview-scripts/assets/Script/api/clients/SwaggerException.js';var __require = CC_EDITOR ? function (request) {return cc.require(request, require);} : function (request) {return cc.require(request, __filename);};function __define (exports, require, module) {"use strict";
cc._RF.push(module, '2fe46Lxd+xPdYHLI4bPty1x', 'SwaggerException', __filename);
// Script/api/clients/SwaggerException.ts

Object.defineProperty(exports, "__esModule", { value: true });
var SwaggerException = /** @class */ (function (_super) {
    __extends(SwaggerException, _super);
    function SwaggerException(message, status, response, headers, result) {
        var _this = _super.call(this) || this;
        _this.isSwaggerException = true;
        _this.message = message;
        _this.status = status;
        _this.response = response;
        _this.headers = headers;
        _this.result = result;
        return _this;
    }
    SwaggerException.isSwaggerException = function (obj) {
        return obj.isSwaggerException === true;
    };
    return SwaggerException;
}(Error));
exports.SwaggerException = SwaggerException;
function throwException(message, status, response, headers, result) {
    if (result !== null && result !== undefined) {
        throw result;
    }
    else {
        throw new SwaggerException(message, status, response, headers, null);
    }
}
exports.throwException = throwException;

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
        //# sourceMappingURL=SwaggerException.js.map
        