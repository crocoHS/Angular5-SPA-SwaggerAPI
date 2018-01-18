import { InjectionToken } from "@angular/core";
import { Theme } from "../../models/theme.type";
import { IAppConfig } from "../../models/app-config.type";

export const APP_DI_CONFIG: IAppConfig = {
    MAIL4SOLLY: 'mail4solly@gmail.com',
    COLORS: ['maroon', 'red', 'orange', 'yellow', 'olive', 'green', 'purple', 'fuchsia', 'lime', 'teal', 'aqua', 'blue', 'navy', 'black', 'gray'],
    NAMES: ['Maia', 'Asher', 'Olivia', 'Atticus', 'Amelia', 'Jack', 'Charlotte', 'Theodore', 'Isla', 'Oliver', 'Isabella', 'Jasper', 'Cora', 'Levi', 'Violet', 'Arthur', 'Mia', 'Thomas', 'Elizabeth'],
    GENDERS: [
        { value: 'M', display: 'Male' },
        { value: 'F', display: 'Female' },
        { value: 'O', display: 'Other' }
    ],
    THEMES: [
        { backgroundColor: 'black', fontColor: 'white', display: 'Dark' },
        { backgroundColor: 'white', fontColor: 'black', display: 'Light' },
        { backgroundColor: 'grey', fontColor: 'white', display: 'Sleek' }
    ],
    CLIENT_ID: 'k09AccZ2s2G66kX3NpYHJ4NcvTFJEmIs',
    DOMAIN: 'solly.auth0.com',
    AUDIENCE: 'https://todolist-api.azurewebsites.net/api/'
};

export let APP_CONFIG = new InjectionToken<IAppConfig>('app.config');