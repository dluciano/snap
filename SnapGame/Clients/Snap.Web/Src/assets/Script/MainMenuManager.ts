"use strict";

import ItemRoomScrollView from "../Prefabs/ItemRoomScrollView";
import { ConnectionManager } from "./ConnectionManager";
import SnapGame from "./SnapGame";
import WaitingRoomManager from "./WaitingRoomManager";
import JsoSecurityManager from "./oauth/SecurityManager";
import { PlayerClient, IPlayer } from "./api";
import { GameConfiguration } from "./oauth/GameConfiguration";
import { IGameConfiguration, ISecurityManager } from "./oauth/IGameConfiguration";
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

    private connection: ConnectionManager;
    private oauth: ISecurityManager;
    private playerClient: PlayerClient;
    private readonly gameConfig: IGameConfiguration = new GameConfiguration();
    /**
     *
     */
    onLoad(): void {
        cc.systemEvent
            .on(ConnectionManager.ON_CONNECTED, this.onConnected, this);
        cc.systemEvent
            .on(ConnectionManager.ON_CONNECTION_CLOSED, this.onDisconnected, this);
        cc.systemEvent
            .on(GameConfiguration.ON_PLAYER_DATA_UPDATED, this.onPlayerDataUpdated, this);
        cc.systemEvent
            .on(ConnectionManager.ON_CREATE_ROOM, this.onRoomCreated, this);
        cc.systemEvent
            .on(ConnectionManager.ON_JOIN_GAME_MESSAGE, this.onJoinGame, this);

        this.gameConfig.oauthConfiguration = {
            providerID: "snapGameOauth",
            client_id: "snapGameApiDevSwagger",
            authorization: "https://localhost:52365/connect/authorize",
            token: "https://localhost:52365/connect/token",
            redirect_uri: "http://localhost:7456",
            scopes: {
                request: ["snapgame"],
            },
            response_type: "token",
            nonce: "636864884858396406.MWI4ZjNkYWItOGU1My00YmFiLTg1MTAtMWQzOTY2OTM4YzRkOGFhOGI1OGItODc0YS00NGEyLWI3NzgtYzU0YmJiMzk5NWY0"
        };

        this.gameConfig.apiUrl = "https://localhost:44378";
        this.gameConfig.gameServerUrl = "https://localhost:44378/game_notifications";
        const fetcher: GlobalFetch = {
            fetch: (input: RequestInfo, init?: RequestInit): Promise<Response> => {
                return this.oauth ? this.oauth.fetch(input, init) : window.fetch(input, init);
            }
        };
        this.playerClient = new PlayerClient(this.gameConfig, this.gameConfig, fetcher);
        this.oauth = new JsoSecurityManager(this.playerClient, this.gameConfig);
        this.connection = new ConnectionManager(this.gameConfig, this.gameConfig);
    }

    onDestroy(): void {
        cc.systemEvent
            .off(ConnectionManager.ON_CONNECTED, this.onConnected, this);
        cc.systemEvent
            .off(ConnectionManager.ON_CONNECTION_CLOSED, this.onDisconnected, this);
        cc.systemEvent
            .off(GameConfiguration.ON_PLAYER_DATA_UPDATED, this.onPlayerDataUpdated, this);
        cc.systemEvent
            .off(ConnectionManager.ON_CREATE_ROOM, this.onRoomCreated, this);
        cc.systemEvent
            .off(ConnectionManager.ON_JOIN_GAME_MESSAGE, this.onJoinGame, this);
    }

    start = async (): Promise<void> => {
        await this.refresh();
    }

    onJoinGame(e: cc.Event.EventCustom): void {
        const joinedUser: string = e.getUserData().username;
        const roomId: any = e.getUserData().roomId;
        if (joinedUser === this.gameConfig.currentPlayer.username) {
            cc.director.loadScene(this.roomSceneName, () => {
                const scene: cc.Scene = cc.director.getScene();
                const game: SnapGame = scene.getChildByName("Game").getComponent(SnapGame);
                const sceneManager: WaitingRoomManager = scene.getChildByName("WaitingRoomManager").getComponent(WaitingRoomManager);
                game.roomId = roomId;
                sceneManager.loadData();
            });
        }
    }

    createRoom = async (): Promise<void> => {
        await this.connection.createRoom();
    }

    onRoomCreated(e: cc.Event.EventCustom): void {
        const roomId: any = e.getUserData().roomId;
        const createdBy: any = e.getUserData().createdBy;
        if (createdBy === this.gameConfig.currentPlayer.username) {
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

    onConnected(e: cc.Event.EventCustom): void {
        this.connectionStatusLbl.string = "Connected";
    }

    onDisconnected(e: cc.Event.EventCustom): void {
        this.connectionStatusLbl.string = "Disconnected";
    }

    onPlayerDataUpdated(e: cc.Event.EventCustom): void {
        const player: IPlayer = e.getUserData();
        this.loginNameLbl.string = player && player.username ? player.username : "Not singed in";
    }

    connect = async (): Promise<void> => {
        await this.connection.connect();
    }

    refresh = async (): Promise<void> => {
        await this.oauth.login();
        await this.connection.connect();
        // const gameRoomApi: api.GameRoomClient = new api.GameRoomClient("https://localhost:44378");
        // const rooms: api.GameRoom[] = await gameRoomApi.getAll();
        // rooms.forEach(room => this.addRoomItem(room.id));
    }

    login = async (): Promise<void> => {
        await this.oauth.login();
    }

    logout(): void {
        this.oauth.logout();
    }
}
