CREATE TABLE  `mobspawnswaypoints` (
  `Id` int(32) NOT NULL,
  `Playfield` int(32) NOT NULL,
  `X` float NOT NULL,
  `Y` float NOT NULL,
  `Z` float NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;