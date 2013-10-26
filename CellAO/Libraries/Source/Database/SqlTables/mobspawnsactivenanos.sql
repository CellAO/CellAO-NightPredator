CREATE TABLE  `mobspawnsactivenanos` (
  `ID` int(10) unsigned NOT NULL,
  `nanoID` int(10) unsigned NOT NULL,
  `strain` int(10) unsigned NOT NULL,
  `Playfield` int(10) unsigned NOT NULL,
  PRIMARY KEY (`ID`,`Playfield`) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;