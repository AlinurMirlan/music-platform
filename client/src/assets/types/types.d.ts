export type Genre = {
    id: number;
    name: string;
};
export type Author = {
    id: number;
    nickname: string;
};
export interface Song {
    id: number;
    title: string;
    album: string;
    releaseDate: string;
    imageFile: string;
    songFile: string;
    popularity: number;
    authors: Author[];
    genres: Genre[];
}
export interface Page {
    currentPage: number;
    pageSize: number;
    totalPages: number;
}
export interface PagedSongs extends Page {
    results: Song[];
}
export interface JwtRaw {
    jwt: string;
    expiration: string;
}
export interface Jwt {
    token: string;
    expiration: Date;
}
