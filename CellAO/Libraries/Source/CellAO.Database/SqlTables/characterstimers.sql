CREATE TABLE  `characterstimers` (
	`ID` int(32) NOT NULL AUTO_INCREMENT,
	-- charID ??
  `strain` int(10) NOT NULL,
  `timespan` int(10) NOT NULL,
  `function` blob NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;