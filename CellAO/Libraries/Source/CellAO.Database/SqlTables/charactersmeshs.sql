CREATE TABLE  `charactersmeshs` (
	`ID` int(32) NOT NULL AUTO_INCREMENT,
	-- charID ??
  `playfield` int(11) NOT NULL,
  `meshvalue1` int(11) NOT NULL,
  `meshvalue2` int(11) NOT NULL,
  `meshvalue3` int(11) NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;