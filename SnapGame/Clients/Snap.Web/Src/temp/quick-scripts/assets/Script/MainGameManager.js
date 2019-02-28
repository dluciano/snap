(function() {"use strict";var __module = CC_EDITOR ? module : {exports:{}};var __filename = 'preview-scripts/assets/Script/MainGameManager.js';var __require = CC_EDITOR ? function (request) {return cc.require(request, require);} : function (request) {return cc.require(request, __filename);};function __define (exports, require, module) {"use strict";
cc._RF.push(module, 'b3ccfocBzlMopwIcI0F0vp0', 'MainGameManager', __filename);
// Script/MainGameManager.ts

"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var _a = cc._decorator, ccclass = _a.ccclass, property = _a.property;
var ConnectionManager_1 = require("./ConnectionManager");
var SnapGame_1 = require("./SnapGame");
var MainGameManager = /** @class */ (function (_super) {
    __extends(MainGameManager, _super);
    function MainGameManager() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.snapGameNode = null;
        _this.cardPrefab = null;
        _this.menuSceneName = "";
        _this.cardContainer = null;
        _this.yourCards = null;
        _this.popCardTouchNode = null;
        _this.snapTouchNode = null;
        _this.currentPlayerLabel = null;
        _this.cardAtlas = null;
        _this.cardBackPart = null;
        _this.playerNamesContainer = null;
        _this.rotateDirection = 1;
        return _this;
    }
    MainGameManager.prototype.onLoad = function () {
        this.connection = ConnectionManager_1.default.getInstance();
        cc.systemEvent
            .on(ConnectionManager_1.default.ON_POP_CARD_MESSAGE, this.cardPop, this);
        cc.systemEvent
            .on(ConnectionManager_1.default.ON_SNAP_MESSAGE, this.onSnap, this);
        this.snapTouchNode.on("mousedown", this.snap, this);
        this.popCardTouchNode.on("mousedown", this.popCard, this);
        this.game = this.snapGameNode.getComponent(SnapGame_1.default);
    };
    MainGameManager.prototype.onDestroy = function () {
        cc.systemEvent
            .off(ConnectionManager_1.default.ON_POP_CARD_MESSAGE, this.cardPop, this);
        cc.systemEvent
            .on(ConnectionManager_1.default.ON_SNAP_MESSAGE, this.onSnap, this);
    };
    MainGameManager.prototype.cardPop = function (e) {
        var data = e.getUserData();
        this.game.currentPlayer = data.currentPlayer;
        var card = data.card;
        cc.log("You are: " + this.connection.player.username + " and the current player is: " + this.game.currentPlayer);
        cc.log("card pop! type: " + card.type + " value: " + card.value);
        this.renderPopCardInCentralPile(card.type, card.value);
        this.currentPlayerLabel.string = this.game.currentPlayer;
        if (data.popBy === this.connection.player.username && this.yourCards.children.length > 0) {
            this.yourCards.removeChild(this.yourCards.children[0], true);
        }
    };
    MainGameManager.prototype.renderPopCardInCentralPile = function (type, value) {
        var cardNode = cc.instantiate(this.cardPrefab);
        var cardSprite = cardNode.getComponent(cc.Sprite);
        var atlasSliceName = "slice_" + (type + 1) + "_" + value;
        cardNode.angle = this.rotateDirection * Math.random() * 30;
        cardSprite.spriteFrame = this.cardAtlas.getSpriteFrame(atlasSliceName);
        this.cardContainer.addChild(cardNode);
        this.rotateDirection = this.rotateDirection >= 1 ? -1 : 1;
    };
    MainGameManager.prototype.snap = function () {
        this.connection.snap(this.game.gameId);
    };
    MainGameManager.prototype.onSnap = function (e) {
        var data = e.getUserData();
        if (data.username === this.connection.player.username) {
            cc.log("YOU " + data.username + " HAVE SNAPPED");
            this.yourCards.removeAllChildren();
            for (var i = 0; i < data.playerCardsCount; i++) {
                var backCard = cc.instantiate(this.cardPrefab);
                backCard.setPosition(new cc.Vec2(i * 0.01, i * 0.01));
                this.yourCards.addChild(backCard);
            }
        }
        else {
            cc.log("YEEKS, player " + data.username + " hava snapped :(");
        }
        this.cardContainer.removeAllChildren();
    };
    MainGameManager.prototype.loadData = function () {
        var _this = this;
        cc.log("You are: " + this.connection.player.username + " and the current player is: " + this.game.currentPlayer);
        this.currentPlayerLabel.string = this.game.currentPlayer;
        var myPlayerData = this.game.playerData.filter(function (p) { return p.username === _this.connection.player.username; })[0];
        for (var i = 0; i < myPlayerData.playersCardsCount; i++) {
            var backCard = cc.instantiate(this.cardPrefab);
            backCard.setPosition(new cc.Vec2(i * 0.25, i * 0.25));
            this.yourCards.addChild(backCard);
        }
        this.game.playerData.map(function (pd) {
            var node = new cc.Node();
            var label = node.addComponent(cc.Label);
            label.string = pd.username;
            label.fontSize = 24;
            label.lineHeight = 24;
            return node;
        }).forEach(function (l) { return _this.playerNamesContainer.addChild(l); });
    };
    MainGameManager.prototype.popCard = function () {
        this.connection.popCard(this.game.gameId);
    };
    MainGameManager.prototype.exit = function () {
        cc.director.loadScene(this.menuSceneName);
    };
    __decorate([
        property(cc.Node)
    ], MainGameManager.prototype, "snapGameNode", void 0);
    __decorate([
        property(cc.Prefab)
    ], MainGameManager.prototype, "cardPrefab", void 0);
    __decorate([
        property()
    ], MainGameManager.prototype, "menuSceneName", void 0);
    __decorate([
        property(cc.Node)
    ], MainGameManager.prototype, "cardContainer", void 0);
    __decorate([
        property(cc.Node)
    ], MainGameManager.prototype, "yourCards", void 0);
    __decorate([
        property(cc.Node)
    ], MainGameManager.prototype, "popCardTouchNode", void 0);
    __decorate([
        property(cc.Node)
    ], MainGameManager.prototype, "snapTouchNode", void 0);
    __decorate([
        property(cc.Label)
    ], MainGameManager.prototype, "currentPlayerLabel", void 0);
    __decorate([
        property(cc.SpriteAtlas)
    ], MainGameManager.prototype, "cardAtlas", void 0);
    __decorate([
        property(cc.SpriteFrame)
    ], MainGameManager.prototype, "cardBackPart", void 0);
    __decorate([
        property(cc.Node)
    ], MainGameManager.prototype, "playerNamesContainer", void 0);
    MainGameManager = __decorate([
        ccclass
    ], MainGameManager);
    return MainGameManager;
}(cc.Component));
exports.default = MainGameManager;

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
        //# sourceMappingURL=MainGameManager.js.map
        