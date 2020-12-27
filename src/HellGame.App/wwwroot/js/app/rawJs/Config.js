import { ConfigError } from "./Exceptions.js"

export class Config {
    constructor(name, sessionId) {
        this.makeCommon(sessionId);

        if (name === "dev") {
            this.makeDev();
            return;
        } else if (name === "release") {
            this.makeRelease();
            return;
        }

        throw new ConfigError(`Invalid configuration ${name}`);
    }

    makeCommon(sessionId) {
        this.sessionId = sessionId;
    }

    makeDev() {
        this.apiEndpoint = "https://localhost:44327/api";
    }

    makeRelease() {
        this.apiEndpoint = "https://localhost:44327/api";
    }
}