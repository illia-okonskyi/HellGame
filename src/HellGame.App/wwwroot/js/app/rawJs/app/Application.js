import { ApiClient } from "./ApiClient.js";

export class Application {
    constructor(config) {
        this.config = config;
        this.apiClient = new ApiClient(config);

        this.apiClient.getLocale().then((locale) => document.title = locale);
    }
}