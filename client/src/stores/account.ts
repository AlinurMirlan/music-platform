import { Jwt } from '@/assets/classes';
import type { JwtRaw } from '@/assets/types/types';
import { defineStore } from 'pinia';

interface State {
    redirectPath: string,
    jwt: Jwt
}
interface StateRaw {
    redirectPath: string,
    jwt: { [P in keyof Jwt]: string }
}

export const useAccountStore = defineStore('account', {
    state: (): State => {
        let accountState = localStorage.getItem("account");
        if (accountState) {
            const accountRaw: StateRaw = JSON.parse(accountState);
            const account: State = {
                jwt: {
                    token: accountRaw.jwt.token,
                    expiration: new Date(Date.parse(accountRaw.jwt.expiration))
                },
                redirectPath: accountRaw.redirectPath
            }
            return account;
        }
        return {
            redirectPath: "",
            jwt: {
                token: "",
                expiration: new Date()
            }
        }
    },
    getters: {
        isAuthenticated: (state) => state.jwt.token !== "" && state.jwt.expiration > new Date()
    },
    actions: {
        setJwt(jwtRaw: JwtRaw) {
            this.jwt = new Jwt(jwtRaw);
        },
        getRedirect() {
            let redirect = "/";
            if (this.redirectPath) {
                redirect = this.redirectPath;
                this.redirectPath = "";
            }
            return redirect;
        }
    }
});
