import * as signalR from "@aspnet/signalr";
import { JSO, Popup, Fetcher } from "jso";

export class ConnectionManager {
    private static readonly conn: ConnectionManager = new ConnectionManager();
    public static getInstance(): ConnectionManager {
        return ConnectionManager.conn;
    }
    public static readonly ON_CHALLENGING: string = "onChallenging";
    public static readonly ON_PLAYER_LOGIN_EVENT: string = "onPlayerLoginEvent";
    public static readonly ON_PLAYER_LOGOUT: string = "onPlayerLogoutEvent";

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

    private static readonly ENPOINT: string = "https://localhost:44378/game_notifications";
    private connection: signalR.HubConnection;
    private token: ITokenResponse;
    private connectTryCount: number = 0;

    private _player: any = null;

    get player(): any {
        return this._player;
    }

    set player(player: any) {
        this._player = player;
        const eventType: string = this._player ? ConnectionManager.ON_PLAYER_LOGIN_EVENT :
            ConnectionManager.ON_PLAYER_LOGOUT;
        this.dispatch(eventType, this._player);
    }

    private client: JSO = new JSO({
        providerID: "snapGameOauth",
        client_id: "snapGameApiDevSwagger",
        authorization: "https://localhost:52365/connect/authorize",
        token: "https://localhost:52365/connect/token",
        redirect_uri: "http://localhost:7456",
        debug: true,
        scopes: {
            request: ["snapgame"],
        },
        response_type: "token",
        request: {
            // tslint:disable-next-line:max-line-length
            nonce: "636864884858396406.MWI4ZjNkYWItOGU1My00YmFiLTg1MTAtMWQzOTY2OTM4YzRkOGFhOGI1OGItODc0YS00NGEyLWI3NzgtYzU0YmJiMzk5NWY0"
        }
    });

    refresh(): Promise<any> {
        return this.login().then(() => {
            return this.connect().catch(e => {
                cc.error(e);
                throw e;
            });
        });
    }

    login(): Promise<any> {
        this.client.callback();
        cc.log("Trying to loggin");
        this.dispatch(ConnectionManager.ON_CHALLENGING, {});
        return this.client.getToken()
            .then((token) => {
                this.token = token;
                cc.log("I got the token: ", this.token);

                const fetcher: Fetcher = new Fetcher(this.client);
                const url: string = "https://localhost:44378/api/Player/me";
                fetcher.fetch(url, {})
                    .then((data) => {
                        return data.json();
                    })
                    .then((data) => {
                        cc.log(`Player ifno recieved from the server ${data}`);
                        this.player = data;
                    })
                    .catch((err) => {
                        cc.error("Error getting user data: ", err);
                        throw err;
                    });
            })
            .catch((err) => {
                cc.error("Error from getToken: ", err);
                throw err;
            });
    }

    logout(): void {
        this.client.wipeTokens();
        this.player = null;
    }

    connect(): Promise<void> {
        if (!this.connection) {
            const builder: signalR.HubConnectionBuilder = new signalR.HubConnectionBuilder()
                .configureLogging(signalR.LogLevel.Information)
                .withUrl(ConnectionManager.ENPOINT, {
                    accessTokenFactory: () => {
                        return !this.token ? "" : this.token.access_token;
                    }
                });
            this.connection = builder.build();
            this.connection.onclose((err) => {
                this.dispatch(ConnectionManager.ON_CONNECTION_CLOSED, err);
                // this.connect();
                // this.connectionStatusLbl.string = "Disconnected";
                // this.reconnectBtn.node.getChildByName("Label").getComponent(cc.Label).string = "Reconnect";
                // cc.error(err);
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
        return this.connection.start().then(c => {
            this.connectTryCount = 0;

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

            this.dispatch(ConnectionManager.ON_CONNECTED, {});
            cc.log("Connection stablished!");
        });

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

export interface ITokenResponse {
    access_token: string;
    expires: number;
    expires_in: number;
    received: number;
    scopes: [];
    state: string;
    token_type: string;
}
export default ConnectionManager;