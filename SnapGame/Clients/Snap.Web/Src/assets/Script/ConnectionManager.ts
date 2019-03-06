import * as signalR from "@aspnet/signalr";
import { IServerConfiguration, ISecurityConfiguration } from "./oauth/IGameConfiguration";

export class ConnectionManager {
    private static readonly conn: ConnectionManager = new ConnectionManager();
    public static getInstance(): ConnectionManager {
        return ConnectionManager.conn;
    }

    public static readonly ON_CONNECTING: string = "onConnecting";
    public static readonly ON_CONNECTION_CLOSED: string = "onConnectionClosed";
    public static readonly ON_CONNECTED: string = "onConnected";

    public static readonly CREATE_ROOM: string = "CreateRoom";
    public static readonly JOIN_GAME_MESSAGE: string = "JoinGame";
    public static readonly START_GAME_MESSAGE: string = "StartGame";
    public static readonly POP_CARD_MESSAGE: string = "PopCard";
    public static readonly SNAP_MESSAGE: string = "Snap";

    public static readonly ON_CREATE_ROOM: string = "OnCreateRoom";
    public static readonly ON_POP_CARD_MESSAGE: string = "OnJoinGame";
    public static readonly ON_START_GAME_MESSAGE: string = "OnStartGame";
    public static readonly ON_JOIN_GAME_MESSAGE: string = "OnPopCard";
    public static readonly ON_SNAP_MESSAGE: string = "OnPopCard";

    private connection: signalR.HubConnection;
    private connectTryCount: number = 0;

    constructor(private serverConfig?: IServerConfiguration,
        private securityConfig?: ISecurityConfiguration) { }

    async connect(): Promise<void> {
        if (!this.connection) {
            const builder: signalR.HubConnectionBuilder = new signalR.HubConnectionBuilder()
                .configureLogging(signalR.LogLevel.Information)
                .withUrl(this.serverConfig.gameServerUrl, {
                    accessTokenFactory: () => {
                        return this.securityConfig.currentToken.access_token;
                    }
                });
            this.connection = builder.build();
            this.connection.onclose((err) => {
                this.dispatch(ConnectionManager.ON_CONNECTION_CLOSED, err);
            });
            this.connection
                .on(ConnectionManager.CREATE_ROOM, (message) => {
                    cc.log(`${ConnectionManager.CREATE_ROOM} received: ${message}`);
                    this.dispatch(ConnectionManager.ON_CREATE_ROOM, message);
                });
            this.connection
                .on(ConnectionManager.JOIN_GAME_MESSAGE, (message) => {
                    cc.log(`${ConnectionManager.JOIN_GAME_MESSAGE} received: ${message}`);
                    this.dispatch(ConnectionManager.ON_JOIN_GAME_MESSAGE, message);
                });
            this.connection
                .on(ConnectionManager.START_GAME_MESSAGE, (message) => {
                    cc.log(`${ConnectionManager.START_GAME_MESSAGE} received: ${message}`);
                    this.dispatch(ConnectionManager.ON_START_GAME_MESSAGE, message);
                });
            this.connection
                .on(ConnectionManager.POP_CARD_MESSAGE, (message) => {
                    cc.log(`${ConnectionManager.POP_CARD_MESSAGE} received: ${message}`);
                    this.dispatch(ConnectionManager.ON_POP_CARD_MESSAGE, message);
                });
            this.connection
                .on(ConnectionManager.SNAP_MESSAGE, (message) => {
                    cc.log(`${ConnectionManager.SNAP_MESSAGE} received: ${message}`);
                    this.dispatch(ConnectionManager.ON_SNAP_MESSAGE, message);
                });
        }
        if (this.connection.state === signalR.HubConnectionState.Connected) {
            return new Promise<void>(() => {
                cc.log("You are already connected");
                return;
            });
        }

        cc.log("Connecting");
        this.dispatch(ConnectionManager.ON_CONNECTING, {});
        this.connectTryCount++;
        if (this.connectTryCount > 10) {
            throw "maxReconnectFailed";
        }
        await this.connection.start();
        this.connectTryCount = 0;
        this.dispatch(ConnectionManager.ON_CONNECTED, {});
        cc.log("Connection stablished!");

    }

    createRoom(): Promise<void> {
        this.validateConnected();
        return this.connection.send(ConnectionManager.CREATE_ROOM).then((data) => {
            cc.log(data);
        }).catch(e => {
            cc.error(e);
            throw e;
        });
    }

    startGame(roomId: number): any {
        this.validateConnected();
        return this.connection.send(ConnectionManager.START_GAME_MESSAGE, roomId).then((data) => {
            cc.log(`Message ${ConnectionManager.START_GAME_MESSAGE} sent`);
        }).catch(err => {
            cc.error(err);
            throw err;
        });
    }
    joinGame(roomId: number, isViewer: boolean = false): Promise<any> {
        this.validateConnected();
        return this.connection.send(ConnectionManager.JOIN_GAME_MESSAGE, roomId, isViewer)
            .then((data) => {
                cc.log(`Message ${ConnectionManager.JOIN_GAME_MESSAGE} sent`);
            }).catch(err => {
                cc.error(err);
                throw err;
            });
    }
    popCard(gameId: number): Promise<any> {
        this.validateConnected();
        return this.connection.send(ConnectionManager.POP_CARD_MESSAGE, gameId).then((data) => {
            cc.log(`Message ${ConnectionManager.POP_CARD_MESSAGE} sent`);
        }).catch(err => {
            cc.error(err);
            throw err;
        });
    }
    snap(gameId: number): Promise<any> {
        this.validateConnected();
        return this.connection.send(ConnectionManager.SNAP_MESSAGE, gameId).then((data) => {
            cc.log(`Message ${ConnectionManager.POP_CARD_MESSAGE} sent`);
        }).catch(err => {
            cc.error(err);
            throw err;
        });
    }
    private validateConnected(): void {
        if (this.connection.state === signalR.HubConnectionState.Disconnected) {
            cc.error("You should be connected to create a room");
            throw "You are discconected";
        }
        return;
    }

    private dispatch(type: string, data: any): void {
        const e: cc.Event.EventCustom = new cc.Event.EventCustom(type, true);
        e.target = this;
        e.setUserData(data);
        cc.director.dispatchEvent(e);
        cc.systemEvent.dispatchEvent(e);
    }
}

export default ConnectionManager;