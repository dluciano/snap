"use strict";

import ItemRoomScrollView from "../Prefabs/ItemRoomScrollView";
import { ConnectionManager } from "./ConnectionManager";
import SnapGame from "./SnapGame";
import WaitingRoomManager from "./WaitingRoomManager";
const { ccclass, property } = cc._decorator;

@ccclass
export default class MainMenuManager extends cc.Component {
    @property(cc.Label)
    loginNameLbl: cc.Label = null;

    @property(cc.Label)
    connectionStatusLbl: cc.Label = null;

    @property(cc.Button)
    loginBtn: cc.Button = null;

    @property(cc.Button)
    logoutBtn: cc.Button = null;

    @property(cc.Button)
    reconnectBtn: cc.Button = null;

    @property(cc.Prefab)
    itemPrefab: cc.Prefab = null;

    @property(cc.ScrollView)
    roomScrollView: cc.ScrollView = null;

    @property()
    roomSceneName: string = "";

    private readonly connection: ConnectionManager = ConnectionManager.getInstance();

    onLoad(): void {
        cc.systemEvent
            .on(ConnectionManager.ON_CONNECTED, this.onConnected, this);
        cc.systemEvent
            .on(ConnectionManager.ON_CONNECTION_CLOSED, this.onDisconnected, this);
        cc.systemEvent
            .on(ConnectionManager.ON_PLAYER_LOGIN_EVENT, this.onLogin, this);
        cc.systemEvent
            .on(ConnectionManager.ON_CREATE_ROOM, this.onRoomCreated, this);
        cc.systemEvent
            .on(ConnectionManager.ON_JOIN_GAME_MESSAGE, this.onJoinGame, this);

        this.refresh();
    }

    onDestroy(): void {
        cc.systemEvent
            .off(ConnectionManager.ON_CONNECTED, this.onConnected, this);
        cc.systemEvent
            .off(ConnectionManager.ON_CONNECTION_CLOSED, this.onDisconnected, this);
        cc.systemEvent
            .off(ConnectionManager.ON_PLAYER_LOGIN_EVENT, this.onLogin, this);
        cc.systemEvent
            .off(ConnectionManager.ON_CREATE_ROOM, this.onRoomCreated, this);
        cc.systemEvent
            .off(ConnectionManager.ON_JOIN_GAME_MESSAGE, this.onJoinGame, this);
    }

    onJoinGame(e: cc.Event.EventCustom): void {
        const joinedUser: string = e.getUserData().username;
        const roomId: any = e.getUserData().roomId;
        if (joinedUser === this.connection.player.username) {
            cc.director.loadScene(this.roomSceneName, () => {
                const scene: cc.Scene = cc.director.getScene();
                const game: SnapGame = scene.getChildByName("Game").getComponent(SnapGame);
                const sceneManager: WaitingRoomManager = scene.getChildByName("WaitingRoomManager").getComponent(WaitingRoomManager);
                game.roomId = roomId;
                sceneManager.loadData();
            });
        }
    }

    createRoom(): void {
        this.connection.createRoom();
    }

    onRoomCreated(e: cc.Event.EventCustom): any {
        const roomId: any = e.getUserData().roomId;
        const createdBy: any = e.getUserData().createdBy;
        if (createdBy === this.connection.player.username) {
            cc.director.loadScene(this.roomSceneName, () => {
                const scene: cc.Scene = cc.director.getScene();
                const game: SnapGame = scene.getChildByName("Game").getComponent(SnapGame);
                const sceneManager: WaitingRoomManager = scene.getChildByName("WaitingRoomManager").getComponent(WaitingRoomManager);
                game.roomId = roomId;
                game.createdBy = createdBy;
                sceneManager.loadData();
            });
            return;
        }
        this.addRoomItem(roomId);
    }

    private addRoomItem(roomId: number): void {
        cc.log("Game created received: " + roomId);
        const node: cc.Node = cc.instantiate(this.itemPrefab);
        const item: ItemRoomScrollView = node.getComponent(ItemRoomScrollView);
        item.id = roomId;
        const lbl: cc.Label = node.getComponentInChildren(cc.Label);
        lbl.string = `Room #${roomId}`;
        const itemCount: number = this.roomScrollView.content.childrenCount;
        const h: number = node.height + 5;
        const y: number = (itemCount * h);
        this.roomScrollView.content.addChild(node);
        node.setPosition(0, -y);
        this.roomScrollView.content.height += h;
    }

    onConnected(e: cc.Event.EventCustom): any {
        this.connectionStatusLbl.string = "Connected";
    }

    onDisconnected(e: cc.Event.EventCustom): any {
        this.connectionStatusLbl.string = "Disconnected";
    }

    onLogin(e: cc.Event.EventCustom): any {
        this.loginNameLbl.string = this.connection.player.username;
    }

    connect(): void {
        this.connection.connect();
    }

    refresh(): void {
        this.connection.refresh();
        fetch("https://localhost:44378/api/GameRoom")
            .then(data => {
                return data.json();
            }).then(rooms => {
                rooms.forEach(room => {
                    this.addRoomItem(room.id);
                });
            });
    }

    login(): void {
        this.connection.login();
    }

    logout(): void {
        this.connection.logout();
    }
}
