(function() {"use strict";var __module = CC_EDITOR ? module : {exports:{}};var __filename = 'preview-scripts/assets/Script/api/models/GameState.js';var __require = CC_EDITOR ? function (request) {return cc.require(request, require);} : function (request) {return cc.require(request, __filename);};function __define (exports, require, module) {"use strict";
cc._RF.push(module, '6c399TBLd1IebiCYRWQv0h3', 'GameState', __filename);
// Script/api/generated/GameState.ts

Object.defineProperty(exports, "__esModule", { value: true });
var GameState;
(function (GameState) {
    GameState[GameState["NONE"] = 0] = "NONE";
    GameState[GameState["PLAYING"] = 1] = "PLAYING";
    GameState[GameState["FINISHED"] = 2] = "FINISHED";
    GameState[GameState["ABORTED"] = 3] = "ABORTED";
})(GameState = exports.GameState || (exports.GameState = {}));

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
        //# sourceMappingURL=GameState.js.map
        