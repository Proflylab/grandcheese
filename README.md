# GrandCheese

This is the source for an emulator for Grand Chase Season 5 (episode 3) called GrandCheese.

I might continue development in the future. It is a loose port of [lovemomory/GrandChaseSeasonV](https://github.com/lovemomory/GrandChaseSeasonV) to C#.

The database is PostgreSQL. Dapper and Npgsql are used to access the database.

Currently, there is enough to show a server list, but nothing more.

I did not worry about optimization while writing this. Please beware of the dozens of allocations because of LINQ.
