<?php
$packet = '78 01 63 60 00 02 D6 3A 1D 10 05 00 03 FF 00 B0 00 00 00';

$packet = str_replace(' ', '', $packet);
$packet = hex2bin($packet);

$packet = gzuncompress($packet);

echo bin2hex($packet);
