CREATE TABLE  `charactersuploadednanos` (
	`Id` int(32) NOT NULL AUTO_INCREMENT,
	`CharacterId` int(32) NOT NULL,

  `NanoId` int(11) NOT NULL,
  PRIMARY KEY (`ID`),
  INDEX `Nanos` (`CharacterId`, `NanoId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
