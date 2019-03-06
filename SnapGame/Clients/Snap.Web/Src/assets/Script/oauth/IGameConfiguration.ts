"use strict";
import { IPlayer } from "../api";

export interface IApiConfiguration {
    apiUrl: string;
    playersEnpoint: string;
}

export interface IOAuthConfiguration {
    providerID: string;
    client_id: string;
    authorization: string;
    token: string;
    redirect_uri: string;
    scopes: {
        request: [string]
    };
    response_type: string;
    nonce: string;
}
export interface ISecurityConfiguration {
    currentPlayer: IPlayer;
    currentToken: ITokenResponse;
    oauthConfiguration: IOAuthConfiguration;
}
export interface IServerConfiguration {
    gameServerUrl: string;
}
export interface IGameConfiguration extends IServerConfiguration, IApiConfiguration, ISecurityConfiguration { }
export interface ISecurityManager extends GlobalFetch {
    login(): Promise<IPlayer | null>;
    logout(): Promise<void>;
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

export default IGameConfiguration;