CREATE TABLE  `mobspawns_stats` (
  `Id` int(32) NOT NULL,
  `Playfield` int(32) NOT NULL,
  `Stat` int(32) NOT NULL,
  `Value` int(32) NOT NULL,
  PRIMARY KEY (`Id`,`Playfield`,`Stat`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;