CREATE TABLE public.servers
(
    id serial NOT NULL,
    name text NOT NULL,
    description text NOT NULL,
    ip text NOT NULL,
    port integer NOT NULL,
    online_users integer DEFAULT 0,
    max_users integer DEFAULT 1000,
    protocol_version integer DEFAULT 327,
    active boolean DEFAULT 't',
    PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
);

ALTER TABLE public.servers
    OWNER to postgres;

CREATE UNIQUE INDEX "servers_id" ON "public"."servers" USING btree ("id", "name");

INSERT INTO "public"."servers" VALUES ('1', 'Cheddar', 'Test server', '127.0.0.1', '9401', '0', '1000', 't');

