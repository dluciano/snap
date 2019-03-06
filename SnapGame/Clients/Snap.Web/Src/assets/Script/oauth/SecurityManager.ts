"use strict";
import { JSO, Fetcher } from "jso";
import { PlayerClient, IPlayer } from "../api";
import { IOAuthConfiguration, ISecurityManager, ISecurityConfiguration } from "./IGameConfiguration";

export class JsoSecurityManager implements ISecurityManager {
    public static readonly ON_CHALLENGING: string = "onChallenging";
    private client: JSO;

    constructor(private playerClient: PlayerClient,
        private securityConfig: ISecurityConfiguration) { }

    async login(): Promise<IPlayer | null> {
        const oautConfig: IOAuthConfiguration = this.securityConfig.oauthConfiguration;
        this.client = new JSO({
            providerID: oautConfig.providerID,
            client_id: oautConfig.client_id,
            authorization: oautConfig.authorization,
            token: oautConfig.token,
            redirect_uri: oautConfig.redirect_uri,
            debug: true,
            scopes: oautConfig.scopes,
            response_type: oautConfig.response_type,
            request: {
                nonce: oautConfig.nonce
            }
        });
        await this.client.callback();
        cc.log("Loggin started");
        this.dispatch(JsoSecurityManager.ON_CHALLENGING, {});
        try {
            this.securityConfig.currentToken = await this.client.getToken();
            cc.log("I got the token: ", this.securityConfig.currentToken);
            try {
                return this.securityConfig.currentPlayer = await this.playerClient.post();
            } catch (error) {
                cc.log(`Error recieving Player info: ${error}`);
                throw error;
            }
        } catch (error) {
            cc.error("Error from getToken: ", error);
            throw error;
        }
    }

    logout = (): Promise<void> => {
        if (!this.client) {
            return;
        }
        this.client.wipeTokens();
        this.securityConfig.currentPlayer = null;
    }

    fetch(input: RequestInfo, init?: RequestInit): Promise<Response> {
        return this.client ? new Fetcher(this.client).fetch(input, init) :
            window.fetch(input, init);
    }

    private dispatch(type: string, data: any): void {
        const e: cc.Event.EventCustom = new cc.Event.EventCustom(type, true);
        e.target = this;
        e.setUserData(data);
        cc.systemEvent.dispatchEvent(e);
    }
}

export default JsoSecurityManager;