(function() {"use strict";var __module = CC_EDITOR ? module : {exports:{}};var __filename = 'preview-scripts/assets/Script/api/models/index.js';var __require = CC_EDITOR ? function (request) {return cc.require(request, require);} : function (request) {return cc.require(request, __filename);};function __define (exports, require, module) {"use strict";
cc._RF.push(module, '6975bY1zCFNu6pQrlt/lO2l', 'index', __filename);
// Script/api/models/index.ts

function __export(m) {
    for (var p in m) if (!exports.hasOwnProperty(p)) exports[p] = m[p];
}
Object.defineProperty(exports, "__esModule", { value: true });
__export(require("./GameData"));
__export(require("./GameRoom"));
__export(require("./GameRoomPlayer"));
__export(require("./GameState"));
__export(require("./IGameData"));
__export(require("./IGameRoom"));
__export(require("./IGameRoomPlayer"));
__export(require("./IPlayer"));
__export(require("./IPlayerTurn"));
__export(require("./Player"));
__export(require("./PlayerTurn"));

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
        //# sourceMappingURL=index.js.map
        