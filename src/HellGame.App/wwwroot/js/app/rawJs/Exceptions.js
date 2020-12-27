export class AppError extends Error {
    constructor(message) {
        super(message);
        this.name = this.constructor.name;
    }
}

export class ConfigError extends AppError {
    constructor(message) {
        super(message);
    }
}

export class ApiError extends AppError {
    constructor(message) {
        super(message);
    }
}