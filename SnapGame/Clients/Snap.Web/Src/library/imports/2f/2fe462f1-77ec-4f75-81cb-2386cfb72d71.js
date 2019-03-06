"use strict";
cc._RF.push(module, '2fe46Lxd+xPdYHLI4bPty1x', 'SwaggerException');
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