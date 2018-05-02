CREATE TABLE public."characters" (
    "id" int8 NOT NULL,
    "user_id" int4,
    "character_type" int4,
    "promotion" int4 DEFAULT 0,
    "current_promotion" int4 DEFAULT 0,
    "exp" int8 DEFAULT 0,
    "win" int4 DEFAULT 0,
    "lose" int4 DEFAULT 0,
    "level" int4 DEFAULT 0,
    "weapon_id" int4 DEFAULT 0,
    "use_weapon" boolean DEFAULT 'fal',
    "slot_number" int4 DEFAULT 0,
    "game_points" int4 DEFAULT 3000,
    "bonus_points" int4 DEFAULT 3,
    "inventory_capacity" int4 DEFAULT 120,
    "look_inventory_capacity" int4 DEFAULT 120,
    PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
);

ALTER TABLE public."characters"
    OWNER to postgres;