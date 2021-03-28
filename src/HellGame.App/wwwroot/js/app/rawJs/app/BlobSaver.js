export class BlobSaver {
    constructor(blob, fileName) {
        this.blob = blob;
        this.fileName = fileName;
    }

    save() {
        const url = window.URL.createObjectURL(this.blob);

        let anchorElem = document.createElement("a");
        anchorElem.style = "display: none";
        anchorElem.href = url;
        anchorElem.download = this.fileName;

        document.body.appendChild(anchorElem);
        anchorElem.click();

        document.body.removeChild(anchorElem);

        // On Edge, revokeObjectURL should be called only after
        // a.click() has completed, atleast on EdgeHTML 15.15048
        setTimeout(function () {
            window.URL.revokeObjectURL(url);
        }, 1000);
    }
}

