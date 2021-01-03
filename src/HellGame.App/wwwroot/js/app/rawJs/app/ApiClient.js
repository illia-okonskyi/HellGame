import { ApiError } from "../Exceptions.js"
import { Base64Encoder } from "./Base64Encoder.js";

export class ApiClient {
    constructor(config) {
        this.SESSION_ID_HEADER_NAME = "HellGame-SessionId";

        this.sessionId = config.sessionId;
        this.apiEndpoint = config.apiEndpoint;
    }

    async ping() {
        await this.apiRequest({ method: "POST", path: "ping" });
    }

    async getLocale() {
        const response = await this.apiRequest({ path: "locale" });
        return response.locale;
    }

    async setLocale(locale) {
        const response = await this.apiRequest(
            {
                method: "POST",
                path: "locale",
                payload: {
                    locale: locale
                }
            });

        return response.locale;
    }

    async getTextAsset(key) {
        const response = await this.apiRequest({ path: "assets/text", payload: { key: key } });

        return {
            key: response.key,
            data: Base64Encoder.decode(response.data)
        };
    }

    async getImageAsset(key) {
        return await this.apiRequest({ path: "assets/image", payload: { key: key } });
    }

    async apiRequest({ method = "GET", path = "", payload = null } = {}) {
        let url = new URL(`${this.apiEndpoint}/${path}`);
        let body = null;
        if (payload !== null) {
            if (method === "GET") {
                const payloadSearchParams = new URLSearchParams(payload);
                for (const [key, value] of payloadSearchParams) {
                    url.searchParams.append(`request.payload.${key}`, value);
                }
            } else {
                body = JSON.stringify({
                    payload: payload
                });
            }
        }

        const response = await fetch(url, {
            method: method,
            mode: "cors",
            cache: "no-cache",
            credentials: "same-origin",
            headers: {
                "Content-Type": "application/json",
                [this.SESSION_ID_HEADER_NAME]: this.sessionId 
            },
            redirect: "follow",
            referrerPolicy: "no-referrer",
            body: body
        });
        const apiResponse = await response.json();

        if (!apiResponse.success) {
            throw new ApiError(apiResponse.error);
        }

        return apiResponse.payload;
    }
}