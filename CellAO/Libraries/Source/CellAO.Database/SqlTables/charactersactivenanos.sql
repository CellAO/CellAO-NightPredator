CREATE TABLE  `charactersactivenanos` (
	`ID` int(32) NOT NULL AUTO_INCREMENT,
	`CharacterID` int(32) NOT NULL,
	`NanoID` int(10) unsigned NOT NULL,
	`Strain` int(10) unsigned NOT NULL,
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;