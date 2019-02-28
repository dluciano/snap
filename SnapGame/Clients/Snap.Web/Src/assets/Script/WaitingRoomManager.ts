"use strict";
import ConnectionManager from "./ConnectionManager";
import "./ConnectionManager";
import SnapGame from "./SnapGame";
import MainGameManager from "./MainGameManager";

const { ccclass, property } = cc._decorator;

@ccclass
export default class WaitingRoomManager extends cc.Component {
    @property(cc.Prefab)
    userPrefab: cc.Prefab = null;

    @property()
    menuScene: string = "";
    @property()
    mainGameScene: string = "";

    @property(cc.Label)
    titleLabel: cc.Label = null;

    @property(cc.Node)
    snapGameNode: cc.Node = null;

    @property(cc.Node)
    playersPanelNode: cc.Node = null;

    private game: SnapGame = null;
    private readonly connection: ConnectionManager = ConnectionManager.getInstance();

    onLoad(): void {
        cc.systemEvent
            .on(ConnectionManager.ON_START_GAME_MESSAGE, this.gameStarted, this);
        this.game = this.snapGameNode.getComponent(SnapGame);
        cc.systemEvent
            .on(ConnectionManager.ON_JOIN_GAME_MESSAGE, this.onJoinGame, this);
    }

    onDestroy(): void {
        cc.systemEvent
            .off(ConnectionManager.ON_START_GAME_MESSAGE, this.gameStarted, this);
        cc.systemEvent
            .off(ConnectionManager.ON_JOIN_GAME_MESSAGE, this.onJoinGame, this);
    }

    loadData(): void {
        this.titleLabel.string = "Room #" + this.game.roomId.toString();
        fetch(`https://localhost:44378/api/GameRoom/${this.game.roomId}`)
            .then(data => {
                return data.json();
            }).then(room => {
                room.roomPlayers.map(rp => rp.player).forEach(player => {
                    this.addPlayerItem(player.username);
                });
            });
    }

    private onJoinGame(e: cc.Event.EventCustom): void {
        const joinedUser: string = e.getUserData().username;
        this.addPlayerItem(joinedUser);
    }

    startGame(): void {
        ConnectionManager.getInstance().startGame(this.game.roomId);
    }

    private gameStarted(e: cc.Event.EventCustom): void {
        const data: any = e.getUserData();
        const gameId: number = data.gameId;
        const currentPlayer: string = data.currentPlayer;
        cc.director.loadScene(this.mainGameScene, () => {
            const scene: cc.Scene = cc.director.getScene();
            const gameManager: MainGameManager = scene
                .getChildByName("MainGameManager")
                .getComponent(MainGameManager);
            const game: SnapGame = scene
                .getChildByName("Game")
                .getComponent(SnapGame);
            game.gameId = gameId;
            game.currentPlayer = currentPlayer;
            game.playerData = data.playerData;
            gameManager.loadData();
        });
    }

    private addPlayerItem(username: string): void {
        cc.log("User: " + username + " joined the match");
        const node: cc.Node = cc.instantiate(this.userPrefab);
        const lbl: cc.Label = node.getChildByName("Label").getComponent(cc.Label);
        lbl.string = username.toString();
        this.playersPanelNode.addChild(node);
    }

    back(): void {
        cc.director.loadScene(this.menuScene);
    }

    deleteRoom(): void {
        throw "Not yet implemented";
    }
}
