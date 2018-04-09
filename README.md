# GrandCheese

This is the source for an emulator for Grand Chase Season 5 (episode 3) called GrandCheese.

I might continue development in the future. It is a loose port of [lovemomory/GrandChaseSeasonV](https://github.com/lovemomory/GrandChaseSeasonV) to C#.

There is no database (yet?). The server has enough to get to login and return an "invalid account" dialog.

I did not worry about optimization while writing this. Please beware of the dozens of allocations because of LINQ.