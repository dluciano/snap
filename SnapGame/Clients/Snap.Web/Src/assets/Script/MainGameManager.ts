"use strict";

const { ccclass, property } = cc._decorator;
import ConnectionManager from "./ConnectionManager";
import SnapGame from "./SnapGame";

@ccclass
export default class MainGameManager extends cc.Component {
    private connection: ConnectionManager;
    private game: SnapGame;

    @property(cc.Node)
    snapGameNode: cc.Node = null;

    @property(cc.Prefab)
    cardPrefab: cc.Prefab = null;

    @property()
    menuSceneName: string = "";

    @property(cc.Node)
    cardContainer: cc.Node = null;

    @property(cc.Node)
    yourCards: cc.Node = null;

    @property(cc.Node)
    popCardTouchNode: cc.Node = null;

    @property(cc.Node)
    snapTouchNode: cc.Node = null;

    @property(cc.Label)
    currentPlayerLabel: cc.Label = null;

    @property(cc.SpriteAtlas)
    cardAtlas: cc.SpriteAtlas = null;

    @property(cc.SpriteFrame)
    cardBackPart: cc.SpriteFrame = null;

    @property(cc.Node)
    playerNamesContainer: cc.Node = null;

    rotateDirection: number = 1;

    onLoad(): void {
        this.connection = ConnectionManager.getInstance();
        cc.systemEvent
            .on(ConnectionManager.ON_POP_CARD_MESSAGE, this.cardPop, this);
        cc.systemEvent
            .on(ConnectionManager.ON_SNAP_MESSAGE, this.onSnap, this);
        this.snapTouchNode.on("mousedown", this.snap, this);
        this.popCardTouchNode.on("mousedown", this.popCard, this);
        this.game = this.snapGameNode.getComponent(SnapGame);
    }

    onDestroy(): void {
        cc.systemEvent
            .off(ConnectionManager.ON_POP_CARD_MESSAGE, this.cardPop, this);
        cc.systemEvent
            .on(ConnectionManager.ON_SNAP_MESSAGE, this.onSnap, this);
    }

    private cardPop(e: cc.Event.EventCustom): any {
        const data: any = e.getUserData();
        this.game.currentPlayer = data.currentPlayer;
        const card: any = data.card;

        cc.log(`You are: ${this.connection.player.username} and the current player is: ${this.game.currentPlayer}`);
        cc.log(`card pop! type: ${card.type} value: ${card.value}`);

        this.renderPopCardInCentralPile(card.type, card.value);
        this.currentPlayerLabel.string = this.game.currentPlayer;
        if (data.popBy === this.connection.player.username && this.yourCards.children.length > 0) {
            this.yourCards.removeChild(this.yourCards.children[0], true);
        }
    }

    renderPopCardInCentralPile(type: number, value: number): void {
        const cardNode: cc.Node = cc.instantiate(this.cardPrefab);
        const cardSprite: cc.Sprite = cardNode.getComponent(cc.Sprite);
        const atlasSliceName: string = `slice_${type + 1}_${value}`;
        cardNode.angle = this.rotateDirection * Math.random() * 30;
        cardSprite.spriteFrame = this.cardAtlas.getSpriteFrame(atlasSliceName);
        this.cardContainer.addChild(cardNode);
        this.rotateDirection = this.rotateDirection >= 1 ? -1 : 1;
    }

    snap(): void {
        this.connection.snap(this.game.gameId);
    }

    onSnap(e: cc.Event.EventCustom): void {
        const data: any = e.getUserData();
        if (data.username === this.connection.player.username) {
            cc.log(`YOU ${data.username} HAVE SNAPPED`);
            this.yourCards.removeAllChildren();
            for (let i: number = 0; i < data.playerCardsCount; i++) {
                const backCard: cc.Node = cc.instantiate(this.cardPrefab);
                backCard.setPosition(new cc.Vec2(i * 0.01, i * 0.01));
                this.yourCards.addChild(backCard);
            }
        } else {
            cc.log(`YEEKS, player ${data.username} hava snapped :(`);
        }
        this.cardContainer.removeAllChildren();
    }

    loadData(): void {
        cc.log(`You are: ${this.connection.player.username} and the current player is: ${this.game.currentPlayer}`);
        this.currentPlayerLabel.string = this.game.currentPlayer;
        const myPlayerData: any = this.game.playerData.filter(p => p.username === this.connection.player.username)[0];

        for (let i: number = 0; i < myPlayerData.playersCardsCount; i++) {
            const backCard: cc.Node = cc.instantiate(this.cardPrefab);
            backCard.setPosition(new cc.Vec2(i * 0.25, i * 0.25));
            this.yourCards.addChild(backCard);
        }

        this.game.playerData.map(pd => {
            const node: cc.Node = new cc.Node();
            const label: cc.Label = node.addComponent(cc.Label);
            label.string = pd.username;
            label.fontSize = 24;
            label.lineHeight = 24;
            return node;
        }).forEach((l: cc.Node) => this.playerNamesContainer.addChild(l));
    }

    popCard(): void {
        this.connection.popCard(this.game.gameId);
    }

    exit(): void {
        cc.director.loadScene(this.menuSceneName);
    }
}
