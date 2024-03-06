import { MongoClient } from "mongodb";
import type { BackupJob } from "./validation";

export interface File {
    _id: string;
    content: BackupJob[];
}

const defaultUri = "mongodb://localhost:27017/backup-utilities";
export const mongo = new MongoClient(Bun.env.MONGO_URI ?? defaultUri);

await mongo.connect();

export const db = mongo.db();
export const collection = db.collection<File>("files");
