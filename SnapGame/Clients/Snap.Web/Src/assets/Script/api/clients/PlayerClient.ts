"use strict";
import { Player } from "../models/Player";

import { throwException } from ".";
import { IApiConfiguration, ISecurityConfiguration, ISecurityManager } from "../../oauth/IGameConfiguration";
import { IPlayer } from "../models";

export class PlayerClient {
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(private apiConfig: IApiConfiguration,
        private securityConfig: ISecurityConfiguration,
        private fetcher: GlobalFetch) {
    }

    protected get = async (action: string): Promise<Response | null> => {
        const url_: string = (this.apiConfig.apiUrl +
            this.apiConfig.playersEnpoint +
            action).replace(/[?&]$/, "");
        const options_: RequestInit = <RequestInit>{
            method: "GET",
            headers: {
                "Accept": "application/json",
                "Authentication": `Bearer ${this.securityConfig.currentToken.access_token}`
            }
        };
        return await this.fetcher.fetch(url_, options_);
    }

    protected post_ = async (): Promise<Response | null> => {
        const url_: string = (this.apiConfig.apiUrl +
            this.apiConfig.playersEnpoint).replace(/[?&]$/, "");
        const options_: RequestInit = <RequestInit>{
            method: "POST",
            headers: {
                "Accept": "application/json",
                "Authentication": `Bearer ${this.securityConfig.currentToken.access_token}`
            }
        };
        return await this.fetcher.fetch(url_, options_);
    }
    async me(): Promise<IPlayer | null> {
        const _response: Response = await this.get("/me");
        return await this.responseToEntity(_response);
    }

    post = async (): Promise<IPlayer | null> => {
        const response: Response = await this.post_();
        const player: IPlayer = await this.responseToEntity(response);
        return player;
    }

    protected responseToEntity = async (response: Response): Promise<IPlayer | null> => {
        const status: number = response.status;
        let _headers: any = {};
        if (response.headers && response.headers.forEach) {
            response.headers.forEach((v: any, k: any) => _headers[k] = v);
        }

        if (status === 200) {
            const _responseText: any = await response.text();
            const resultData200: any = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
            const data: any = typeof resultData200 === "object" ? resultData200 : {};
            const entity: IPlayer = new Player();
            entity.init(data);
            return entity;
        } else if (status !== 200 && status !== 204) {
            const _responseText: any = await response.text();
            return throwException("An unexpected server error occurred.", status, _responseText, _headers);
        }
        return Promise.resolve<IPlayer | null>(<any>null);
    }
}
