import { Theme } from "./theme.type";

export interface IAppConfig {
    MAIL4SOLLY: string;
    GENDERS: any;
    THEMES: Theme[];
    CLIENT_ID: string;
    DOMAIN: string;
    AUDIENCE: string;
}