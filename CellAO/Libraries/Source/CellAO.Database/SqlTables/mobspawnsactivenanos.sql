CREATE TABLE  `mobspawnsactivenanos` (
  `Id` int(10) unsigned NOT NULL,
  `NanoId` int(10) unsigned NOT NULL,
  `Strain` int(10) unsigned NOT NULL,
  `Playfield` int(10) unsigned NOT NULL,
  PRIMARY KEY (`Id`,`Playfield`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;