CREATE TABLE public.characters
(
    id serial NOT NULL,
    user_id integer NOT NULL,
    promotion integer default 0,
    experience integer default 0,
    weapon_change integer,
    weapon_change_id integer,
    pet integer,
    PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
);

ALTER TABLE public.characters
    OWNER to postgres;