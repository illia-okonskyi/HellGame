import { Application } from "./app/Application.js";
import { Config } from "./Config.js";

window.addEventListener("load", init);

let config = null;
let app = null;

function init() {
    const configName = document.querySelector("#gConfig").value;
    const sessionId = document.querySelector("#gSessionId").value;

    config = new Config(configName, sessionId);
    app = new Application(config);
}

