# This Dockerfile must be run with the context of the root directory of the project!

FROM oven/bun:1.0-slim AS build
WORKDIR /app

COPY Server/package.json Server/bun.lockb ./
RUN bun install

COPY Server/src ./src
COPY Shared/config.schema.json ./src/schema.json

RUN bun build src/index.ts --compile --outfile server

FROM debian:bullseye-slim AS runtime
COPY --from=build /app/server .

CMD ["./server"]
