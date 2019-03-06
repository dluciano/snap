import { GameRoom } from "../models/GameRoom";
import { throwException } from "./SwaggerException";

export class GameRoomClient {
    private http: {
        fetch(url: RequestInfo, init?: RequestInit): Promise<Response>;
    };
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;
    constructor(baseUrl?: string, http?: {
        fetch(url: RequestInfo, init?: RequestInit): Promise<Response>;
    }) {
        this.http = http ? http : <any>window;
        this.baseUrl = baseUrl ? baseUrl : "https://localhost:44378";
    }
    getAll(): Promise<GameRoom[] | null> {
        const url_: string = (this.baseUrl + "/api/GameRoom").replace(/[?&]$/, "");
        const options_: RequestInit = <RequestInit>{
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };
        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processGetAll(_response);
        });
    }
    protected processGetAll(response: Response): Promise<GameRoom[] | null> {
        const status: number = response.status;
        let _headers: any = {};
        if (response.headers && response.headers.forEach) {
            response.headers.forEach((v: any, k: any) => _headers[k] = v);
        }

        if (status === 200) {
            return response.text().then((_responseText) => {
                let result200: any = null;
                const resultData200: any = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                if (resultData200 && resultData200.constructor === Array) {
                    result200 = [] as any;
                    for (let item of resultData200) {
                        result200!.push(GameRoom.fromJS(item));
                    }
                }
                return result200;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<GameRoom[] | null>(<any>null);
    }

    get(id: number): Promise<GameRoom | null> {
        let url_: string = this.baseUrl + "/api/GameRoom/{id}";
        if (id === undefined || id === null) {
            throw new Error("The parameter 'id' must be defined.");
        }
        url_ = url_.replace("{id}", encodeURIComponent("" + id));
        url_ = url_.replace(/[?&]$/, "");
        const options_: RequestInit = <RequestInit>{
            method: "GET",
            headers: {
                "Accept": "application/json"
            }
        };
        return this.http.fetch(url_, options_).then((_response: Response) => {
            return this.processGet(_response);
        });
    }
    protected processGet(response: Response): Promise<GameRoom | null> {
        const status: number = response.status;
        let _headers: any = {};
        if (response.headers && response.headers.forEach) {
            response.headers.forEach((v: any, k: any) => _headers[k] = v);
        }

        if (status === 200) {
            return response.text().then((_responseText) => {
                let result200: any = null;
                const resultData200: any = _responseText === "" ? null : JSON.parse(_responseText, this.jsonParseReviver);
                result200 = resultData200 ? GameRoom.fromJS(resultData200) : <any>null;
                return result200;
            });
        } else if (status !== 200 && status !== 204) {
            return response.text().then((_responseText) => {
                return throwException("An unexpected server error occurred.", status, _responseText, _headers);
            });
        }
        return Promise.resolve<GameRoom | null>(<any>null);
    }
}
