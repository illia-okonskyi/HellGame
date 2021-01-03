export class GameImage {
    constructor() {
        this.divGameImage = document.querySelector("#divGameImage");
    }

    setLabels(header, source) {
        this.divGameImage.querySelector("h5").textContent = header;
        this.divGameImage.querySelector("a").textContent = source;
    }

    setGameImage(mediaType, soureUrl, data) {
        let img = this.divGameImage.querySelector("img");
        let a = this.divGameImage.querySelector("a");

        img.src = `data:${mediaType};base64, ${data}`;

        if (soureUrl) {
            a.href = soureUrl;
            a.target = "_blank";
        } else {
            a.href = "#";
            a.target = "";
        }
    }
}