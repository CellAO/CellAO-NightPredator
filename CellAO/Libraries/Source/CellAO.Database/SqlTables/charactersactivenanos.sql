CREATE TABLE  `charactersactivenanos` (
	`Id` int(32) NOT NULL AUTO_INCREMENT,
	`CharacterId` int(32) NOT NULL,
	`NanoId` int(32) unsigned NOT NULL,
	`Strain` int(32) unsigned NOT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;