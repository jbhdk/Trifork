CREATE TABLE IF NOT EXISTS messages (
    id          bigint          GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    timestamp   timestamp       NOT NULL,
    count       int             NOT NULL
);
