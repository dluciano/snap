"use strict";
cc._RF.push(module, 'c08569tFsJD4IL4dl1t47SV', 'ItemRoomScrollView');
// Prefabs/ItemRoomScrollView.ts

"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
require("../Script/ConnectionManager");
var ConnectionManager_1 = require("../Script/ConnectionManager");
var _a = cc._decorator, ccclass = _a.ccclass, property = _a.property;
var ItemRoomScrollView = /** @class */ (function (_super) {
    __extends(ItemRoomScrollView, _super);
    function ItemRoomScrollView() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.id = -1;
        _this.roomSceneName = "";
        return _this;
    }
    ItemRoomScrollView.prototype.onLoad = function () {
        var _this = this;
        this.node.on("mousedown", function (event) {
            ConnectionManager_1.default.getInstance().joinGame(_this.id, false);
        });
    };
    __decorate([
        property()
    ], ItemRoomScrollView.prototype, "roomSceneName", void 0);
    ItemRoomScrollView = __decorate([
        ccclass
    ], ItemRoomScrollView);
    return ItemRoomScrollView;
}(cc.Component));
exports.default = ItemRoomScrollView;

cc._RF.pop();