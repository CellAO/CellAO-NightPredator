CREATE TABLE  `charactersmeshs` (
	`Id` int(32) NOT NULL AUTO_INCREMENT,
	`CharacterId`int(32) NOT NULL,
  `Playfield` int(11) NOT NULL,
  `MeshValue1` int(11) NOT NULL,
  `MeshValue2` int(11) NOT NULL,
  `MeshValue3` int(11) NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;