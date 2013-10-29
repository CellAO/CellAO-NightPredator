CREATE TABLE  `mobspawns_stats` (
  `ID` int(32) NOT NULL,
  `Playfield` int(32) NOT NULL,
  `Stat` int(32) NOT NULL,
  `Value` int(32) NOT NULL,
  PRIMARY KEY (`ID`,`Playfield`,`Stat`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;