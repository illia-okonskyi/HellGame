export class Base64Encoder {
    static encode(str, encoding = 'utf-8') {
        var bytes = new (TextEncoder || TextEncoderLite)(encoding).encode(str);
        return base64js.fromByteArray(bytes);
    }

    static decode(str, encoding = 'utf-8') {
        var bytes = base64js.toByteArray(str);
        return new (TextDecoder || TextDecoderLite)(encoding).decode(bytes);
    }
}