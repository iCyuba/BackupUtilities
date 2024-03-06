import { Hono } from "hono";
import { showRoutes } from "hono/dev";
import { logger } from "hono/logger";
import { validator } from "hono/validator";
import { nanoid } from "nanoid";

import { collection } from "./db";
import { validate } from "./validation";

const app = new Hono()
    .use("*", logger())

    .get("/", (c) => c.redirect("https://github.com/iCyuba/BackupUtilities"))

    .get("/:id", async (c) => {
        const _id = c.req.param("id");

        const file = await collection.findOne({ _id });
        if (!file) return c.json({ error: "File not found" }, { status: 404 });

        return c.json(file.content);
    })

    .post(
        "/",
        /** Validate the request body */
        validator("json", (value, c) => {
            if (!validate(value))
                return c.json(
                    {
                        error: "Invalid request body",
                        errors: validate.errors,
                    },
                    { status: 400 }
                );

            return value;
        }),

        /** Insert the data into the database */
        async (c) => {
            const buffer = await c.req.arrayBuffer();

            // Make sure the body is under 50kb
            if (buffer.byteLength > 50_000)
                return new Response("Request body is too large", {
                    status: 413,
                });

            const id = nanoid(5);
            await collection.insertOne({
                _id: id,
                content: await c.req.json(),
            });

            return c.json({ id }, { status: 201 });
        }
    );

showRoutes(app);

// Stop the server on Control + D (SIGTERM)
process.on("SIGTERM", () => process.exit(0));

export default app;
