import type { JwtRaw } from './types/types';

export class Jwt {
    token: string;
    expiration: Date;
    constructor(jwtRaw: JwtRaw) {
        this.token = jwtRaw.jwt;
        this.expiration = new Date(Date.parse(jwtRaw.expiration));
    }
}
