import { ApiError } from "../Exceptions.js"

export class ApiClient {
    constructor(config) {
        this.sessionId = config.sessionId;
        this.apiEndpoint = config.apiEndpoint;
    }

    async getLocale() {
        const response = await this.apiRequest(
            {
                request: "locale",
                payload: {
                    sessionId: this.sessionId,
                }
            });
        return response.locale;
    }

    async setLocale(locale) {
        const response = await this.apiRequest(
            {
                method: "POST",
                request: "locale",
                payload: {
                    sessionId: this.sessionId,
                    locale: locale
                }
            });

        return response.locale;
    }


    async apiRequest({ method = "GET", request = "", payload = null } = {}) {
        let url = new URL(`${this.apiEndpoint}/${request}`);
        let body = null;
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
        const response = await fetch(url, {
            method: method,
            mode: "cors",
            cache: "no-cache",
            credentials: "same-origin",
            headers: {
                "Content-Type": "application/json"
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