CREATE TABLE public.items (
	"id" int8 NOT NULL,
	"item_id" int4,
	"count" int4 DEFAULT -1,
	"init_count" int4 DEFAULT -1,
	"enchant_level" int4 DEFAULT 0,
	"grade_id" int4 DEFAULT 0,
	"equip_level" int4 DEFAULT 0,
	"period" int4 DEFAULT -1,
	"start_date" int4 DEFAULT 0,
	"reg_date" int4 DEFAULT 0,
	"end_date" int4 DEFAULT 0,
	"equip_state" boolean DEFAULT 'fal',
    PRIMARY KEY (id)
)
WITH (
    OIDS = FALSE
);

ALTER TABLE public.items
    OWNER to postgres;