CREATE TABLE public.users
(
    id serial NOT NULL,
    username varchar(255) NOT NULL,
    password varchar(255) NOT NULL,
    email varchar(255) NOT NULL,
    nickname varchar(255),
    gender int2 DEFAULT 0,
    gp int4 DEFAULT 3000,
    PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
);

ALTER TABLE public.users
    OWNER to postgres;

CREATE UNIQUE INDEX "users_id" ON "public"."users" USING btree ("id", "username");
