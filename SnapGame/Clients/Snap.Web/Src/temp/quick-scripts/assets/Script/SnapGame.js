(function() {"use strict";var __module = CC_EDITOR ? module : {exports:{}};var __filename = 'preview-scripts/assets/Script/SnapGame.js';var __require = CC_EDITOR ? function (request) {return cc.require(request, require);} : function (request) {return cc.require(request, __filename);};function __define (exports, require, module) {"use strict";
cc._RF.push(module, 'a5751EyGXpFb7z8Z+Jt9PZr', 'SnapGame', __filename);
// Script/SnapGame.ts

"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var _a = cc._decorator, ccclass = _a.ccclass, property = _a.property;
var SnapGame = /** @class */ (function (_super) {
    __extends(SnapGame, _super);
    function SnapGame() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.roomId = -1;
        _this.createdBy = "";
        _this.gameId = -1;
        _this.currentPlayer = "";
        _this.playerData = {};
        return _this;
    }
    SnapGame = __decorate([
        ccclass
    ], SnapGame);
    return SnapGame;
}(cc.Component));
exports.default = SnapGame;

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
        //# sourceMappingURL=SnapGame.js.map
        