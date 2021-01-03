export class Pinger {
    constructor(apiClient, onError) {
        this.PING_INTERVAL = 60000; // 1 min

        this.apiClient = apiClient;
        this.onError = onError;

        this.pingTimerId = null;
        this.ping();
    }

    ping() {
        this.pingInternal()
            .catch(error => this.onError(error));
    }

    async pingInternal() {
        await this.apiClient.ping();
        this.fetchTimerId = setTimeout(() => this.ping(), this.PING_INTERVAL);
    }
}