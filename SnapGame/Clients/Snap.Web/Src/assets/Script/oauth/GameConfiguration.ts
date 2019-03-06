"use strict";
import { IGameConfiguration, ITokenResponse, IOAuthConfiguration } from "./IGameConfiguration";
import { IPlayer } from "../api";

export class GameConfiguration implements IGameConfiguration {
    readonly playersEnpoint: string = "/api/Player";

    private _oauthConfiguration: {
        providerID: string;
        client_id: string;
        authorization: string;
        token: string;
        redirect_uri: string;
        scopes: { request: [string]; };
        response_type: string;
        nonce: string;
    };
    private _apiUrl: string;
    private _serverUrl: string;
    private _token: ITokenResponse;
    private _player: IPlayer = null;
    public static readonly ON_PLAYER_DATA_UPDATED: string = "onPlayerDataUpdated";

    get oauthConfiguration(): IOAuthConfiguration {
        return this._oauthConfiguration;
    }

    set oauthConfiguration(config: IOAuthConfiguration) {
        this._oauthConfiguration = config;
    }

    get apiUrl(): string {
        return this._apiUrl;
    }
    set apiUrl(baseUrl: string) {
        this._apiUrl = baseUrl;
    }
    get gameServerUrl(): string {
        return this._serverUrl;
    }
    set gameServerUrl(_serverUrl: string) {
        this._serverUrl = _serverUrl;
    }
    get currentToken(): ITokenResponse {
        return this._token;
    }
    set currentToken(token: ITokenResponse) {
        this._token = token;
    }

    get currentPlayer(): IPlayer {
        return this._player;
    }

    set currentPlayer(player: IPlayer) {
        this._player = player;
        this.dispatch(GameConfiguration.ON_PLAYER_DATA_UPDATED, this._player);
    }
    private dispatch(type: string, data: any): void {
        const e: cc.Event.EventCustom = new cc.Event.EventCustom(type, true);
        e.target = this;
        e.setUserData(data);
        cc.systemEvent.dispatchEvent(e);
    }
}
