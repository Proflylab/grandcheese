<?php
$packet = '';

$packet = str_replace(' ', '', $packet);
$packet = hex2bin($packet);

$packet = gzuncompress($packet);

echo bin2hex($packet);
