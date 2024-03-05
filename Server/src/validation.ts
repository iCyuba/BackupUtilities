import Ajv from "ajv";
import schema from "./schema.json";

export interface Retention {
    count: number;
    size: number;
}

export enum Method {
    Full = "full",
    Incremental = "incremental",
    Differential = "differential",
}

export enum Output {
    Folder = "folder",
    Tar = "tar",
    TarGz = "tar.gz",
    TarBz2 = "tar.bz2",
    Zip = "zip",
}

export interface BackupJob {
    sources: string[];
    ignore?: string[];
    targets: string[];
    method: Method;
    timing: string;
    retention: Retention;
    output?: Output;
}

const ajv = new Ajv();
export const validate = ajv.compile<BackupJob[]>(schema);
