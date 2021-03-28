export class StatusBar {
    constructor() {
        this.divStatusBar = document.querySelector("#divStatusBar");
        this.lblStatus = this.divStatusBar.querySelector("label");
    }

    setStatus(status) {
        this.lblStatus.textContent = status;
        this.scrollIntoView();
    }

    setLabel(header) {
        this.divStatusBar.querySelector("h5").textContent = header;
    }

    scrollIntoView() {
        this.divStatusBar.scrollIntoView();
    }
}