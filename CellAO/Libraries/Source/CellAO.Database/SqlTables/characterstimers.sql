CREATE TABLE  `characterstimers` (
	`Id` int(32) NOT NULL AUTO_INCREMENT,
	`CharacterId` int(32) NOT NULL,
  `Strain` int(32) NOT NULL,
  `Timespan` int(32) NOT NULL,
  `Function` blob NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;